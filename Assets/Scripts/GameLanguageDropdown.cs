using TMPro;
using UnityEngine;

public class GameLanguageDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown; // Reference to the Dropdown - TextMesh Pro
    public TMP_FontAsset[] fonts; // Array of TMP Font Assets


    void Start()
    {
        int index = dropdown.options.FindIndex(option => option.text == GameLanguageManager.gameLanguage);

        if (index != -1)
        {
            dropdown.value = index;
        }
        // Loop through each option and assign a different font
        for (int i = 0; i < dropdown.options.Count; i++)
        {
            TMP_Text itemText = dropdown.itemText;

            // If we have enough fonts, apply the font for each item
            if (i < fonts.Length)
            {
                itemText.font = fonts[i];
            }
        }
    }

    public void fontUpdate(int index)
    {
        // Updates the font when Dropdown option is changed
        switch (index)
        {
            case 0:
                dropdown.captionText.font = fonts[0];
                break;
            case 1:
                dropdown.captionText.font = fonts[1];
                break;
        }
        
    }
}
