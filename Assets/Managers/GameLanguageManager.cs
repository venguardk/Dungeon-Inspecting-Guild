using UnityEngine;

public class GameLanguageManager : MonoBehaviour, IDataPersistence
{
    public static string gameLanguage = "English";

    public void ToggleLanguage(string optionLanguage)
    {
        gameLanguage = optionLanguage;
        DataPersistenceManager.instance.SaveOption();
    }

    public void SaveData(ref GameData data)
    {
        return;
    }

    public void LoadData(GameData data)
    {
        return;
    }

    public void SaveOption(ref OptionData data)
    {
        data.LangOption = gameLanguage;
    }

    public void LoadOption(OptionData data)
    {
        gameLanguage = data.LangOption;
    }
}
