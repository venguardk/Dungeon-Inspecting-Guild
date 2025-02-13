using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private TextMeshProUGUI goldBudgetText, threatLevelText, playerHealthText;
    [SerializeField] private Canvas playModeCanvas, levelEditorCanvas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateStatText();
        levelEditorCanvas.enabled = true;
        playModeCanvas.enabled = false;
    }

    void FixedUpdate()
    {
        UpdateStatText();
    }

    private void UpdateStatText()
    {
        goldBudgetText.text = "Gold Budget: " + LevelEditorManager.instance.GetGoldSpent().ToString() + "/" + LevelEditorManager.instance.GetGoldBudget().ToString();
        threatLevelText.text = "Threat Level: " + LevelEditorManager.instance.GetCurrentThreatLevel().ToString() + "/" + LevelEditorManager.instance.GetRequiredThreatLevel().ToString();
        playerHealthText.text = $"Health: {PlayerManager.instance.GetPlayerHealth()}";
    }

    public void SwitchToPlayMode()
    {
        if (LevelEditorManager.instance.LevelValuesMet())
        {
            GameManager.instance.SwitchToPlayMode();
            levelEditorCanvas.enabled = false;
            playModeCanvas.enabled = true;
        }
    }

    public void SwitchToLevelEditor()
    {
        GameManager.instance.SwitchToLevelEditor();
        levelEditorCanvas.enabled = true;
        playModeCanvas.enabled = false;
    }
}
