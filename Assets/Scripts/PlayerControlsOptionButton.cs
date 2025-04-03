using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.LowLevel;

public class PlayerControlsOptionButton : MonoBehaviour
{
    // This script is used to determine what text is displayed on the Limited Dexterity button in the Options menu
    [SerializeField] private TextMeshProUGUI buttonText;

    void Update()
    {
        if (PlayerControlsOption.instance.isOneHandMode == true)
        {
            if (GameLanguageManager.gameLanguage == "日本語")
            {
                buttonText.font = PlayerControlsOption.instance.fontJap;
                buttonText.text = "片手操作";
            }
            else
            {
                buttonText.font = PlayerControlsOption.instance.fontEng;
                buttonText.text = "Limited Dexterity: ON";
            }
        }
        else
        {
            if (GameLanguageManager.gameLanguage == "日本語")
            {
                buttonText.font = PlayerControlsOption.instance.fontJap;
                buttonText.text = "通常操作";
            }
            else
            {
                buttonText.font = PlayerControlsOption.instance.fontEng;
                buttonText.text = "Limited Dexterity: OFF";
            }

        }
    }
}
