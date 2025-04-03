using TMPro;
using UnityEngine;

public class VolumeControlOptionButton : MonoBehaviour
{
    // This script is used to determine what text is displayed on the Volume Control button featured in the Options menu
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
        // This function is called when the player presses the button to toggle between volume on and volume off; It updates the button text accordingly
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
