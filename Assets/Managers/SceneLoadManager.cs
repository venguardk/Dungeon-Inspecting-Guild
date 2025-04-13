using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour, IDataPersistence
{
    //This script handles the transition between scenes and the saving/loading of data
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

    public void PlayLevel() //Go to main level
    {
        sceneMovement = SceneManager.GetActiveScene().name;
        NewGame = false;
        SceneManager.LoadScene("SaveSlotsScene");
    }

    public void ContinueLevel() //Go to main level and continue from where the player left off

    {
        if (NewGame == false)
        {
            sceneMovement = "ContinueLevel";
            Debug.Log("Continue");
        }
        else
        {
            sceneMovement = SceneManager.GetActiveScene().name;
            NewGame = false;
        }

        SceneManager.LoadScene("SaveSlotsScene");
    }

    public void LoadInstructions() //Go to instructions page
    {
        sceneMovement = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("InstructionsPage");
    }

    public void LoadScene(string sceneName) //Load a scene by name
    {
        sceneMovement = SceneManager.GetActiveScene().name;
        previousScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadPreviousScene() //Return to previous scene
    {
        sceneMovement = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(previousScene);
    }

    public void RestartScene() //Restart current scene
    {
        sceneMovement = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu() //Return to main menu
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

    public void ExitGame() //Close game
    {
        Application.Quit();
    }
}
