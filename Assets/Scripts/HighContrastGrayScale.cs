using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HighContrastGrayScale : MonoBehaviour, IDataPersistence
{
    public Volume volume;
    private ColorAdjustments colorAdjustments;

    private bool isGrayscale = false;

    void Awake()
    {
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            colorAdjustments.active = true;
        }
    }

    public void ToggleGrayscale()
    {
        isGrayscale = !isGrayscale;

        if (isGrayscale)
        {
            colorAdjustments.saturation.Override(-100f);
        }
        else
        {
            colorAdjustments.saturation.Override(0f);
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
        data.ColorOption = isGrayscale;
    }

    public void LoadOption(OptionData data)
    {
        this.isGrayscale = data.ColorOption;

        if (isGrayscale)
        {
            colorAdjustments.saturation.Override(-100f);
        }
        else
        {
            colorAdjustments.saturation.Override(0f);
        }
    }
}