using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : Menu
{
    public override void OnTouch()
    {
        SceneManager.LoadScene("InGame");
    }
}
