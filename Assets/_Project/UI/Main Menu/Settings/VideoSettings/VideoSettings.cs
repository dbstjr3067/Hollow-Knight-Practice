using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Video Settings")]
public class VideoSettings : ScriptableObject
{
    public enum fullScreenEnum{
        On,
        Borderless,
        Off
    }
    public enum blurEnum{
        High,
        Medium,
        Low
    }
    public enum frameSpeedLimitEnum{
        Off,
        F_30,
        F_60,
        F_120,
        F_180
    }

    public Resolution resolution;
    public fullScreenEnum fullScreen;
    public bool vsync;
    public bool particles;
    public blurEnum blur;
    public frameSpeedLimitEnum frameSpeedLimit;
}