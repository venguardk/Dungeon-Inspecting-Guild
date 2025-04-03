using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour, IPointerEnterHandler
{
    // This script is attached to the quest button prefab in the level editor UI

    private Button button;
    private Image buttonImage;

    private void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        // Check if the level values are met to enable button interactivity
        if (LevelEditorManager.instance.LevelValuesMet())
        {
            SetButtonActive();
        }
        else
        {
            SetButtonInactive();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Deactivate the current asset button when the mouse hovers over it
        LevelEditorManager.instance.DeactivateButton();
    }

    private void SetButtonActive()
    {
        // Become interactable
        button.interactable = true;
        buttonImage.color = Color.white;
    }

    private void SetButtonInactive()
    {
        // Become un-interactable
        button.interactable = false;
        buttonImage.color = new Color(159f / 255f, 137f / 255f, 101f / 255f);
    }
}
