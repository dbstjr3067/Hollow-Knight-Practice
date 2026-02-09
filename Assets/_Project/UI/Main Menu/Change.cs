using UnityEngine;

public class Change : Menu
{
    [Header("Change Menu Set")]
    [SerializeField] private GameObject SetToChange;
    MenuSelect MenuSelect;
    void Awake(){
        MenuSelect = GameObject.Find("MenuSelect").GetComponent<MenuSelect>();
    }
    public override void OnTouch()
    {
        MenuSelect.ChangeMenuSet(SetToChange);
    }
}
