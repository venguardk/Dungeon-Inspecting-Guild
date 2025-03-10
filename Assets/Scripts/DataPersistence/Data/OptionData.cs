using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionData
{
    public bool ColorOption;
    public string LangOption;

    public OptionData()
    {
       this.ColorOption = false;
       this.LangOption = "English";
    }
}
