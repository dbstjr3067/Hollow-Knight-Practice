using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class OptionMenu : Menu
{
    [SerializeField] protected List<string> options;
    [SerializeField] protected TMP_Text displayOption;
    protected int optionLength;
    protected int optionIndex;
    protected virtual void Start()
    {
        optionLength = options.Count;
    }
    public void TurnLeft()
    {
        optionIndex--;
        if(optionIndex < 0) optionIndex = optionLength - 1;
        OnChange();
    }
    public void TurnRight()
    {
        optionIndex++;
        if(optionIndex == optionLength) optionIndex = 0;
        OnChange();
    }
    protected virtual void OnChange()
    {
        displayOption.text = options[optionIndex];
    }
    public override void OnTouch()
    {
        TurnRight();
    }

}
