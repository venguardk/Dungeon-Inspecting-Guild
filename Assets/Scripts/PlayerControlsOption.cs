using TMPro;
using UnityEngine;

public class PlayerControlsOption : MonoBehaviour, IDataPersistence
{
    // This script is used to manage the player's control options from the button in the Options menu
    public static PlayerControlsOption instance;
    public bool isOneHandMode = false;
    public TMP_FontAsset fontEng;
    public TMP_FontAsset fontJap;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleControls()
    {
        // This function is called when the player presses the button to toggle between limited dexterity and full dexterity mode
        isOneHandMode = !isOneHandMode;
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
        data.isOneHandMode = isOneHandMode;
    }

    public void LoadOption(OptionData data)
    {
        // This function is called when the game loads to set the player's control options; This enables the player to save their control options between sessions
        this.isOneHandMode = data.isOneHandMode;
    }
}
