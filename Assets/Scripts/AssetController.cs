using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssetController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // This script is used to control the asset buttons in the level editor UI
    // Add this script to each asset button object
    // Other scripts that interact with this script: LevelEditorManager, UIManager

    [SerializeField] private int ID;
    public bool clicked;
    private LevelEditorManager levelEditorManager;
    private Image buttonImage;

    private void Start()
    {
        levelEditorManager = GameObject.FindGameObjectWithTag("LevelEditorManager").GetComponent<LevelEditorManager>();
        buttonImage = GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1) && clicked) //If right mouse button is clicked, deactivate this button
        {
            SetButtonInactive();
        }

        if (levelEditorManager.currentButtonPressed == ID && clicked) //If this is the current active button, indicate it by making the button icon slightly darker
        {
            buttonImage.color = new Color(155f / 255f, 155f / 255f, 155f / 255f);
        }
        else
        {
            SetButtonInactive();
        }
    }

    public void ButtonClicked() //When button is clicked, inform level editor manager which asset should be added and spawn the image
    {
        //AudioManager.instance.PlayButtonPressSound();

        levelEditorManager.DeactivateButton(); //Deactivate any other active buttons
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //Updating where the mouse is
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        clicked = true;
        levelEditorManager.currentButtonPressed = ID;
        Instantiate(levelEditorManager.assetImages[ID], new Vector3(worldPosition.x + 0.06f, worldPosition.y + 0.34f, 0), Quaternion.identity); //Spawn the asset image at the mouse position
    }

    private void SetButtonInactive()
    {
        //Reset button to be clickable and reset its color to full
        GetComponent<Button>().interactable = true;
        buttonImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Display the description of the asset when the mouse hovers over the button
        UIManager.instance.DisplayDescription(ID);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Hide the description of the asset when the mouse is no longer hovering over the button
        UIManager.instance.HideDescription();
    }
}
