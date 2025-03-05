using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager instance;
    public static string previousScene = "";
    public static string sceneMovement = "";

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
        SceneManager.LoadScene("Prototype 3");
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
}
