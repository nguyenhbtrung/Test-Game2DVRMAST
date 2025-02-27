using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
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
                string issue = $"Player tại không đứng được trên platform tại {sequence.begin.position} ";
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

    private void HideTraps()
    {
        GameObject trapParent = GameObject.Find(TestHelper.TRAP_PARENT_NAME);
        if (trapParent != null)
        {
            trapParent.SetActive(false);
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
