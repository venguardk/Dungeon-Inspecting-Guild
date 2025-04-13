using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsManager : MonoBehaviour, IDataPersistence
{
    public static SaveSlotsManager instance;
    private SaveSlot[] saveSlots;

    [SerializeField] private Button backButton;
    [SerializeField] TMP_InputField dungeonName;

    private bool textFocus;

    private string levelName;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        dungeonName.onSelect.AddListener(OnInputFieldSelected);
        dungeonName.onDeselect.AddListener(OnInputFieldDeselected);
        ActivateMenu();
    }

    void OnInputFieldSelected(string text)
    {
        textFocus = true;
    }

    void OnInputFieldDeselected(string text)
    {
        textFocus = false;
    }

    public void ActivateMenu()
    {
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileID(), out profileData);
            saveSlot.SetData(profileData);
            if(profileData == null && SceneLoadManager.sceneMovement == "ContinueLevel")
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
            }
        }
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();
        DataPersistenceManager.instance.ChangeSelectedProfile(saveSlot.GetProfileID());
        if (SceneLoadManager.sceneMovement != "ContinueLevel")
        {
            DataPersistenceManager.instance.NewGame();
            
            dungeonName.gameObject.SetActive(true);
            //dungeonName.gameObject.transform.position = saveSlot.gameObject.transform.position;

            StartCoroutine(InputHalt());
            

            
        }
        else
        {
            SceneManager.LoadScene("Prototype 3");
        }
    }

    IEnumerator InputHalt()
    {
        Debug.Log(textFocus);
        yield return new WaitUntil(() => textFocus);
        Debug.Log(textFocus);
        yield return new WaitUntil(() => !textFocus);
        levelName = dungeonName.text;
        dungeonName.gameObject.SetActive(false);
        SceneManager.LoadScene("Prototype 3");
    }

    private void DisableMenuButtons()
    {
        foreach (SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }

    public void LoadData(GameData data)
    {
        return;
    }

    public void SaveData(ref GameData data)
    {
        data.levelName = this.levelName;
    }

    public void LoadOption(OptionData data)
    {
        return;
    }

    public void SaveOption(ref OptionData data)
    {
        return;
    }
}
