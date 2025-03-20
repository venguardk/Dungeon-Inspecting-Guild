using TMPro;
using UnityEngine;

public class VolumeControlOptionButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonTextOn;
    [SerializeField] private TextMeshProUGUI buttonTextOff;

    private void Start()
    {
        // Initialize the button text based on the current volume state
        if (VolumeControlOption.instance.VolumeOn)
        {
            buttonTextOn.enabled = true;
            buttonTextOff.enabled = false;
        }
        else
        {
            buttonTextOn.enabled = false;
            buttonTextOff.enabled = true;
        }
    }

    public void ToggleVolumeText()
    {
        if (buttonTextOn.enabled)
        {
            buttonTextOn.enabled = false;
            buttonTextOff.enabled = true;
        }
        else if (buttonTextOff.enabled)
        {
            buttonTextOff.enabled = false;
            buttonTextOn.enabled = true;
        }
    }
}
