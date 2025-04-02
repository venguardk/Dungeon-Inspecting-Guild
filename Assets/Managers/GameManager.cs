using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    //This script handles the level and determines if the game is in level editor mode or play mode
    //Other scripts this script interacts with: LevelEditorManager, PlayerManager, CameraManager

    public static GameManager instance;
    [SerializeField] public bool isLevelEditorMode; //This variable determines if the game is in level editor mode or not; Should be true on start

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
        LevelEditorManager.instance.LevelSave(); //Saving the level layout
        PlayerManager.instance.FullHeal(); //Healing the player before starting the level
        isLevelEditorMode = false;
        GameObject itemImage = GameObject.FindWithTag("AssetImage"); //Finding the asset image over the mouse in case it is there and removes it
        if (itemImage != null) //Guaranteeing removal of asset image
        {
            Destroy(GameObject.FindGameObjectWithTag("AssetImage"));
        }
        CameraManager.instance.SwitchToPlayModeCamera(); //Switching to play mode camera
    }

    public void SwitchToLevelEditor()
    {
        LevelEditorManager.instance.LevelLoad(); //Loading the level layout
        isLevelEditorMode = true;
        CameraManager.instance.SwitchToLevelEditorCamera(); //Switching to level editor camera
    }

    public bool IsLevelEditorMode() //Check if the game is in level editor mode; Used by various scripts
    {
        return isLevelEditorMode;
    }
}
