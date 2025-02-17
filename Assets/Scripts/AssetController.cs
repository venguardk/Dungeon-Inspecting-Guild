using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssetController : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private int ID;
    public bool clicked;
    private LevelEditorManager levelEditorManager;

    private void Start()
    {
        levelEditorManager = GameObject.FindGameObjectWithTag("LevelEditorManager").GetComponent<LevelEditorManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        levelEditorManager.DeactivateButton();
    }

    public void ButtonClicked()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //Updating where the mouse is
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        clicked = true;
        levelEditorManager.currentButtonPressed = ID;
        Instantiate(levelEditorManager.assetImages[ID], new Vector3(worldPosition.x + 0.06f, worldPosition.y + 0.34f, 0), Quaternion.identity); //Spawn the asset at the mouse position
    }
}
