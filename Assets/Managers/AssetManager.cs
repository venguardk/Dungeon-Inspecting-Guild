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
            levelEditorManager.AddThreatAssetCount(ID);

            AudioManager.instance.PlayAddAssetSound(ID);
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

            levelEditorManager.MinusGold(goldCost);
            levelEditorManager.MinusThreatLevel(threatLevel);
            levelEditorManager.RemoveAsset(new Vector2(Mathf.Ceil(this.transform.position.x - 0.75f) + 0.5f, Mathf.Ceil(this.transform.position.y - 0.75f) + 0.5f), objType);
            levelEditorManager.MinusThreatAssetCount(ID);
            Destroy(this.gameObject);
        }
    }
}
