using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] public bool isLevelEditorMode;

    private void Awake()
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

    void Start()
    {
        isLevelEditorMode = true;
    }

    public void SwitchToPlayMode()
    {
        LevelEditorManager.instance.LevelSave();
        PlayerManager.instance.FullHeal();
        isLevelEditorMode = false;
        GameObject itemImage = GameObject.FindWithTag("AssetImage");
        if (itemImage != null) //Guaranteeing removal of asset image
        {
            Destroy(GameObject.FindGameObjectWithTag("AssetImage"));
        }
        Destroy(GameObject.FindGameObjectWithTag("AssetImage"));
        CameraManager.instance.SwitchToPlayModeCamera();
    }

    public void SwitchToLevelEditor()
    {
        LevelEditorManager.instance.LevelLoad();
        isLevelEditorMode = true;
        CameraManager.instance.SwitchToLevelEditorCamera();
    }

    public bool IsLevelEditorMode()
    {
        return isLevelEditorMode;
    }
}
