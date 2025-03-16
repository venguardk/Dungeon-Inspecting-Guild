using TMPro;
using UnityEngine;

public class PlayerControlsOption : MonoBehaviour, IDataPersistence
{
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
        isOneHandMode = !isOneHandMode;
        Debug.Log("One Hand Mode: " + isOneHandMode);
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
        this.isOneHandMode = data.isOneHandMode;
    }
}
