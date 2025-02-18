using UnityEngine;

public class AssetManager : MonoBehaviour
{
    //Add this script to all assets that the player places down in the level editor
    [SerializeField] public int ID, goldCost, threatLevel, objType;
    private LevelEditorManager levelEditorManager;
    [SerializeField] private bool prebuiltItem = false;

    void Start()
    {
        if (prebuiltItem == false)
        {
            levelEditorManager = GameObject.FindGameObjectWithTag("LevelEditorManager").GetComponent<LevelEditorManager>();
            levelEditorManager.AddGold(goldCost);
            levelEditorManager.AddThreatLevel(threatLevel);
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && GameManager.instance.IsLevelEditorMode() && prebuiltItem == false) //If right clicked, destroy the asset
        {
            Bridge bridgeScript = this.GetComponent<Bridge>(); //If object is a bridge, enable the gap
            if (bridgeScript != null)
            {
                bridgeScript.EnableGap();
            }

            Destroy(this.gameObject);
            levelEditorManager.MinusGold(goldCost);
            levelEditorManager.MinusThreatLevel(threatLevel);
            levelEditorManager.RemoveAsset(new Vector2(this.transform.position.x, this.transform.position.y), objType);
        }
    }
}
