using UnityEngine;

public class GamepadCursorController : MonoBehaviour
{
    [SerializeField] private GameObject LevelEditorGC, PlayGC, LossGC; // Reference to the GamepadCursor script
    [SerializeField] private Canvas LevelEditorCanvas, PlayCanvas, LossCanvas; // Reference to the LevelEditorManager script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LevelEditorGC.SetActive(true); // Activate the Level Editor Gamepad Cursor
        PlayGC.SetActive(false); // Deactivate the Play Gamepad Cursor
        LossGC.SetActive(false); // Deactivate the Loss Gamepad Cursor
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelEditorCanvas.enabled)
        {
            LevelEditorGC.SetActive(true); // Activate the Level Editor Gamepad Cursor
            PlayGC.SetActive(false); // Deactivate the Play Gamepad Cursor
            LossGC.SetActive(false); // Deactivate the Loss Gamepad Cursor
        }
        else if (PlayCanvas.enabled)
        {
            LevelEditorGC.SetActive(false); // Deactivate the Level Editor Gamepad Cursor
            PlayGC.SetActive(true); // Activate the Play Gamepad Cursor
            LossGC.SetActive(false); // Deactivate the Loss Gamepad Cursor
        }
        else if (LossCanvas.enabled)
        {
            LevelEditorGC.SetActive(false); // Deactivate the Level Editor Gamepad Cursor
            PlayGC.SetActive(false); // Deactivate the Play Gamepad Cursor
            LossGC.SetActive(true); // Activate the Loss Gamepad Cursor
        }
    }
}
