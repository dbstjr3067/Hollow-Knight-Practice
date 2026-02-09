using UnityEngine;

public class MenuSelect : MonoBehaviour
{
    [SerializeField] private GameObject Selected;
    [SerializeField] private GameObject CurrentMenuSet;
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public void ChangeMenuByKey(GameObject selected, Direction direction){
        Menu menu = selected.GetComponent<Menu>();
        GameObject nextMenu = menu.ChangeMenuByKey(direction);
        if (nextMenu != null){
            Selected.GetComponent<Menu>().StartCoroutine("UnselectAnimation");
            nextMenu.GetComponent<Menu>().StartCoroutine("SelectAnimation");
            Selected = nextMenu;
        }
    }
    public void ChangeMenu(GameObject selected){
        Selected.GetComponent<Menu>().StartCoroutine("UnselectAnimation");
        selected.GetComponent<Menu>().StartCoroutine("SelectAnimation");
        Selected = selected;
        //Debug.Log("Menu changed to " + selected.name);
    }
    public void ChangeMenuSet(GameObject newMenuSet){
        Selected.GetComponent<Menu>().StartCoroutine("UnselectAnimation");
        newMenuSet.SetActive(true);
        newMenuSet.GetComponent<MenuSet>().MenuEnable();
        CurrentMenuSet.GetComponent<MenuSet>().MenuDisable();
        Selected = newMenuSet.GetComponent<MenuSet>().InitialMenu;
        CurrentMenuSet = newMenuSet;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)){
            ChangeMenuByKey(Selected, Direction.Up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)){
            ChangeMenuByKey(Selected, Direction.Down);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)){
            ChangeMenuByKey(Selected, Direction.Left);
            OptionMenu option = Selected.GetComponent<OptionMenu>();
            if(option != null) option.TurnLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)){
            ChangeMenuByKey(Selected, Direction.Right);
            OptionMenu option = Selected.GetComponent<OptionMenu>();
            if(option != null) option.TurnRight();
        }
        if (Input.GetKeyDown(KeyCode.Return)){
            Selected.GetComponent<Menu>().OnTouch();
        }
    }
}
