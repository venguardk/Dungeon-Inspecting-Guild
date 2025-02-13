using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isLevelEditorMode;

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
        isLevelEditorMode = false;
    }

    public void SwitchToLevelEditor()
    {
        isLevelEditorMode = true;
    }

    public bool IsLevelEditorMode()
    {
        return isLevelEditorMode;
    }
}
