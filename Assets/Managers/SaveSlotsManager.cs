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

    [SerializeField] private Button switchButton1;
    [SerializeField] private Button switchButton2;
    [SerializeField] private Button backButton;
    [SerializeField] TMP_InputField dungeonName;

    private bool ContNew = true;

    private bool textFocus;

    private string levelName;

    private string sceneName;

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
            if(profileData == null && ContNew == false)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
            }
        }
    }


    public void OnModeSwitched()
    {
        if (ContNew == true)
        {
            ContNew = false;
        }
        else
        {
            ContNew = true;
        }
        ActivateMenu();
    }

    public void OnSaveSlotClickedCreative(SaveSlot saveSlot)
    {
        DisableMenuButtons();
        DataPersistenceManager.instance.ChangeSelectedProfile(saveSlot.GetProfileID());

        if (ContNew == true)
        {
            DataPersistenceManager.instance.ResetGame();
            
            dungeonName.gameObject.SetActive(true);

            StartCoroutine(InputHalt());
        }
        else
        {
            SceneLoadManager.sceneMovement = "Continue";
            SceneManager.LoadScene(sceneName);
        }
    }

    public void OnSaveSlotClickedCommunity(SaveSlot saveSlot)
    {
        DisableMenuButtons();
        DataPersistenceManager.instance.ChangeSelectedProfile(saveSlot.GetProfileID());

        if (ContNew == true)
        {
            DataPersistenceManager.instance.ResetGame();

            dungeonName.gameObject.SetActive(true);

            StartCoroutine(InputHaltCommunity());
        }
        else
        {
            SceneLoadManager.sceneMovement = "Continue";
            SceneManager.LoadScene(sceneName);
        }
    }

    IEnumerator InputHalt()
    {
        yield return new WaitUntil(() => textFocus);
        yield return new WaitUntil(() => !textFocus);
        levelName = dungeonName.text;
        dungeonName.gameObject.SetActive(false);

        SceneManager.LoadScene("CreativeMenu");
    }

    IEnumerator InputHaltCommunity()
    {
        yield return new WaitUntil(() => textFocus);
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
        switchButton1.interactable = false;
        switchButton2.interactable = false;
        backButton.interactable = false;
    }

    public void LoadData(GameData data)
    {
        this.levelName = data.levelName;
        this.sceneName = data.sceneName;
    }

    public void SaveData(ref GameData data)
    {
        data.levelName = this.levelName;
        data.sceneName = this.sceneName;
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
