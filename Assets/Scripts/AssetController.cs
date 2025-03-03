using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssetController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
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
        if (Input.GetMouseButtonDown(1) && clicked)
        {
            SetButtonInactive();
        }

        if (levelEditorManager.currentButtonPressed == ID && clicked)
        {
            buttonImage.color = new Color(155f / 255f, 155f / 255f, 155f / 255f);
        }
        else
        {
            SetButtonInactive();
        }
    }

    public void ButtonClicked()
    {
        levelEditorManager.DeactivateButton();
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //Updating where the mouse is
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        clicked = true;
        levelEditorManager.currentButtonPressed = ID;
        Instantiate(levelEditorManager.assetImages[ID], new Vector3(worldPosition.x + 0.06f, worldPosition.y + 0.34f, 0), Quaternion.identity); //Spawn the asset at the mouse position
    }

    private void SetButtonInactive()
    {
        GetComponent<Button>().interactable = true;
        buttonImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.instance.DisplayDescription(ID);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.HideDescription();
    }
}
