using UnityEngine;

public class AssetManager : MonoBehaviour
{
    //This script handles the placement and removal of assets in the level editor
    //Add this script to all asset prefabs that the player will place down in the level editor
    //Other scripts this script interacts with: LevelEditorManager, AudioManager, Bridge

    [SerializeField] public int ID, goldCost, threatLevel, objType;
    private LevelEditorManager levelEditorManager;
    [SerializeField] private bool prebuiltItem = false; //For checking to see if the asset in the scene is a prebuilt item or not; Can be set in the Unity inspector

    void Start()
    {
        if (prebuiltItem == false) //If asset is not a prebuilt item...
        {
            levelEditorManager = GameObject.FindGameObjectWithTag("LevelEditorManager").GetComponent<LevelEditorManager>(); //Accessing LevelEditorManager
            levelEditorManager.AddGold(goldCost);
            levelEditorManager.AddThreatLevel(threatLevel);
            levelEditorManager.AddThreatAssetCount(ID);

            AudioManager.instance.PlayAddAssetSound(ID); //Play the sound for placing an asset
        }
    }

    private void OnMouseOver() //Prebuilt Unity Function that checks if mouse is hovering over object
    {
        if (Input.GetMouseButtonDown(1) && GameManager.instance.IsLevelEditorMode() && prebuiltItem == false) //If right clicked, destroy the asset
        {
            Bridge bridgeScript = this.GetComponent<Bridge>(); //If object is a bridge, re-enable the gap prefab underneath it
            if (bridgeScript != null)
            {
                bridgeScript.EnableGap();
            }

            //Removing values from the LevelEditorManager
            levelEditorManager.MinusGold(goldCost);
            levelEditorManager.MinusThreatLevel(threatLevel);
            levelEditorManager.RemoveAsset(new Vector2(Mathf.Ceil(this.transform.position.x - 0.75f) + 0.5f, Mathf.Ceil(this.transform.position.y - 0.75f) + 0.5f), objType);
            levelEditorManager.MinusThreatAssetCount(ID);
            Destroy(this.gameObject);
        }
    }
}
