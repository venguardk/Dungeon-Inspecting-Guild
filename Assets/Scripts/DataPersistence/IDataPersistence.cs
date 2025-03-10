using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void LoadData(GameData data);

    void SaveData(ref GameData data);

    void LoadOption(OptionData data);

    void SaveOption(ref OptionData data);
}
