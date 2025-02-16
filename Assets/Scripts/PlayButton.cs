using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour, IPointerEnterHandler
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LevelEditorManager.instance.DeactivateButton();
    }
}
