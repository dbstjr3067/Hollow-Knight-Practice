using UnityEngine;

public class Resolutions : OptionMenu
{
    Resolution[] resolutions;
    [SerializeField] GameObject ApplyButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRateRatio + "Hz";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                displayOption.text = option;
            }
        }
        optionLength = options.Count;
    }
    protected override void OnChange(){
        if (resolutions[optionIndex].width == Screen.currentResolution.width &&
            resolutions[optionIndex].height == Screen.currentResolution.height) ApplyButton.SetActive(false);
        else ApplyButton.SetActive(true);
        displayOption.text = options[optionIndex];
    }
    public void ApplyResolutions(){
        Resolution resolution = resolutions[optionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        ApplyButton.SetActive(false);
    }
}
