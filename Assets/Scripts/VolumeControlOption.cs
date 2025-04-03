using UnityEngine;

public class VolumeControlOption : MonoBehaviour, IDataPersistence
{
    // This script is attached to the Volume Control button featured in the options menu and determines if the player wants volume on or not

    public static VolumeControlOption instance; // Singleton instance of this script to be used in other scenes and scripts
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
        // This function is called when the player presses the button to toggle between volume on and volume off
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
        // This function is called when the game loads to set the player's volume options; This enables the player to save their volume options between sessions
        this.VolumeOn = data.VolumeOn;
        SetAudioListener();
    }

    private void SetAudioListener() //Setting volume of game
    {
        // This function is called to set the volume of the game based on the player's preference; It sets the scenes' Audio Listener based on the player preference
        AudioListener.volume = VolumeOn ? 1 : 0;
    }
}
