using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionData
{
    // For saving and loading options
    public bool ColorOption;
    public string LangOption;
    public bool NewGame;
    public bool isOneHandMode;
    public bool VolumeOn;

    public OptionData()
    {
        this.ColorOption = false;
        this.LangOption = "English";
        this.NewGame = true;
        this.isOneHandMode = false;
        this.VolumeOn = true;
    }
}
