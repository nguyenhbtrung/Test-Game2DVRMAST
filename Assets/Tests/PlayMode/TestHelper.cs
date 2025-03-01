using HitboxBehaviorTests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TestHelper
{
    public const string divider = "-------------------------------------------\n" +
                                      "-------------------------------------------\n";
    public const float allowedOffset = 0.04f;
    public const string PLAYER_NAME = "Player";
    public const string FINISH_POINT_NAME = "End (Pressed) (64x64)_0";
    public const string TRAP_PARENT_NAME = "Trap";
    public const string MOVING_PLATFORM_PARENT_NAME = "SupMove";
    public const string MOVING_PLATFORM_NAME = "On (32x10)_0";

    public static TrapType GetTrapType(string trapName)
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

    public static GameObject GetSpikedBallTrapObject(GameObject trapParent)
    {
        var kill = trapParent.transform.Find("Kill");
        if (kill == null) return null;
        return kill.Find("Spiked Ball").gameObject;
    }

    public static Dictionary<string, List<string>> TestsToSkipBySceneIndex = new Dictionary<string, List<string>>()
    {
        { "Game1", new List<string> { nameof(PlatformHitboxBehaviorTests.TestPlayerStandUnderVerticalMovingPlatform) } },
    };

    public static bool ShouldSkipTest(string testName)
    {
        if (TestsToSkipBySceneIndex.TryGetValue(TestSettings.SceneName, out List<string> testsToSkip))
        {
            return testsToSkip.Contains(testName);
        }
        return false;
    }
}

public enum TrapType
{
    On,
    Idle,
    Blink,
    SpikedBall,
    NoDamage,
    Unknown
}
