using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileID = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContnet;
    [SerializeField] private GameObject hasDataContnet;
    [SerializeField] private TextMeshProUGUI namedText;

    private Button saveSlotButton;

    private void Awake()
    {
        saveSlotButton = this.GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        if (data == null || data.sceneName == "")
        {
            noDataContnet.SetActive(true);
            hasDataContnet.SetActive(false);
            DataPersistenceManager.instance.DeleteProfileData(profileID);
        }
        else
        {
            noDataContnet.SetActive(false);
            hasDataContnet.SetActive(true);

            namedText.text = data.levelName;
        }
    }

    public string GetProfileID()
    {
        return this.profileID;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
    }
}
