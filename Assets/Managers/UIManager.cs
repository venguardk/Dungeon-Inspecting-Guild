using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    //This script handles the UI for the main levels both during level editor and play mode
    //Other scripts this script interacts with: GameManager, LevelEditorManager, PlayerManager, GameLanguageManager
    public static UIManager instance;
    [SerializeField] private TextMeshProUGUI goldBudgetText, threatLevelText, keyCountText;
    [SerializeField] private TextMeshProUGUI dartsCountText, spikesCountText, flameCountText, enemyCountText;
    [SerializeField] private Canvas playModeCanvas, levelEditorCanvas, lossCanvas;
    [SerializeField] private GameObject assetsTab, threatsTab, assetDescriptionSection;
    [SerializeField] private TextMeshProUGUI assetDescription, assetName;
    [SerializeField] private Image[] heartIcons; //5 Heart Icons
    [SerializeField] private Sprite fullHeart, halfHeart, emptyHeart;
    private string[] assetDescriptions = new string[] //Array of asset descriptions
    {
        "Can be placed to block paths and projectiles.",
        "Shoots darts every few seconds.",
        "Pops spikes up from the ground every few seconds.",
        "Rapidly shoots fireballs in a specific direction.",
        "Chases the inspector when close and will attack if near.",
        "A key that the can be picked up to unlock one door.",
        "Restores the inspector back to full health when picked up.",
        "A box that the inspector can push.",
        "A shield that grants the inspector immunity for a few seconds.",
        "A bridge that the inspector can use to cross gaps.",
        "Emits electrical zaps every few seconds."
    };
    private string[] assetNames = new string[] //Array of asset descriptions
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
        "Bridge",
        "Electrical Plate"
    };
    public TMP_FontAsset fontEng;
    public TMP_FontAsset fontJap;
    private string[] assetDescriptionsJap = new string[]
    {
        "道を塞いだり飛んできたモノを防いだりするカベ",
        "数秒に一回、矢を放つワナ",
        "数秒に一回、トゲが飛び出してくるワナ",
        "ひとつの方向へ連続して火の玉を放つワナ",
        "アナタを追いかけて攻撃して来るテキ",
        "カギの掛かったトビラを開けれるカギ",
        "アナタの体力を全回復させれるクスリ",
        "アナタが押して動かせるハコ",
        "アナタを数秒間無敵にできるバリア",
        "アナタがスキマを通るのに使えるハシ"
    };
    private string[] assetNamesJap = new string[]
    {
        "カベ",
        "ワナ（矢）",
        "ワナ（トゲ）",
        "ワナ（炎）",
        "テキ",
        "カギ",
        "クスリ",
        "ハコ",
        "バリア",
        "ハシ"
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
        if (GameLanguageManager.gameLanguage == "日本語")
        {
            goldBudgetText.font = fontJap;
            goldBudgetText.text = "予算: " + LevelEditorManager.instance.GetGoldRemaining().ToString();
            threatLevelText.font = fontJap;
            threatLevelText.text = "危険度: " + LevelEditorManager.instance.GetCurrentThreatLevel().ToString() + "/" + LevelEditorManager.instance.GetRequiredThreatLevel().ToString();
            dartsCountText.font = fontJap;
            dartsCountText.text = $"ワナ（矢）: {LevelEditorManager.instance.GetDartsCount()}/{LevelEditorManager.instance.GetDartsRequirement()}";
            spikesCountText.font = fontJap;
            spikesCountText.text = $"ワナ（トゲ）: {LevelEditorManager.instance.GetSpikesCount()}/{LevelEditorManager.instance.GetSpikesRequirement()}";
            flameCountText.font = fontJap;
            flameCountText.text = $"ワナ（炎）: {LevelEditorManager.instance.GetFlamethrowersCount()}/{LevelEditorManager.instance.GetFlamethrowersRequirement()}";
            enemyCountText.font = fontJap;
            enemyCountText.text = $"テキ: {LevelEditorManager.instance.GetEnemiesCount()}/{LevelEditorManager.instance.GetEnemiesRequirement()}";
        }
        else
        {
            goldBudgetText.font = fontEng;
            goldBudgetText.text = "Gold Budget: " + LevelEditorManager.instance.GetGoldRemaining().ToString();
            threatLevelText.font = fontEng;
            threatLevelText.text = "Threat Level: " + LevelEditorManager.instance.GetCurrentThreatLevel().ToString() + "/" + LevelEditorManager.instance.GetRequiredThreatLevel().ToString();
            dartsCountText.font = fontEng;
            dartsCountText.text = $"Darts: {LevelEditorManager.instance.GetDartsCount()}/{LevelEditorManager.instance.GetDartsRequirement()}";
            spikesCountText.font = fontEng;
            spikesCountText.text = $"Spikes: {LevelEditorManager.instance.GetSpikesCount()}/{LevelEditorManager.instance.GetSpikesRequirement()}";
            flameCountText.font = fontEng;
            flameCountText.text = $"Flames: {LevelEditorManager.instance.GetFlamethrowersCount()}/{LevelEditorManager.instance.GetFlamethrowersRequirement()}";
            enemyCountText.font = fontEng;
            enemyCountText.text = $"Enemies: {LevelEditorManager.instance.GetEnemiesCount()}/{LevelEditorManager.instance.GetEnemiesRequirement()}";
        }
    }

    private void UpdateLevelStatColours()
    {
        //Updates the color of the level stats, specifically the gold budget, threat level, and asset counts
        //This is used to indicate if the player has enough gold, threat level, and asset counts
        //Green indicates requirement is met, red indicates requirement is not met
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
        //Display the appropriate amount of hearts based on the player's health
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
        if (GameLanguageManager.gameLanguage == "日本語")
        {
            assetDescription.font = fontJap;
            assetName.font = fontJap;
            assetDescription.text = assetDescriptionsJap[ID];
            assetName.text = assetNamesJap[ID];
        }
        else
        {
            assetDescription.font = fontEng;
            assetName.font = fontEng;
            assetDescription.text = assetDescriptions[ID];
            assetName.text = assetNames[ID];
        }

    }

    public void HideDescription()
    {
        assetDescriptionSection.SetActive(false);
    }

    public void AccessAssetsTab() //Access the assets tab
    {
        assetsTab.SetActive(true);
        threatsTab.SetActive(false);
    }

    public void AccessThreatsTab() //Access the threats tab
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

    public void ReattemptLevel() //Close lossCanvas and open levelEditorCanvas
    {
        lossCanvas.enabled = false;
        levelEditorCanvas.enabled = true;
    }
}
