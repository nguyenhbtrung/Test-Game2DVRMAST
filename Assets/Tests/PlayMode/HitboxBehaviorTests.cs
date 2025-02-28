using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using static HitboxConfigTests;
using static UnityEngine.UI.Dropdown;

public class HitboxBehaviorTests
{
    private const float positionThreshold = 0.1f;
    private List<ActionSequence> sequences;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene(TestSettings.SceneIndex);
        string logFileName = TestSettings.TestLogFileName;
        string message = TestHelper.divider + $"Scene: {TestSettings.SceneName}";
        TestLogger.Log(message, logFileName);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestInputSimulation()
    {
        GameObject player = GameObject.Find("Player");
        dichuyen2 playerMovement = player.GetComponent<dichuyen2>();
        TextAsset jsonText = Resources.Load<TextAsset>("InputData");
        if (jsonText != null)
        {
            InputSequencesData data = JsonUtility.FromJson<InputSequencesData>(jsonText.text);
            sequences = new List<ActionSequence>(data.actionSequences);
            Debug.Log("Loaded " + sequences.Count + " sequence(s).");
        }
        else
        {
            Debug.LogError("Không tìm thấy file InputData.json trong thư mục Resources.");
        }

        foreach (var sequence in sequences)
        {
            player.transform.position = sequence.begin.position;
            yield return new WaitForSeconds(0.2f);
            ProcessPlayerAction(playerMovement, sequence.begin);
            if (sequence.actions == null)
                continue;
            foreach (var actionData in sequence.actions)
            {
                yield return new WaitUntil(() =>
                    Vector2.Distance(player.transform.position, actionData.position) < positionThreshold);
                ProcessPlayerAction(playerMovement, actionData);
            }
        }

        yield return new WaitForSeconds(2);
    }

    [UnityTest]
    public IEnumerator TestPlayerBlockedByWall()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestPlayerBlockedByWall);
        TestLogger.Log(message, logFileName);

        GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
        dichuyen2 playerMovement = player.GetComponent<dichuyen2>();
        string file = $"{TestSettings.SceneName}-{nameof(TestPlayerBlockedByWall)}-InputData";
        LoadSimulatorInputData(file);

        HideTraps();
        HideMovingPlatforms();

        GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
        List<string> issues = new();

        foreach (var sequence in sequences)
        {
            player.transform.position = sequence.begin.position;
            cam.transform.position = player.transform.position + Vector3.back * 10;
            ProcessPlayerAction(playerMovement, sequence.begin);
            if (sequence.actions != null)
                foreach (var actionData in sequence.actions)
                {
                    yield return new WaitUntil(() =>
                        Vector2.Distance(player.transform.position, actionData.position) < positionThreshold);
                    ProcessPlayerAction(playerMovement, actionData);

                }
            yield return new WaitForSeconds(0.7f);
            Vector3 prePos = player.transform.position;
            yield return new WaitForSeconds(0.2f);
            Vector3 currentPos = player.transform.position;
            if (prePos.x != currentPos.x)
            {
                string issue = $"Player tại {sequence.begin.position} không bị chặn bởi tường";
                issues.Add(issue);
            }
        }

        if (issues.Count > 0)
        {
            string errorMessage = string.Join("\n", issues);
            TestLogger.Log(errorMessage, logFileName);
            Assert.Fail(errorMessage);
        }
        else
        {
            string successMessage = $"{nameof(TestPlayerBlockedByWall)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestPlayerStandOnStaticPlatform()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestPlayerStandOnStaticPlatform);
        TestLogger.Log(message, logFileName);

        GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
        dichuyen2 playerMovement = player.GetComponent<dichuyen2>();
        string file = $"{TestSettings.SceneName}-{nameof(TestPlayerStandOnStaticPlatform)}-InputData";
        LoadSimulatorInputData(file);

        HideTraps();
        HideMovingPlatforms();

        GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
        List<string> issues = new();

        foreach (var sequence in sequences)
        {
            player.transform.position = sequence.begin.position;
            cam.transform.position = player.transform.position + Vector3.back * 10;
            ProcessPlayerAction(playerMovement, sequence.begin);
            
            yield return new WaitForSeconds(0.7f);
            
            if (player.GetComponent<Rigidbody2D>().velocity.magnitude > 0.01)
            {
                string issue = $"Player không đứng được trên platform tại {sequence.begin.position} ";
                issues.Add(issue);
            }

        }

        if (issues.Count > 0)
        {
            string errorMessage = string.Join("\n", issues);
            TestLogger.Log(errorMessage, logFileName);
            Assert.Fail(errorMessage);
        }
        else
        {
            string successMessage = $"{nameof(TestPlayerStandOnStaticPlatform)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestPlayerStandOnMovingPlatform()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestPlayerStandOnMovingPlatform);
        TestLogger.Log(message, logFileName);

        GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
        GameObject parent = GameObject.Find(TestHelper.MOVING_PLATFORM_PARENT_NAME);
        Assert.IsNotNull(parent, "Không tìm thấy MovingPlatformsParent trong scene.");

        GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
        List<string> issues = new();

        foreach (Transform child in parent.transform)
        {
            if (!child.gameObject.name.StartsWith(TestHelper.MOVING_PLATFORM_NAME))
            {
                continue;
            }
            Vector3 newPos = child.position + Vector3.up * 0.3f;
            player.transform.position = newPos;
            cam.transform.position = player.transform.position + Vector3.back * 10;

            yield return new WaitForSeconds(2);

            float distance = Vector2.Distance(player.transform.position, child.position);

            if ((player.transform.position.y < child.position.y) || (distance > 0.3f))
            {
                string issue = $"Player không đứng được trên platform động {child.name}. {distance} {player.transform.position} {child.position}";
                issues.Add(issue);
            }
        }

        if (issues.Count > 0)
        {
            string errorMessage = string.Join("\n", issues);
            TestLogger.Log(errorMessage, logFileName);
            Assert.Fail(errorMessage);
        }
        else
        {
            string successMessage = $"{nameof(TestPlayerStandOnMovingPlatform)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestPlayerStandUnderVerticalMovingPlatform()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestPlayerStandUnderVerticalMovingPlatform);
        TestLogger.Log(message, logFileName);

        GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
        dichuyen2 playerMovement = player.GetComponent<dichuyen2>();
        string file = $"{TestSettings.SceneName}-{nameof(TestPlayerStandUnderVerticalMovingPlatform)}-InputData";
        LoadSimulatorInputData(file);

        HideTraps();

        GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
        List<string> issues = new();

        foreach (var sequence in sequences)
        {
            player.transform.position = sequence.begin.position;
            cam.transform.position = player.transform.position + Vector3.back * 10;
            ProcessPlayerAction(playerMovement, sequence.begin);

            Transform movingPlatform = GetMovingPlatformAbove(sequence.begin.position, maxDistance: 5f);

            if (movingPlatform != null)
            {
                float initialPlatformY = movingPlatform.position.y;

                bool hasMovedDown = false;
                bool hasMovedUpAgain = false;

                float movementThreshold = 0.1f;

                while (!hasMovedDown || !hasMovedUpAgain)
                {
                    float currentPlatformY = movingPlatform.position.y;

                    if (!hasMovedDown && currentPlatformY < initialPlatformY - movementThreshold)
                    {
                        hasMovedDown = true;
                    }

                    if (hasMovedDown && currentPlatformY >= initialPlatformY - movementThreshold)
                    {
                        hasMovedUpAgain = true;
                    }

                    yield return null;
                }
            }
            yield return new WaitForSeconds(0.5f);

            if (player.transform.position.y - sequence.begin.position.y > 0.2f)
            {
                string issue = $"Player bị đẩy lên platform động tại {sequence.begin.position}";
                issues.Add(issue);
            }
        }
        if (issues.Count > 0)
        {
            string errorMessage = string.Join("\n", issues);
            TestLogger.Log(errorMessage, logFileName);
            Assert.Fail(errorMessage);
        }
        else
        {
            string successMessage = $"{nameof(TestPlayerStandUnderVerticalMovingPlatform)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestIsGroundedStatusBeforeAndAfterJump()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestIsGroundedStatusBeforeAndAfterJump);
        TestLogger.Log(message, logFileName);

        GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
        dichuyen2 playerMovement = player.GetComponent<dichuyen2>();
       
        HideTraps();
        HideMovingPlatforms();

        //GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
        List<string> issues = new();

        player.transform.Translate(0.4f, 0, 0);
        yield return new WaitForSeconds(0.5f);

        FieldInfo isGroundedField = typeof(dichuyen2)
            .GetField("isGrounded", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsNotNull(isGroundedField, "Không tìm thấy trường isGrounded trong PlayerController!");

        bool groundedBefore = (bool)isGroundedField.GetValue(playerMovement);
        if (!groundedBefore)
        {
            issues.Add("IsGrounded = false khi đứng trên mặt đất trước khi nhảy");
        }

        playerMovement.JumpButton();
        yield return null;

        bool groundedAfter = (bool)isGroundedField.GetValue(playerMovement);
        if (groundedAfter)
        {
            issues.Add("IsGrounded = true sau khi nhảy");
        }

        yield return new WaitForSeconds(1f);
        bool groundedWhenGrounding = (bool)isGroundedField.GetValue(playerMovement);
        if (!groundedWhenGrounding)
        {
            issues.Add("IsGrounded = false khi tiếp đất");
        }


        if (issues.Count > 0)
        {
            string errorMessage = string.Join("\n", issues);
            TestLogger.Log(errorMessage, logFileName);
            Assert.Fail(errorMessage);
        }
        else
        {
            string successMessage = $"{nameof(TestIsGroundedStatusBeforeAndAfterJump)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestPlayerCollisionWithTrapReceiveDamage()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestPlayerCollisionWithTrapReceiveDamage);
        TestLogger.Log(message, logFileName);
        List<string> issues = new();

        GameObject trapsParent = GameObject.Find(TestHelper.TRAP_PARENT_NAME);
        Assert.IsNotNull(trapsParent, "Không tìm thấy trapsParent trong scene.");

        List<string> trapNames = new();
        foreach (Transform child in trapsParent.transform)
        {
            GameObject trap = child.gameObject;
            TrapType trapType = TestHelper.GetTrapType(trap.name);
            trap = trapType == TrapType.SpikedBall ? TestHelper.GetSpikedBallTrapObject(trap) : trap;
            if (trapType == TrapType.NoDamage || trapType == TrapType.Unknown)
                continue;
            trapNames.Add(trap.name);
        }
        foreach (string trapName in trapNames)
        {
            SceneManager.LoadScene(TestSettings.SceneIndex);
            yield return null;

            GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
            GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
            Player playerScript = player.GetComponent<Player>();
            GameObject trap = GameObject.Find(trapName);
            if (trap == null)
                continue;

            int inititalLives = playerScript.lives;

            player.transform.position = trap.transform.position;
            cam.transform.position = player.transform.position + Vector3.back * 10;

            yield return new WaitForFixedUpdate();
            int livesAfterCollision = playerScript.lives;
            int damage = inititalLives - livesAfterCollision;
            int expectedDamage = 1;
            if (damage != expectedDamage)
            {
                string issue = $"Player va chạm với bẫy {trapName} không nhận đúng thiệt hại. " +
                    $"\nExpected: Damage = {expectedDamage}. " +
                    $"\nActual: Damage = {damage}.";
                issues.Add(issue);
            }
        }
        if (issues.Count > 0)
        {
            string errorMessage = string.Join("\n", issues);
            TestLogger.Log(errorMessage, logFileName);
            Assert.Fail(errorMessage);
        }
        else
        {
            string successMessage = $"{nameof(TestPlayerCollisionWithTrapReceiveDamage)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestPlayerCollisionWithTrapDeactivated()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestPlayerCollisionWithTrapDeactivated);
        TestLogger.Log(message, logFileName);
        List<string> issues = new();

        GameObject trapsParent = GameObject.Find(TestHelper.TRAP_PARENT_NAME);
        Assert.IsNotNull(trapsParent, "Không tìm thấy trapsParent trong scene.");

        List<string> trapNames = new();
        foreach (Transform child in trapsParent.transform)
        {
            GameObject trap = child.gameObject;
            TrapType trapType = TestHelper.GetTrapType(trap.name);
            trap = trapType == TrapType.SpikedBall ? TestHelper.GetSpikedBallTrapObject(trap) : trap;
            if (trapType == TrapType.NoDamage || trapType == TrapType.Unknown)
                continue;
            trapNames.Add(trap.name);
        }
        foreach (string trapName in trapNames)
        {
            SceneManager.LoadScene(TestSettings.SceneIndex);
            yield return null;

            GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
            GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
            Player playerScript = player.GetComponent<Player>();
            GameObject trap = GameObject.Find(trapName);
            if (trap == null)
                continue;

            player.transform.position = trap.transform.position;
            cam.transform.position = player.transform.position + Vector3.back * 10;

            yield return new WaitForFixedUpdate();
            if (playerScript.CanMove())
            {
                string issue = $"Player va chạm với bẫy {trapName} không bị ngừng hoạt động. ";
                issues.Add(issue);
            }
        }
        if (issues.Count > 0)
        {
            string errorMessage = string.Join("\n", issues);
            TestLogger.Log(errorMessage, logFileName);
            Assert.Fail(errorMessage);
        }
        else
        {
            string successMessage = $"{nameof(TestPlayerCollisionWithTrapDeactivated)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestPlayerCollisionWithTrapDeadAnimation()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestPlayerCollisionWithTrapDeadAnimation);
        TestLogger.Log(message, logFileName);
        List<string> issues = new();

        GameObject trapsParent = GameObject.Find(TestHelper.TRAP_PARENT_NAME);
        Assert.IsNotNull(trapsParent, "Không tìm thấy trapsParent trong scene.");

        List<string> trapNames = new();
        foreach (Transform child in trapsParent.transform)
        {
            GameObject trap = child.gameObject;
            TrapType trapType = TestHelper.GetTrapType(trap.name);
            trap = trapType == TrapType.SpikedBall ? TestHelper.GetSpikedBallTrapObject(trap) : trap;
            if (trapType == TrapType.NoDamage || trapType == TrapType.Unknown)
                continue;
            trapNames.Add(trap.name);
        }
        foreach (string trapName in trapNames)
        {
            SceneManager.LoadScene(TestSettings.SceneIndex);
            yield return null;

            GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
            GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
            Player playerScript = player.GetComponent<Player>();
            GameObject trap = GameObject.Find(trapName);
            if (trap == null)
                continue;

            player.transform.position = trap.transform.position;
            cam.transform.position = player.transform.position + Vector3.back * 10;

            yield return new WaitForFixedUpdate();
            float duration = 0f;
            bool isDeadAnimation = false;
            while (duration <= 1)
            { 
                if (playerScript.playerSprite.color == playerScript.damagedColor)
                {
                    isDeadAnimation = true;
                    break;
                }
                duration += Time.deltaTime;
                yield return null;
            }
            if (!isDeadAnimation)
            {
                string issue = $"Player va chạm với bẫy {trapName} không hiện dead animation. ";
                issues.Add(issue);
            }
        }
        if (issues.Count > 0)
        {
            string errorMessage = string.Join("\n", issues);
            TestLogger.Log(errorMessage, logFileName);
            Assert.Fail(errorMessage);
        }
        else
        {
            string successMessage = $"{nameof(TestPlayerCollisionWithTrapDeadAnimation)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestPlayerCollisionWithTrapUntilRunsOutOfLives()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestPlayerCollisionWithTrapUntilRunsOutOfLives);
        TestLogger.Log(message, logFileName);
        List<string> issues = new();

        GameObject trapsParent = GameObject.Find(TestHelper.TRAP_PARENT_NAME);
        Assert.IsNotNull(trapsParent, "Không tìm thấy trapsParent trong scene.");

        GameObject selectedTrap = null;
        foreach (Transform child in trapsParent.transform)
        {
            GameObject trap = child.gameObject;
            TrapType trapType = TestHelper.GetTrapType(trap.name);
            trap = trapType == TrapType.SpikedBall ? TestHelper.GetSpikedBallTrapObject(trap) : trap;
            if (trapType == TrapType.NoDamage || trapType == TrapType.Unknown)
                continue;
            selectedTrap = trap;
            break;
        }

        GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
        GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
        Player playerScript = player.GetComponent<Player>();

        while (playerScript.lives > 0)
        {
            int inititalLives = playerScript.lives;

            player.transform.position = selectedTrap.transform.position;
            cam.transform.position = player.transform.position + Vector3.back * 10;

            yield return new WaitForFixedUpdate();

            int livesAfterCollision = playerScript.lives;
            int damage = inititalLives - livesAfterCollision;
            int expectedDamage = 1;

            if (livesAfterCollision == 0)
            {
                playerScript.StopAllCoroutines();
            }

            if (damage != expectedDamage)
            {
                string issue = $"Player va chạm với bẫy không nhận đúng thiệt hại khi còn {inititalLives} mạng. " +
                    $"\nExpected: Damage = {expectedDamage}. " +
                    $"\nActual: Damage = {damage}.";
                issues.Add(issue);
            }
            if (playerScript.CanMove())
            {
                string issue = $"Player va chạm với bẫy không bị ngừng hoạt động khi còn {inititalLives} mạng. ";
                issues.Add(issue);
            }

            float duration = 0f;
            bool isDeadAnimation = false;
            while (duration <= 1)
            {
                if (playerScript.playerSprite.color == playerScript.damagedColor)
                {
                    isDeadAnimation = true;
                    break;
                }
                duration += Time.deltaTime;
                yield return null;
            }
            if (!isDeadAnimation)
            {
                string issue = $"Player va chạm với bẫy không hiện dead animation khi còn {inititalLives} mạng. ";
                issues.Add(issue);
            }
            yield return new WaitForSeconds(2);
        }

        if (issues.Count > 0)
        {
            string errorMessage = string.Join("\n", issues);
            TestLogger.Log(errorMessage, logFileName);
            Assert.Fail(errorMessage);
        }
        else
        {
            string successMessage = $"{nameof(TestPlayerCollisionWithTrapUntilRunsOutOfLives)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestPlayerCollisionWithNoDamegeTrap()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestPlayerCollisionWithNoDamegeTrap);
        TestLogger.Log(message, logFileName);

        GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
        dichuyen2 playerMovement = player.GetComponent<dichuyen2>();
        string file = $"{TestSettings.SceneName}-{nameof(TestPlayerCollisionWithNoDamegeTrap)}-InputData";
        LoadSimulatorInputData(file);

        HideTraps(true);
        HideMovingPlatforms();

        GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
        List<string> issues = new();

        foreach (var sequence in sequences)
        {
            player.transform.position = sequence.begin.position;
            cam.transform.position = player.transform.position + Vector3.back * 10;
            ProcessPlayerAction(playerMovement, sequence.begin);
            yield return new WaitForSeconds(0.2f);
            Vector3 prePos = player.transform.position;

            yield return new WaitForSeconds(1.5f);

            if (player.transform.position == prePos)
            {
                string issue = $"Player bị xuyên qua NoDamageTrap tại {sequence.begin.position} ";
                issues.Add(issue);
            }

        }

        if (issues.Count > 0)
        {
            string errorMessage = string.Join("\n", issues);
            TestLogger.Log(errorMessage, logFileName);
            Assert.Fail(errorMessage);
        }
        else
        {
            string successMessage = $"{nameof(TestPlayerCollisionWithNoDamegeTrap)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestPlayerCollisionFinishPoint()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestPlayerCollisionFinishPoint);
        TestLogger.Log(message, logFileName);

        GameObject finishPoint = GameObject.Find(TestHelper.FINISH_POINT_NAME);
        Assert.IsNotNull(finishPoint, "Không tìm thấy FinishPoint trong scene.");
        Win win = finishPoint.GetComponent<Win>();

        GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);

        player.transform.position = finishPoint.transform.position;
        yield return null;
        
        if (!win.gameWinIU.activeInHierarchy)
        {
            string err = "Màn hình gameWin không hiển thị khi Player va chạm với FinishPoint.";
            TestLogger.Log(err, logFileName);
            Assert.Fail(err);
        }

        string successMessage = $"{nameof(TestPlayerCollisionFinishPoint)} passed.";
        TestLogger.Log(successMessage, logFileName);
        Assert.Pass(successMessage);

        yield return null;
    }

    private void HideTraps(bool exceptNoDamage = false)
    {
        if (exceptNoDamage)
        {
            GameObject trapParent = GameObject.Find(TestHelper.TRAP_PARENT_NAME);
            foreach (Transform child in trapParent.transform)
            {
                GameObject trap = child.gameObject;
                TrapType trapType = TestHelper.GetTrapType(trap.name);
                if (trapType != TrapType.NoDamage)
                    trap.SetActive(false);
            }
        }
        else
        {
            GameObject trapParent = GameObject.Find(TestHelper.TRAP_PARENT_NAME);
            if (trapParent != null)
            {
                trapParent.SetActive(false);
            }
        }
    }

    private void HideMovingPlatforms()
    {
        GameObject movingPlatformParent = GameObject.Find(TestHelper.MOVING_PLATFORM_PARENT_NAME);
        if (movingPlatformParent != null)
        {
            movingPlatformParent.SetActive(true);movingPlatformParent.SetActive(false);
        }
    }

    private Transform GetMovingPlatformAbove(Vector3 position, float maxDistance)
    {
        Vector2 origin = position;
        Vector2 direction = Vector2.up;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance);

        if (hit.collider != null)
        {
            string hitObjectName = hit.collider.gameObject.name;
            if (hitObjectName.StartsWith(TestHelper.MOVING_PLATFORM_NAME))
            {
                return hit.collider.transform;
            }
        }
        return null;
    }



    private void LoadSimulatorInputData(string jsonFileName)
    {
        TextAsset jsonText = Resources.Load<TextAsset>(jsonFileName);
        if (jsonText != null)
        {
            InputSequencesData data = JsonUtility.FromJson<InputSequencesData>(jsonText.text);
            sequences = new List<ActionSequence>(data.actionSequences);
            Debug.Log("Loaded " + sequences.Count + " sequence(s).");
        }
        else
        {
            Debug.LogError("Không tìm thấy file InputData.json trong thư mục Resources.");
        }
    }

    private void ProcessPlayerAction(dichuyen2 playerMovement, InputActionData actionData)
    {
        switch (actionData.action)
        {
            case "left":
                playerMovement.MoveLeft();
                break;
            case "right":
                playerMovement.MoveRight();
                break;
            case "jump":
                playerMovement.JumpButton();
                break;
            case "stopLeft":
                playerMovement.StopMoveLeft();
                break;
            case "stopRight":
                playerMovement.StopMoveRight();
                break;
            case "none":
                break;
            default:
                Debug.LogWarning("Hành động không xác định: " + actionData.action);
                break;
        }
    }
}

[System.Serializable]
public class InputActionData
{
    public Vector2 position;
    public string action;
}

[System.Serializable]
public class ActionSequence
{
    public InputActionData begin;
    public InputActionData[] actions;
}

[System.Serializable]
public class InputSequencesData
{
    public ActionSequence[] actionSequences;
}
