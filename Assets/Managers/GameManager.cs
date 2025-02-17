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
        //Save Level Data
        LevelEditorManager.instance.LevelSave();
        PlayerManager.instance.FullHeal();
        isLevelEditorMode = false;
        CameraManager.instance.SwitchToPlayModeCamera();
    }

    public void SwitchToLevelEditor()
    {
        //Load Level Data
        Debug.Log("LoadedEditor?");
        LevelEditorManager.instance.LevelLoad();
        isLevelEditorMode = true;
        CameraManager.instance.SwitchToLevelEditorCamera();
    }

    public bool IsLevelEditorMode()
    {
        return isLevelEditorMode;
    }
}
