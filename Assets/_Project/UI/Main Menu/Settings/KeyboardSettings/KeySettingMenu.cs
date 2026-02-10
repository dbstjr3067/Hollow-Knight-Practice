using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class KeySettingMenu : Menu
{
    [Header("Key Settings")]
    [SerializeField] private TMP_Text KeyText;
    [SerializeField] private InputActionAsset Actions;
    [SerializeField] private InputActionReference currentAction = null;
    [SerializeField] private int bindingIndex = 0;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    void Awake(){
        ShowBindText();
    }
    public override void OnTouch()
    {
        RebindStart();
    }
    public void RebindStart()
    {
        currentAction.action.Disable();
        KeyText.text = "새 키 누르기";
        if(bindingIndex == 0){
            rebindingOperation = currentAction.action.PerformInteractiveRebinding()
                .WithCancelingThrough("<Keyboard>/escape")
                .OnCancel(operation => RebindCancel())
                .OnComplete(operation => RebindComplete())
                .Start();
        }
        else{
            rebindingOperation = currentAction.action.PerformInteractiveRebinding()
                .WithTargetBinding(bindingIndex)
                .WithCancelingThrough("<Keyboard>/escape")
                .OnCancel(operation => RebindCancel())
                .OnComplete(operation => RebindComplete())
                .Start();
        }
    }
    private void RebindCancel()
    {
        rebindingOperation.Dispose();
        currentAction.action.Enable();
        ShowBindText();
    }
    private void RebindComplete()
    {
        var newBinding = currentAction.action.bindings[bindingIndex];
        string newPath = newBinding.effectivePath;

        ResolveDuplicates(currentAction.action, bindingIndex, newPath);

        rebindingOperation.Dispose();
        currentAction.action.Enable();
        foreach(Transform menu in transform.parent.transform)
        {
            var sc = menu.gameObject.GetComponent<KeySettingMenu>();
            if (sc != null) sc.ShowBindText();
        }
    }
    public void ShowBindText()
    {
        var displayString = string.Empty;
        var deviceLayoutName = default(string);
        var controlPath = default(string);

        displayString = currentAction.action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath);
        KeyText.text = string.IsNullOrEmpty(displayString) ? "키 없음" : displayString;
    }
    
    private void ResolveDuplicates(
        InputAction targetAction,
        int targetBindingIndex,
        string newPath
    )
    {
        foreach (var map in Actions.actionMaps)
        {
            foreach (var action in map.actions)
            {
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    // 자기 자신은 제외
                    if (action == targetAction && i == targetBindingIndex)
                        continue;

                    var binding = action.bindings[i];

                    if (binding.effectivePath == newPath)
                    {
                        // ❗ 중복 발견 → None 처리
                        action.ApplyBindingOverride(i, string.Empty);
                    }
                }
            }
        }
    }
}