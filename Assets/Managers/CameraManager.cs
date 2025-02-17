using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [SerializeField] private Camera levelEditorCamera, playModeCamera;

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

    void Start()
    {
        levelEditorCamera.gameObject.SetActive(true);
        playModeCamera.gameObject.SetActive(false);
    }

    public void SwitchToPlayModeCamera()
    {
        levelEditorCamera.gameObject.SetActive(false);
        playModeCamera.gameObject.SetActive(true);
    }

    public void SwitchToLevelEditorCamera()
    {
        levelEditorCamera.gameObject.SetActive(true);
        playModeCamera.gameObject.SetActive(false);
    }
}
