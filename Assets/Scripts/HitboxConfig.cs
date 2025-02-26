using UnityEngine;

public enum HitBoxType
{
    PlayerIdle,
    PlayerRun,
    OnTrap,
    IdleTrap,
    BlinkTrap,
    FinishPoint
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
                return new HitBoxConfig(0.8421053f, 0.8421053f, 0f, 0f);
            case HitBoxType.IdleTrap:
                return new HitBoxConfig(0.8768559f, 0.4066713f, -0.03114343f, -0.2966643f);
            case HitBoxType.BlinkTrap:
                return new HitBoxConfig(0.725241f, 0.7504812f, 0.008003799f, -0.01076666f);
            case HitBoxType.FinishPoint:
                return new HitBoxConfig(0.6899635f, 0.5842748f, 0f, -0.2078626f);
            default:
                throw new System.ArgumentException($"Invalid HitBoxType: {hitBoxType}");
        }
    }
}
