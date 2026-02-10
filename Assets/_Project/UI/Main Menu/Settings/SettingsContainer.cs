using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Settings Container")]
public class SettingsContainer : ScriptableObject
{
    public GameSettings game;
    public VideoSettings video;
    public AudioSettings audio;
}