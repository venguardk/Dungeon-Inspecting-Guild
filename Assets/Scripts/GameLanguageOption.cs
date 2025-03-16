using UnityEngine.UI;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering.Universal;
using TMPro;

[ExecuteAlways]
public class GameLanguageOption : MonoBehaviour
{
    public static string gameLanguage = "English";
    public Translation[] translations = new Translation[] { new Translation(gameLanguage) };
    TextMeshProUGUI textObject;
    private void Awake()
    {
        textObject = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        FillEmptyTranslations();
        Translate();
    }

    private void Update()
    {
        Translate();
    }

    public void Translate()
    {
        int languageIndex = FindLanguageIndex();
        textObject.text = translations[languageIndex].text;
        textObject.font = translations[languageIndex].font;
    }

    public int FindLanguageIndex()
    {
        return FindLanguageIndex(translations);
    }
    public static int FindLanguageIndex(Translation[] translations)
    {
        for (int i = 0; i < translations.Length; i++)
        {
            if (translations[i].language == GameLanguageManager.gameLanguage)
            {
                return i;
            }
        }
        return 0;
    }

    public void FillEmptyTranslations()
    {
        for (int i = 0; i < translations.Length; i++)
        {
            if (translations[i].text == "")
            {
                translations[i].text = textObject.text;
                translations[i].font = textObject.font;
            }
        }
    }

    [System.Serializable]
    public struct Translation
    {
        public string name;
        public string language;
        public TMP_FontAsset font;
        [TextArea] public string text;

        public Translation(string gameLanguage)
        {
            name = gameLanguage;
            this.language = gameLanguage;
            text = "";
            font = null;
        }
    }
}
