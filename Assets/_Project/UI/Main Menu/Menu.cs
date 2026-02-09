using UnityEngine;
using System.Collections;


public abstract class Menu : MonoBehaviour
{
    [Header("Menu Links")]
    [SerializeField] protected GameObject Up;
    [SerializeField] protected GameObject Down;
    [SerializeField] protected GameObject Left;
    [SerializeField] protected GameObject Right;
    [Header("Animation Object")]
    [SerializeField] protected GameObject MenuAnimation;
    void OnEnable(){
        MenuAnimation = transform.Find("MenuAnimation").gameObject;
    }
    public GameObject ChangeMenuByKey(MenuSelect.Direction direction){
        switch (direction)
        {
            case MenuSelect.Direction.Up:
                return Up;
            case MenuSelect.Direction.Down:
                return Down;
            case MenuSelect.Direction.Left:
                return Left;
            case MenuSelect.Direction.Right:
                return Right;
            default:
                return null;
        }
    }
    private IEnumerator SelectAnimation(){
        Animator animator = MenuAnimation.GetComponent<Animator>();
        if (animator != null) animator.Play("Select");
        yield return null;
    }
    private IEnumerator UnselectAnimation(){
        Animator animator = MenuAnimation.GetComponent<Animator>();
        if (animator != null) animator.Play("UnSelect");
        yield return null;
    }
    public abstract void OnTouch();
}
