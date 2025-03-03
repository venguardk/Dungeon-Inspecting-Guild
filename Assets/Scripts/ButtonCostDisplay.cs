using UnityEngine;
using TMPro;

public class ButtonCostDisplay : MonoBehaviour
{
    [SerializeField] private int goldCost, threatLevel;
    [SerializeField] private TextMeshProUGUI goldCostText, threatLevelText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goldCostText.text = goldCost.ToString();
        threatLevelText.text = threatLevel.ToString();
    }
}
