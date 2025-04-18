using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[System.Serializable]
public class SerializableGameObject
{
    // This script is for converting GameObject into a serializable format for JSON file, and converting it back to GameObject
    public string name;

    public SerializableGameObject(GameObject gameObject)
    {
        this.name = gameObject.name;
    }

    public GameObject ToGameObject()
    {
        return AssetTranslator.instance.gameObjectConvertion(this.name);
    }
}
