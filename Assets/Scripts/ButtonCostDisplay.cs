using UnityEngine;
using TMPro;

public class ButtonCostDisplay : MonoBehaviour
{
    // This script is used to display the cost of the asset in the level editor UI under the assets and threats tabs
    // Add this script to each asset button object
    // Values can be adjusted in the Unity inspector
    [SerializeField] private int goldCost, threatLevel;
    [SerializeField] private TextMeshProUGUI goldCostText, threatLevelText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goldCostText.text = goldCost.ToString();
        threatLevelText.text = threatLevel.ToString();
    }
}
