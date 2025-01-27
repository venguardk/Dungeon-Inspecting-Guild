using UnityEngine;

public class AssetManager : MonoBehaviour
{
    //Add this script to all assets that the player places down in the level editor
    [SerializeField] public int ID, goldCost, threatLevel;
    private LevelEditorManager levelEditorManager;

    void Start()
    {
        levelEditorManager = GameObject.FindGameObjectWithTag("LevelEditorManager").GetComponent<LevelEditorManager>();
        levelEditorManager.AddGold(goldCost);
        levelEditorManager.AddThreatLevel(threatLevel);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) //If right clicked, destroy the asset
        {
            Destroy(this.gameObject);
            levelEditorManager.MinusGold(goldCost);
            levelEditorManager.MinusThreatLevel(threatLevel);
        }
    }
}
