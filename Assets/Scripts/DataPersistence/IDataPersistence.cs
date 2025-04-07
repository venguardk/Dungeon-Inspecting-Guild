using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    // Class variable for saving and loading datas
    void LoadData(GameData data);

    void SaveData(ref GameData data);

    void LoadOption(OptionData data);

    void SaveOption(ref OptionData data);
}
