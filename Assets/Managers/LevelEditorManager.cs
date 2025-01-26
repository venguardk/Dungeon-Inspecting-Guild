using UnityEngine;

public class LevelEditorManager : MonoBehaviour
{
    public AssetController[] assetButtons;
    public GameObject[] assets, assetImages;
    public int currentButtonPressed;

    private void Update()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //Updating where the mouse is
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        if (Input.GetMouseButtonDown(0) && assetButtons[currentButtonPressed].clicked)
        { //If the left mouse button is clicked
            assetButtons[currentButtonPressed].clicked = false;
            Instantiate(assets[currentButtonPressed], new Vector3(worldPosition.x, worldPosition.y, 0), Quaternion.identity); //Spawn the asset at the mouse position
            Destroy(GameObject.FindGameObjectWithTag("AssetImage"));
        }
    }
}
