using UnityEngine;

public class AssetManager : MonoBehaviour
{
    //Add this script to all assets that the player places down in the level editor
    [SerializeField] public int ID;
    private LevelEditorManager levelEditorManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelEditorManager = GameObject.FindGameObjectWithTag("LevelEditorManager").GetComponent<LevelEditorManager>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) //If right clicked, destroy the asset
        {
            Destroy(this.gameObject);
        }
    }
}
