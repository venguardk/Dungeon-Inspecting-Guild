using System.Collections.Generic;
using UnityEngine;

public class LevelEditorManager : MonoBehaviour
{
    //This script is based on How to make a Level Editor in Unity -https://youtu.be/eWBDuEWUOwc?si=lxP03a4ICCOSW2Z_
    public AssetController[] assetButtons;
    public GameObject[] assets, assetImages;
    public int currentButtonPressed;
    [SerializeField] private int goldBudget, requiredThreatLevel;
    private int goldSpent, currentThreatLevel;
    private Dictionary<Vector2, GameObject> RoomDictionary = new Dictionary<Vector2, GameObject>();

    private void Update()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //Updating where the mouse is
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);


        if (Input.GetMouseButtonDown(0) && assetButtons[currentButtonPressed].clicked)
        { //If the left mouse button is clicked, spawn the asset

            if (RoomDictionary.ContainsKey(new Vector2(Mathf.Ceil(worldPosition.x - 0.5f), Mathf.Ceil(worldPosition.y - 0.5f))) == false)
            {
                //assetButtons[currentButtonPressed].clicked = false;
                float rotation = GameObject.FindGameObjectWithTag("AssetImage").transform.rotation.eulerAngles.z; //Acquiring rotation from asset
                //Setting the asset so that it will be located in a grid position
                Instantiate(assets[currentButtonPressed], new Vector3(Mathf.Ceil(worldPosition.x - 0.5f), Mathf.Ceil(worldPosition.y - 0.5f), 0), Quaternion.Euler(0, 0, rotation)); //Spawn the asset at the mouse position
                RoomDictionary.Add(new Vector2(Mathf.Ceil(worldPosition.x - 0.5f), Mathf.Ceil(worldPosition.y - 0.5f)), assets[currentButtonPressed]);

                //Destroy(GameObject.FindGameObjectWithTag("AssetImage"));
            }
        }
        else if (Input.GetMouseButtonDown(1) && assetButtons[currentButtonPressed].clicked)
        { //If the right mouse button is clicked, cancel addition
            assetButtons[currentButtonPressed].clicked = false;
            Destroy(GameObject.FindGameObjectWithTag("AssetImage"));
        }
    }

    private void FixedUpdate()
    {
        if (goldSpent > goldBudget)
        {
            Debug.Log("You have exceeded your gold budget!");
        }
        if (currentThreatLevel > requiredThreatLevel)
        {
            Debug.Log("You have met the required threat level!");
        }
    }

    public void AddGold(int gold)
    {
        goldSpent += gold;
        //Debug.Log("Gold Spent: " + goldSpent);
    }

    public void MinusGold(int gold)
    {
        goldSpent -= gold;
    }

    public void AddThreatLevel(int threatLevel)
    {
        currentThreatLevel += threatLevel;
    }

    public void MinusThreatLevel(int threatLevel)
    {
        currentThreatLevel -= threatLevel;
    }

    public void RemoveAsset(Vector2 position)
    {
        RoomDictionary.Remove(position);
    }
}
