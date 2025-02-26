using UnityEngine;

public enum HitBoxType
{
    PlayerIdle,
    PlayerRun,
    OnTrap,
    IdleTrap,
    BlinkTrap,
    SpikedBallTrap,
    NoDamageTrap,
    FinishPoint,
    MovingPlatform,
}

public struct HitBoxConfig
{
    public Vector2 sizeRatio;
    public Vector2 offsetRatio;


    public HitBoxConfig(float xSizeRatio, float ySizeRatio, float xOffsetRatio, float yOffsetRatio)
    {
        this.sizeRatio = new Vector2(xSizeRatio, ySizeRatio);
        this.offsetRatio = new Vector2(xOffsetRatio, yOffsetRatio);
    }
}

public static class HitBoxConfigManager
{
    public static HitBoxConfig GetHitBoxConfig(HitBoxType hitBoxType)
    {
        switch (hitBoxType)
        {
            case HitBoxType.PlayerIdle:
                return new HitBoxConfig(0.625f, 0.7711431f, 0f, -0.0916231f);
            case HitBoxType.PlayerRun:
                return new HitBoxConfig(0.625f, 0.7828325f, 0f, -0.05181431f);
            case HitBoxType.OnTrap:
                return new HitBoxConfig(1f, 1f, 0f, 0f);
            case HitBoxType.IdleTrap:
                return new HitBoxConfig(0.8110747f, 0.3671584f, -0.03154576f, -0.3164208f);
            case HitBoxType.BlinkTrap:
                return new HitBoxConfig(0.7120274f, 0.7460278f, 0.01171511f, -0.01415593f);
            case HitBoxType.SpikedBallTrap:
                return new HitBoxConfig(0.8928571f, 0.875f, 0.01632401f, -0.01767618f);
            case HitBoxType.NoDamageTrap:
                return new HitBoxConfig(0.7071428f, 0.7071428f, 0f, 0f);
            case HitBoxType.FinishPoint:
                return new HitBoxConfig(0.6899635f, 0.5842748f, 0f, -0.2078626f);
            case HitBoxType.MovingPlatform:
                return new HitBoxConfig(1f, 1f, 0f, 0f);
            default:
                throw new System.ArgumentException($"Invalid HitBoxType: {hitBoxType}");
        }
    }
}
