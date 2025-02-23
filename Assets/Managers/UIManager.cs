using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private TextMeshProUGUI goldBudgetText, threatLevelText, playerHealthText;
    [SerializeField] private Canvas playModeCanvas, levelEditorCanvas;
    [SerializeField] private GameObject assetsTab, threatsTab;

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
        assetsTab.SetActive(true);
        threatsTab.SetActive(false);
    }

    void FixedUpdate()
    {
        UpdateStatText();
    }

    private void UpdateStatText()
    {
        goldBudgetText.text = "Gold Budget: " + LevelEditorManager.instance.GetGoldRemaining().ToString();
        threatLevelText.text = "Threat Level: " + LevelEditorManager.instance.GetCurrentThreatLevel().ToString() + "/" + LevelEditorManager.instance.GetRequiredThreatLevel().ToString();
        playerHealthText.text = $"Health: {PlayerManager.instance.GetPlayerHealth()}";

        if (LevelEditorManager.instance.GetGoldRemaining() < 0)
        {
            goldBudgetText.color = Color.red;
        }
        else
        {
            goldBudgetText.color = new Color(255f / 255f, 193f / 255f, 85f / 255f);
        }

        if (LevelEditorManager.instance.GetCurrentThreatLevel() < LevelEditorManager.instance.GetRequiredThreatLevel())
        {
            threatLevelText.color = Color.red;
        }
        else
        {
            threatLevelText.color = Color.green;
        }
    }

    public void AccessAssetsTab()
    {
        assetsTab.SetActive(true);
        threatsTab.SetActive(false);
    }

    public void AccessThreatsTab()
    {
        assetsTab.SetActive(false);
        threatsTab.SetActive(true);
    }

    public void SwitchToPlayMode()
    {
        if (LevelEditorManager.instance.LevelValuesMet())
        {
            LevelEditorManager.instance.DeactivateButton();
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
