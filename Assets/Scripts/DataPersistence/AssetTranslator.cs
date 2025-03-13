using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssetTranslator : MonoBehaviour
{
    public static AssetTranslator instance;
    public GameObject[] allAsset;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public GameObject gameObjectConvertion(string name)
    {
        return allAsset.FirstOrDefault(obj => obj.name == name);
    }
}
