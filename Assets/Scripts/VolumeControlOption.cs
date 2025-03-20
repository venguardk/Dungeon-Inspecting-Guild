using UnityEngine;

public class VolumeControlOption : MonoBehaviour, IDataPersistence
{
    public static VolumeControlOption instance;
    public bool VolumeOn = true;

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

    public void ToggleVolume()
    {
        if (VolumeOn == true)
        {
            VolumeOn = false;
        }
        else if (VolumeOn == false)
        {
            VolumeOn = true;
        }
        SetAudioListener();
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
        data.VolumeOn = VolumeOn;
    }

    public void LoadOption(OptionData data)
    {
        this.VolumeOn = data.VolumeOn;
        SetAudioListener();
    }

    private void SetAudioListener() //Setting volume of game
    {
        AudioListener.volume = VolumeOn ? 1 : 0;
    }
}
