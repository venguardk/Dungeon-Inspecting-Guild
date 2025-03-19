using TMPro;
using UnityEngine;

public class VolumeControlOptionButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonTextOn;
    [SerializeField] private TextMeshProUGUI buttonTextOff;


    // Update is called once per frame
    public void toggleVolumeText()
    {
        if (buttonTextOn.enabled)
        {
            buttonTextOn.enabled = false;
            buttonTextOff.enabled = true;
        }
        else if(buttonTextOff.enabled)
        {
            buttonTextOff.enabled = false;
            buttonTextOn.enabled = true;
        }
    }
}
