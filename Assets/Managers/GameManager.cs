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
        PlayerManager.instance.FullHeal();
        isLevelEditorMode = false;
    }

    public void SwitchToLevelEditor()
    {
        //Load Level Data
        isLevelEditorMode = true;
    }

    public bool IsLevelEditorMode()
    {
        return isLevelEditorMode;
    }
}
