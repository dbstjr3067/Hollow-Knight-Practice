using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Game Settings")]
public class GameSettings : ScriptableObject
{
    public enum LanguageEnum{
        Korean,
        English,
        Japanese,
        Chinese
    }
    public LanguageEnum Language;
    public bool ShowAchivements;
}