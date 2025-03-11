using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private TextMeshProUGUI goldBudgetText, threatLevelText, keyCountText;
    [SerializeField] private TextMeshProUGUI dartsCountText, spikesCountText, flameCountText, enemyCountText;
    [SerializeField] private Canvas playModeCanvas, levelEditorCanvas, lossCanvas;
    [SerializeField] private GameObject assetsTab, threatsTab, assetDescriptionSection;
    [SerializeField] private TextMeshProUGUI assetDescription, assetName;
    [SerializeField] private Image[] heartIcons; //5 Heart Icons
    [SerializeField] private Sprite fullHeart, halfHeart, emptyHeart;
    private string[] assetDescriptions = new string[]
    {
        "Can be placed to block paths and projectiles.",
        "Shoots darts every few seconds.",
        "Pops spikes up from the ground that instantly kills the player.",
        "Rapidly shoots fireballs in a specific direction.",
        "Chases the player when close and will attack if near.",
        "A key that the can be picked up to unlock one door.",
        "Restores the player back to full health when picked up.",
        "A box that the player can push.",
        "A shield that grants the player immunity for a few seconds.",
        "A bridge that the player can use to cross gaps."
    };
    private string[] assetNames = new string[]
    {
        "Wall",
        "Dart",
        "Spike",
        "Flamethrower",
        "Enemy",
        "Key",
        "Potion",
        "Box",
        "Shield",
        "Bridge"
    };

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
        levelEditorCanvas.enabled = true;
        playModeCanvas.enabled = false;
        lossCanvas.enabled = false;
        assetsTab.SetActive(true);
        threatsTab.SetActive(false);
        assetDescriptionSection.SetActive(false);
        UpdateLevelStatText();
    }

    void FixedUpdate()
    {
        if (GameManager.instance.IsLevelEditorMode())
        {
            UpdateLevelStatText();
            UpdateLevelStatColours();
        }
    }

    private void UpdateLevelStatText()
    {
        goldBudgetText.text = "Gold Budget: " + LevelEditorManager.instance.GetGoldRemaining().ToString();
        threatLevelText.text = "Threat Level: " + LevelEditorManager.instance.GetCurrentThreatLevel().ToString() + "/" + LevelEditorManager.instance.GetRequiredThreatLevel().ToString();
        dartsCountText.text = $"Darts: {LevelEditorManager.instance.GetDartsCount()}/{LevelEditorManager.instance.GetDartsRequirement()}";
        spikesCountText.text = $"Spikes: {LevelEditorManager.instance.GetSpikesCount()}/{LevelEditorManager.instance.GetSpikesRequirement()}";
        flameCountText.text = $"Flames: {LevelEditorManager.instance.GetFlamethrowersCount()}/{LevelEditorManager.instance.GetFlamethrowersRequirement()}";
        enemyCountText.text = $"Enemies: {LevelEditorManager.instance.GetEnemiesCount()}/{LevelEditorManager.instance.GetEnemiesRequirement()}";
    }

    private void UpdateLevelStatColours()
    {
        if (LevelEditorManager.instance.GetGoldRemaining() < 0)
        {
            goldBudgetText.color = Color.red;
        }
        else
        {
            goldBudgetText.color = new Color(135f / 255f, 103f / 255f, 46f / 255f);
        }

        if (LevelEditorManager.instance.GetCurrentThreatLevel() < LevelEditorManager.instance.GetRequiredThreatLevel())
        {
            threatLevelText.color = Color.red;
        }
        else
        {
            threatLevelText.color = Color.green;
        }

        if (LevelEditorManager.instance.GetDartsCount() < LevelEditorManager.instance.GetDartsRequirement())
        {
            dartsCountText.color = Color.red;
        }
        else
        {
            dartsCountText.color = Color.green;
        }

        if (LevelEditorManager.instance.GetSpikesCount() < LevelEditorManager.instance.GetSpikesRequirement())
        {
            spikesCountText.color = Color.red;
        }
        else
        {
            spikesCountText.color = Color.green;
        }

        if (LevelEditorManager.instance.GetFlamethrowersCount() < LevelEditorManager.instance.GetFlamethrowersRequirement())
        {
            flameCountText.color = Color.red;
        }
        else
        {
            flameCountText.color = Color.green;
        }

        if (LevelEditorManager.instance.GetEnemiesCount() < LevelEditorManager.instance.GetEnemiesRequirement())
        {
            enemyCountText.color = Color.red;
        }
        else
        {
            enemyCountText.color = Color.green;
        }
    }

    public void UpdatePlayerStats()
    {
        UpdatePlayerHealthUI();
        keyCountText.text = "x" + PlayerManager.instance.GetKeyCount().ToString();
    }

    private void UpdatePlayerHealthUI()
    {
        switch (PlayerManager.instance.GetPlayerHealth())
        {
            case 10:
                heartIcons[0].sprite = fullHeart;
                heartIcons[1].sprite = fullHeart;
                heartIcons[2].sprite = fullHeart;
                heartIcons[3].sprite = fullHeart;
                heartIcons[4].sprite = fullHeart;
                break;
            case 9:
                heartIcons[0].sprite = fullHeart;
                heartIcons[1].sprite = fullHeart;
                heartIcons[2].sprite = fullHeart;
                heartIcons[3].sprite = fullHeart;
                heartIcons[4].sprite = halfHeart;
                break;
            case 8:
                heartIcons[0].sprite = fullHeart;
                heartIcons[1].sprite = fullHeart;
                heartIcons[2].sprite = fullHeart;
                heartIcons[3].sprite = fullHeart;
                heartIcons[4].sprite = emptyHeart;
                break;
            case 7:
                heartIcons[0].sprite = fullHeart;
                heartIcons[1].sprite = fullHeart;
                heartIcons[2].sprite = fullHeart;
                heartIcons[3].sprite = halfHeart;
                heartIcons[4].sprite = emptyHeart;
                break;
            case 6:
                heartIcons[0].sprite = fullHeart;
                heartIcons[1].sprite = fullHeart;
                heartIcons[2].sprite = fullHeart;
                heartIcons[3].sprite = emptyHeart;
                heartIcons[4].sprite = emptyHeart;
                break;
            case 5:
                heartIcons[0].sprite = fullHeart;
                heartIcons[1].sprite = fullHeart;
                heartIcons[2].sprite = halfHeart;
                heartIcons[3].sprite = emptyHeart;
                heartIcons[4].sprite = emptyHeart;
                break;
            case 4:
                heartIcons[0].sprite = fullHeart;
                heartIcons[1].sprite = fullHeart;
                heartIcons[2].sprite = emptyHeart;
                heartIcons[3].sprite = emptyHeart;
                heartIcons[4].sprite = emptyHeart;
                break;
            case 3:
                heartIcons[0].sprite = fullHeart;
                heartIcons[1].sprite = halfHeart;
                heartIcons[2].sprite = emptyHeart;
                heartIcons[3].sprite = emptyHeart;
                heartIcons[4].sprite = emptyHeart;
                break;
            case 2:
                heartIcons[0].sprite = fullHeart;
                heartIcons[1].sprite = emptyHeart;
                heartIcons[2].sprite = emptyHeart;
                heartIcons[3].sprite = emptyHeart;
                heartIcons[4].sprite = emptyHeart;
                break;
            case 1:
                heartIcons[0].sprite = halfHeart;
                heartIcons[1].sprite = emptyHeart;
                heartIcons[2].sprite = emptyHeart;
                heartIcons[3].sprite = emptyHeart;
                heartIcons[4].sprite = emptyHeart;
                break;
            default:
                break;
        }
    }

    public void DisplayDescription(int ID)
    {
        assetDescriptionSection.SetActive(true);
        assetDescription.text = assetDescriptions[ID];
        assetName.text = assetNames[ID];
    }

    public void HideDescription()
    {
        assetDescriptionSection.SetActive(false);
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

    public void ShowLossScreen()
    {
        levelEditorCanvas.enabled = false;
        lossCanvas.enabled = true;
    }

    public void ReattemptLevel()
    {
        lossCanvas.enabled = false;
        levelEditorCanvas.enabled = true;
    }
}
