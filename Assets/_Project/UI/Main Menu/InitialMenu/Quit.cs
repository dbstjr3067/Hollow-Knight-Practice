using UnityEngine;
using System.Collections;

public class Quit : Menu
{
    public override void OnTouch()
    {
        StartCoroutine( QuitGame() );
    }

    private IEnumerator QuitGame()
    {
        transform.parent.GetComponent<MenuSet>().MenuDisable();
        yield return new WaitForSeconds(1f);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;  
        #else
            Application.Quit();
        #endif
    }
}
