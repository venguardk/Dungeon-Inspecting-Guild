using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HighContrastGrayScale : MonoBehaviour, IDataPersistence
{
    public Volume volume;
    private ColorAdjustments colorAdjustments;

    private bool isGrayscale = false; // Track whether grayscale is active

    void Awake()
    {
        // Ensure the volume is set up and get the Color Adjustments override
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            // Initial setup: Ensure Color Adjustments is enabled
            colorAdjustments.active = true;
        }
    }

    /*void Update()
    {
        // Toggle grayscale with a key press (for example, the "G" key)
        if (Input.GetKeyDown(KeyCode.G))
        {
            ToggleGrayscale();
        }
    }*/

    public void ToggleGrayscale()
    {
        isGrayscale = !isGrayscale;

        // Modify the saturation to simulate grayscale
        if (isGrayscale)
        {
            // Set saturation to 0 for grayscale
            colorAdjustments.saturation.Override(-100f);
        }
        else
        {
            // Set saturation back to normal (0 is default for normal color)
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
            // Set saturation to 0 for grayscale
            colorAdjustments.saturation.Override(-100f);
        }
        else
        {
            // Set saturation back to normal (0 is default for normal color)
            colorAdjustments.saturation.Override(0f);
        }
    }
}