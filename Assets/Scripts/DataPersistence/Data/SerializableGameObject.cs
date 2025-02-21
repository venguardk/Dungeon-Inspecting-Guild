using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[System.Serializable]
public class SerializableGameObject
{
    public string name;
    public string prefab;
    public bool initial;

    public SerializableGameObject(GameObject gameObject)
    {
        if (PrefabUtility.GetPrefabAssetType(gameObject) != PrefabAssetType.NotAPrefab)
        {
            this.initial = false;
            string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
            Debug.Log(path);
            this.prefab = System.IO.Path.GetFileNameWithoutExtension(path);
        }
        else
        {
            this.initial = true;
            this.prefab = "";
        }

        this.name = gameObject.name;
    }

    public GameObject ToGameObject()
    {
        return new GameObject(name);
    }
}
