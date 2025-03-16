using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControlsOptionButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;

    // Update is called once per frame
    void Update()
    {
        if (PlayerControlsOption.instance.isOneHandMode == true)
        {
            buttonText.text = "Limited Dexterity: ON";
        }
        else
        {
            buttonText.text = "Limited Dexterity: OFF";
        }
    }
}
