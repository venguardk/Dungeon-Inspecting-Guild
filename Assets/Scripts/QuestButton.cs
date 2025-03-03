using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour, IPointerEnterHandler
{
    private Button button;
    private Image buttonImage;

    private void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
    }

    private void FixedUpdate()
    {
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
        LevelEditorManager.instance.DeactivateButton();
    }

    private void SetButtonActive()
    {
        button.interactable = true;
        buttonImage.color = Color.white;
    }

    private void SetButtonInactive()
    {
        button.interactable = false;
        buttonImage.color = new Color(159f / 255f, 137f / 255f, 101f / 255f);
    }
}
