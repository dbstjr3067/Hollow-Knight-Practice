using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Audio Settings")]
public class AudioSettings : ScriptableObject
{
    public int master;
    public int sfx;
    public int music;
}