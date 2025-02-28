using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

public class HitboxConfigTests
{
    private readonly string divider = "-------------------------------------------\n" +
                                      "-------------------------------------------\n";
    private const float allowedOffset = 0.04f;
    private const string PLAYER_NAME = "Player";
    private const string FINISH_POINT_NAME = "End (Pressed) (64x64)_0";
    private const string TRAP_PARENT_NAME = "Trap";
    private const string MOVING_PLATFORM_PARENT_NAME = "SupMove";
    private const string MOVING_PLATFORM_NAME = "On (32x10)_0";

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene(TestSettings.SceneIndex);
        string logFileName = TestSettings.TestLogFileName;
        string message = divider + $"Scene: {SceneManager.GetSceneByBuildIndex(TestSettings.SceneIndex).name}";
        TestLogger.Log(message, logFileName);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestPlayerHasCapsuleCollider()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestPlayerHasCapsuleCollider);
        TestLogger.Log(message, logFileName);

        GameObject player = GameObject.Find(PLAYER_NAME);
        Assert.IsNotNull(player, "Không tìm thấy Player trong scene.");

        var capsuleCollider = player.GetComponent<CapsuleCollider2D>();
        if (capsuleCollider == null)
        {
            string err = "Player không có CapsuleCollider2D.";
            TestLogger.Log(err, logFileName);
            Assert.IsNotNull(capsuleCollider, err);
        }

        string successMessage = $"{nameof(TestPlayerHasCapsuleCollider)} passed.";
        TestLogger.Log(successMessage, logFileName);
        Assert.Pass(successMessage);

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestPlayerHasPlayerScript()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestPlayerHasPlayerScript);
        TestLogger.Log(message, logFileName);

        GameObject player = GameObject.Find(PLAYER_NAME);
        Assert.IsNotNull(player, "Không tìm thấy Player trong scene.");

        var playerScript = player.GetComponent<Player>();
        if (playerScript == null)
        {
            string err = "Player không có script Player.cs.";
            TestLogger.Log(err, logFileName);
            Assert.IsNotNull(playerScript, err);
        }
        
        string successMessage = $"{nameof(TestPlayerHasPlayerScript)} passed.";
        TestLogger.Log(successMessage, logFileName);
        Assert.Pass(successMessage);

        yield return null;
    }


    [UnityTest]
    public IEnumerator TestPlayerHitboxParametersWhenIdle()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestPlayerHitboxParametersWhenIdle);
        TestLogger.Log(message, logFileName);

        GameObject player = GameObject.Find(PLAYER_NAME);

        var spriteRenderer = player.GetComponent<SpriteRenderer>();
        var sprite = spriteRenderer.sprite;
        Vector2 spriteSize = sprite.bounds.size;

        var collider = player.GetComponent<CapsuleCollider2D>();
        HitBoxConfig playerHitBoxConfig = HitBoxConfigManager.GetHitBoxConfig(HitBoxType.PlayerIdle);

        List<string> issues = new();

        Vector2 colliderSize = collider.size;
        Vector2 offset = collider.offset;
        CheckHitboxParameters(spriteSize, playerHitBoxConfig, issues, colliderSize, offset);

        if (issues.Count > 0)
        {
            string errorMessage = string.Join("\n", issues);
            TestLogger.Log(errorMessage, logFileName);
            Assert.Fail(errorMessage);
        }
        else
        {
            string successMessage = $"{nameof(TestPlayerHitboxParametersWhenIdle)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestPlayerHitboxParameterWhenMoving()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestPlayerHitboxParameterWhenMoving);
        TestLogger.Log(message, logFileName);

        GameObject player = GameObject.Find(PLAYER_NAME);
        var movementScript = player.GetComponent<dichuyen2>();
        movementScript.MoveLeft();

        yield return null;

        var spriteRenderer = player.GetComponent<SpriteRenderer>();
        var sprite = spriteRenderer.sprite;
        Vector2 spriteSize = sprite.bounds.size;

        var collider = player.GetComponent<CapsuleCollider2D>();
        HitBoxConfig playerHitBoxConfig = HitBoxConfigManager.GetHitBoxConfig(HitBoxType.PlayerRun);

        List<string> issues = new List<string>();

        Vector2 colliderSize = collider.size;
        Vector2 offset = collider.offset;
        CheckHitboxParameters(spriteSize, playerHitBoxConfig, issues, colliderSize, offset);

        if (issues.Count > 0)
        {
            string errorMessage = string.Join("\n", issues);
            TestLogger.Log(errorMessage, logFileName);
            Assert.Fail(errorMessage);
        }
        else
        {
            string successMessage = $"{nameof(TestPlayerHitboxParameterWhenMoving)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }
    }

    [UnityTest]
    public IEnumerator TestFinishPointHasBoxCollider()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestFinishPointHasBoxCollider);
        TestLogger.Log(message, logFileName);

        GameObject finishPoint = GameObject.Find(FINISH_POINT_NAME);
        Assert.IsNotNull(finishPoint, "Không tìm thấy FinishPoint trong scene.");

        var collider = finishPoint.GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            string err = "FinishPoint không có BoxCollider2D.";
            TestLogger.Log(err, logFileName);
            Assert.IsNotNull(collider, err);
        }

        string successMessage = $"{nameof(TestFinishPointHasBoxCollider)} passed.";
        TestLogger.Log(successMessage, logFileName);
        Assert.Pass(successMessage);

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestFinishPointHasWinScript()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestFinishPointHasWinScript);
        TestLogger.Log(message, logFileName);

        GameObject finishPoint = GameObject.Find(FINISH_POINT_NAME);
        Assert.IsNotNull(finishPoint, "Không tìm thấy FinishPoint trong scene.");

        var winScript = finishPoint.GetComponent<Win>();
        if (winScript == null)
        {
            string err = "FinishPoint không có script Win.cs.";
            TestLogger.Log(err, logFileName);
            Assert.IsNotNull(winScript, err);
        }

        string successMessage = $"{nameof(TestFinishPointHasWinScript)} passed.";
        TestLogger.Log(successMessage, logFileName);
        Assert.Pass(successMessage);

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestFinishPointHitboxParameters()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestFinishPointHitboxParameters);
        TestLogger.Log(message, logFileName);

        GameObject finishPoint = GameObject.Find(FINISH_POINT_NAME);

        var spriteRenderer = finishPoint.GetComponent<SpriteRenderer>();
        var sprite = spriteRenderer.sprite;
        Vector2 spriteSize = sprite.bounds.size;

        var collider = finishPoint.GetComponent<BoxCollider2D>();
        HitBoxConfig finishPointHitBoxConfig = HitBoxConfigManager.GetHitBoxConfig(HitBoxType.FinishPoint);

        List<string> issues = new();

        Vector2 colliderSize = collider.size;
        Vector2 offset = collider.offset;
        CheckHitboxParameters(spriteSize, finishPointHitBoxConfig, issues, colliderSize, offset);

        if (issues.Count > 0)
        {
            string errorMessage = string.Join("\n", issues);
            TestLogger.Log(errorMessage, logFileName);
            Assert.Fail(errorMessage);
        }
        else
        {
            string successMessage = $"{nameof(TestFinishPointHitboxParameters)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestTrapsHasAppropriateCollider()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestTrapsHasAppropriateCollider);
        TestLogger.Log(message, logFileName);
        List<string> issues = new();

        GameObject trapsParent = GameObject.Find(TRAP_PARENT_NAME);
        Assert.IsNotNull(trapsParent, "Không tìm thấy trapsParent trong scene.");

        foreach (Transform child in trapsParent.transform)
        {
            GameObject trap = child.gameObject;
            TrapType trapType = GetTrapType(trap.name);
            trap = trapType == TrapType.SpikedBall ? GetSpikedBallTrapObject(trap) : trap;
            bool hasCorrectCollider = IsTrapHasAppropriateCollider(trap, trapType, out string detail);
            if (!hasCorrectCollider)
            {
                issues.Add(detail);
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
            string successMessage = $"{nameof(TestTrapsHasAppropriateCollider)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestTrapsHasTrapTag()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestTrapsHasTrapTag);
        TestLogger.Log(message, logFileName);
        List<string> issues = new();

        GameObject trapsParent = GameObject.Find(TRAP_PARENT_NAME);
        Assert.IsNotNull(trapsParent, "Không tìm thấy trapsParent trong scene.");

        foreach (Transform child in trapsParent.transform)
        {
            GameObject trap = child.gameObject;
            TrapType trapType = GetTrapType(trap.name);
            trap = trapType == TrapType.SpikedBall ? GetSpikedBallTrapObject(trap) : trap;
            if (trapType == TrapType.NoDamage || trapType == TrapType.Unknown)
                continue;
            if (!trap.CompareTag("Trap"))
                issues.Add($"{trap.name} không có Tag 'Trap'");
        }
        if (issues.Count > 0)
        {
            string errorMessage = string.Join("\n", issues);
            TestLogger.Log(errorMessage, logFileName);
            Assert.Fail(errorMessage);
        }
        else
        {
            string successMessage = $"{nameof(TestTrapsHasTrapTag)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestTrapsHitboxParameters()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestTrapsHasTrapTag);
        TestLogger.Log(message, logFileName);
        List<string> issues = new();

        GameObject trapsParent = GameObject.Find(TRAP_PARENT_NAME);
        Assert.IsNotNull(trapsParent, "Không tìm thấy trapsParent trong scene.");

        foreach (Transform child in trapsParent.transform)
        {
            GameObject trap = child.gameObject;
            TrapType trapType = GetTrapType(trap.name);
            if (trapType == TrapType.Unknown) 
                continue;
            trap = trapType == TrapType.SpikedBall ? GetSpikedBallTrapObject(trap) : trap;

            var spriteRenderer = trap.GetComponent<SpriteRenderer>();
            var sprite = spriteRenderer.sprite;
            Vector2 spriteSize = sprite.bounds.size;

            var collider = trap.GetComponent<Collider2D>();

            Vector2 colliderSize = GetSize(collider);
            Vector2 offset = GetOffset(collider);

            List<string> trapIssues = new();

            HitBoxConfig hitboxConfig = HitBoxConfigManager.GetHitBoxConfig(GetTrapHitboxType(trapType));
            
            CheckHitboxParameters(spriteSize, hitboxConfig, trapIssues, colliderSize, offset);
            if (trapIssues.Count > 0)
            {
                issues.Add($"---{trap.name}: ");
                issues.AddRange(trapIssues);
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
            string successMessage = $"{nameof(TestTrapsHasTrapTag)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestMovingPlatformHasBoxCollider()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestMovingPlatformHasBoxCollider);
        TestLogger.Log(message, logFileName);

        GameObject parent = GameObject.Find(MOVING_PLATFORM_PARENT_NAME);
        Assert.IsNotNull(parent, "Không tìm thấy MovingPlatformsParent trong scene.");

        List<string> issues = new();

        foreach (Transform child in  parent.transform)
        {
            if (!child.gameObject.name.StartsWith(MOVING_PLATFORM_NAME))
            {
                continue;
            }
            if (!child.TryGetComponent<BoxCollider2D>(out var collider))
            {
                string issue = $"{child.name} không có BoxCollider2D.";
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
            string successMessage = $"{nameof(TestMovingPlatformHasBoxCollider)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestMovingPlatHasGroundTag()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestMovingPlatHasGroundTag);
        TestLogger.Log(message, logFileName);

        GameObject parent = GameObject.Find(MOVING_PLATFORM_PARENT_NAME);
        Assert.IsNotNull(parent, "Không tìm thấy MovingPlatformsParent trong scene.");

        List<string> issues = new();

        foreach (Transform child in parent.transform)
        {
            if (!child.gameObject.name.StartsWith(MOVING_PLATFORM_NAME))
            {
                continue;
            }
            if (!child.CompareTag("Ground"))
            {
                string issue = $"{child.name} không có Tag 'Ground'.";
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
            string successMessage = $"{nameof(TestMovingPlatHasGroundTag)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestMovingPlatHitboxParameters()
    {
        string logFileName = TestSettings.TestLogFileName;
        string message = nameof(TestMovingPlatHitboxParameters);
        TestLogger.Log(message, logFileName);

        GameObject parent = GameObject.Find(MOVING_PLATFORM_PARENT_NAME);
        Assert.IsNotNull(parent, "Không tìm thấy MovingPlatformsParent trong scene.");

        List<string> issues = new();

        foreach (Transform child in parent.transform)
        {
            if (!child.gameObject.name.StartsWith(MOVING_PLATFORM_NAME))
            {
                continue;
            }
            var spriteRenderer = child.GetComponent<SpriteRenderer>();
            var sprite = spriteRenderer.sprite;
            Vector2 spriteSize = sprite.bounds.size;

            var collider = child.GetComponent<BoxCollider2D>();
            HitBoxConfig hitBoxConfig = HitBoxConfigManager.GetHitBoxConfig(HitBoxType.MovingPlatform);

            List<string> childIssues = new();

            Vector2 colliderSize = collider.size;
            Vector2 offset = collider.offset;
            CheckHitboxParameters(spriteSize, hitBoxConfig, childIssues, colliderSize, offset);

            if (childIssues.Count > 0)
            {
                issues.Add($"---{child.name}: ");
                issues.AddRange(childIssues);
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
            string successMessage = $"{nameof(TestMovingPlatHitboxParameters)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
        }

        yield return null;
    }

    private GameObject GetSpikedBallTrapObject(GameObject trapParent)
    {
        var kill = trapParent.transform.Find("Kill");
        if (kill == null) return null;
        return kill.Find("Spiked Ball").gameObject;
    }

    public bool IsTrapHasAppropriateCollider(GameObject trap, TrapType trapType, out string detail)
    {
        if (!trap.TryGetComponent<Collider2D>(out var collider))
        {
            detail = $"{trap.name} không có Collider2D.";
            return false;
        }

        Type aprropriateColliderType = GetAppropriateColliderType(trapType);
        bool isCorrectCollider = collider.GetType() == aprropriateColliderType;

        if (isCorrectCollider)
        {
            detail = "";
            return true;
        }
        else
        {
            detail = $"{trap.name} không có Collider2D phù hợp. Expected: {aprropriateColliderType.Name}, Actual: {collider.GetType().Name}.";
            return false;
        }
    }

    public static Type GetAppropriateColliderType(TrapType trapType)
    {
        return trapType switch
        {
            TrapType.On 
            or TrapType.SpikedBall => typeof(CapsuleCollider2D),
            TrapType.Blink 
            or TrapType.Idle 
            or TrapType.NoDamage => typeof(BoxCollider2D),
            _ => null,
        };
    }

    private void CheckHitboxParameters(Vector2 spriteSize, HitBoxConfig playerHitBoxConfig, List<string> issues, Vector2 colliderSize, Vector2 colliderOffset)
    {
        float widthRatio = colliderSize.x / spriteSize.x;
        float heightRatio = colliderSize.y / spriteSize.y;

        if (Mathf.Abs(widthRatio - playerHitBoxConfig.sizeRatio.x) > allowedOffset)
        {
            float expectedWidth = playerHitBoxConfig.sizeRatio.x * spriteSize.x;
            string issue = $"Size X của HitBox không hợp lệ. Expected: {expectedWidth} +/- {allowedOffset * spriteSize.x}, Actual: {colliderSize.x}";
            issues.Add(issue);
        }

        if (Mathf.Abs(heightRatio - playerHitBoxConfig.sizeRatio.y) > allowedOffset)
        {
            float expectedHeight = playerHitBoxConfig.sizeRatio.y * spriteSize.y;
            string issue = $"Size Y của HitBox không hợp lệ. Expected: {expectedHeight} +/- {allowedOffset * spriteSize.y}, Actual: {colliderSize.y}";
            issues.Add(issue);
        }

        float offsetRatioX = colliderOffset.x / spriteSize.x;
        float offsetRatioY = colliderOffset.y / spriteSize.y;

        if (Mathf.Abs(offsetRatioX - playerHitBoxConfig.offsetRatio.x) > allowedOffset)
        {
            float expectedOffsetX = playerHitBoxConfig.offsetRatio.x * spriteSize.x;
            string issue = $"Offset X của HitBox không hợp lệ. Expected: {expectedOffsetX} +/- {allowedOffset * spriteSize.x}, Actual: {colliderOffset.x}";
            issues.Add(issue);
        }

        if (Mathf.Abs(offsetRatioY - playerHitBoxConfig.offsetRatio.y) > allowedOffset)
        {
            float expectedOffsetY = playerHitBoxConfig.offsetRatio.y * spriteSize.y;
            string issue = $"Offset Y của HitBox không hợp lệ. Expected: {expectedOffsetY} +/- {allowedOffset * spriteSize.y}, Actual: {colliderOffset.y}";
            issues.Add(issue);
        }
    }

    public TrapType GetTrapType(string trapName)
    {
        if (trapName.StartsWith("On (38x38)_0"))
        {
            return TrapType.On;
        }
        else if (trapName.StartsWith("Idle"))
        {
            return TrapType.Idle;
        }
        else if (trapName.StartsWith("Blink (54x52)_0"))
        {
            return TrapType.Blink;
        }
        else if (trapName.StartsWith("Bua"))
        {
            return TrapType.SpikedBall;
        }
        else if (trapName.StartsWith("Blink (42x42)_0"))
        {
            return TrapType.NoDamage;
        }
        else
        {
            return TrapType.Unknown;
        }
    }

    public HitBoxType GetTrapHitboxType(TrapType type)
    {
        return type switch
        {
            TrapType.On => HitBoxType.OnTrap,
            TrapType.Idle => HitBoxType.IdleTrap,
            TrapType.Blink => HitBoxType.BlinkTrap,
            TrapType.SpikedBall => HitBoxType.SpikedBallTrap,
            TrapType.NoDamage => HitBoxType.NoDamageTrap,
            _ => throw new ArgumentException("Unknown trap name"),
        };
    }

    public Vector2 GetSize(Collider2D collider)
    {
        switch (collider)
        {
            case BoxCollider2D boxCollider:
                return boxCollider.size;
            case CapsuleCollider2D capsuleCollider:
                return capsuleCollider.size;
            default:
                return Vector2.zero;
        }
    }

    public Vector2 GetOffset(Collider2D collider)
    {
        switch (collider)
        {
            case BoxCollider2D boxCollider:
                return boxCollider.offset;
            case CapsuleCollider2D capsuleCollider:
                return capsuleCollider.offset;
            default:
                return Vector2.zero;
        }
    }


}
