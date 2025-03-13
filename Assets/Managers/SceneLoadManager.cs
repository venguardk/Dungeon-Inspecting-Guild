using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour, IDataPersistence
{
    public static SceneLoadManager instance;
    public static string previousScene = "";
    public static string sceneMovement = "";
    private bool NewGame = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlayLevel()
    {
        sceneMovement = SceneManager.GetActiveScene().name;
        NewGame = false;
        SceneManager.LoadScene("Prototype 3");
    }

    public void ContinueLevel()
    {
        if(NewGame == false)
        {
            sceneMovement = "ContinueLevel";
            Debug.Log("Continue");
        }
        else
        {
            sceneMovement = SceneManager.GetActiveScene().name;
            NewGame = false;
        }
        
        SceneManager.LoadScene("Prototype 3");
    }

    public void LoadInstructions()
    {
        sceneMovement = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("InstructionsPage");
    }

    public void LoadScene(string sceneName)
    {
        sceneMovement = SceneManager.GetActiveScene().name;
        previousScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadPreviousScene()
    {
        sceneMovement = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(previousScene);
    }

    public void RestartScene()
    {
        sceneMovement = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        sceneMovement = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("MainMenu");
    }

    public void SaveData(ref GameData data)
    {
        return;
    }

    public void LoadData(GameData data)
    {
        return;
    }
    public void SaveOption(ref OptionData optionData)
    {

        optionData.NewGame = this.NewGame;
    }

    public void LoadOption(OptionData optionData)
    {
        this.NewGame = optionData.NewGame;
    }
}
