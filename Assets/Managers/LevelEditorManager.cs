using UnityEngine;

public class LevelEditorManager : MonoBehaviour
{
    //This script is based on How to make a Level Editor in Unity -https://youtu.be/eWBDuEWUOwc?si=lxP03a4ICCOSW2Z_
    public AssetController[] assetButtons;
    public GameObject[] assets, assetImages;
    public int currentButtonPressed;

    private void Update()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //Updating where the mouse is
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        if (Input.GetMouseButtonDown(0) && assetButtons[currentButtonPressed].clicked)
        { //If the left mouse button is clicked, spawn the asset
            assetButtons[currentButtonPressed].clicked = false;
            float rotation = GameObject.FindGameObjectWithTag("AssetImage").transform.rotation.eulerAngles.z; //Acquiring rotation from asset
            Instantiate(assets[currentButtonPressed], new Vector3(worldPosition.x, worldPosition.y, 0), Quaternion.Euler(0, 0, rotation)); //Spawn the asset at the mouse position
            Destroy(GameObject.FindGameObjectWithTag("AssetImage"));
        }
        else if (Input.GetMouseButtonDown(1) && assetButtons[currentButtonPressed].clicked)
        { //If the right mouse button is clicked, cancel addition
            assetButtons[currentButtonPressed].clicked = false;
            Destroy(GameObject.FindGameObjectWithTag("AssetImage"));
        }
    }
}
