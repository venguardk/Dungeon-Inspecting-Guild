using UnityEngine;

public class GameLanguageManager : MonoBehaviour, IDataPersistence
{
    // This script is for determining which language is being used for the game
    public static string gameLanguage = "English";

    public void ToggleLanguage(string optionLanguage)
    {
        gameLanguage = optionLanguage;
        DataPersistenceManager.instance.SaveOption();
    }

    // Sets the game language to the given type
    public void SwitchLanguage(int optionLanguage)
    {
        switch (optionLanguage)
        {
            case 0:
                gameLanguage = "English";
                break;
            case 1:
                gameLanguage = "日本語";
                break;
        }
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
