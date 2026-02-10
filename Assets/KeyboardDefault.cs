using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardDefault : Menu
{
    [Header("Key Reset")]
    [SerializeField] private InputActionAsset Actions;
    [SerializeField] private string Key_Up;
    [SerializeField] private string Key_Down;
    [SerializeField] private string Key_Left;
    [SerializeField] private string Key_Right;
    [SerializeField] private string Key_Jump;
    [SerializeField] private string Key_Attack;
    public override void OnTouch(){
        Actions.FindAction("Vertical").ApplyBindingOverride(1, Key_Down);
        Actions.FindAction("Vertical").ApplyBindingOverride(2, Key_Up);
        Actions.FindAction("Horizontal").ApplyBindingOverride(1, Key_Left);
        Actions.FindAction("Horizontal").ApplyBindingOverride(2, Key_Right);
        Actions.FindAction("Jump").ApplyBindingOverride(0, Key_Jump);
        Actions.FindAction("Attack").ApplyBindingOverride(0, Key_Attack);
        foreach(Transform menu in transform.parent.transform)
        {
            var sc = menu.gameObject.GetComponent<KeySettingMenu>();
            if (sc != null) sc.ShowBindText();
        }
    }
}
