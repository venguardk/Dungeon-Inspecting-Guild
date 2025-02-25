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

    public SerializableGameObject(GameObject gameObject)
    {
        this.name = gameObject.name;
        this.prefab = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
    }

    public GameObject ToGameObject()
    {
        Debug.Log("ToGameObject:" + LevelEditorManager.instance.gameObjectConvertion(this.name));
        return LevelEditorManager.instance.gameObjectConvertion(this.name);
    }
}
