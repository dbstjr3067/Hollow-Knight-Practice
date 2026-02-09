using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private void Awake(){
        if(instance == null) instance = this;
    }
}
