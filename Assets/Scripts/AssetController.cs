using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssetController : MonoBehaviour
{
    [SerializeField] private int ID;
    public bool clicked;
    private LevelEditorManager levelEditorManager;

    private void Start()
    {
        levelEditorManager = GameObject.FindGameObjectWithTag("LevelEditorManager").GetComponent<LevelEditorManager>();
    }

    public void ButtonClicked()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //Updating where the mouse is
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        Debug.Log("Button " + ID + " clicked");
        clicked = true;

        Instantiate(levelEditorManager.assetImages[ID], new Vector3(worldPosition.x, worldPosition.y, 0), Quaternion.identity); //Spawn the asset at the mouse position

        levelEditorManager.currentButtonPressed = ID;
    }
}
