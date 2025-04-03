using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResetButton : MonoBehaviour
{
    // This script is attached to the Reset button in the level editor UI; The function calls are applied via the Unity inspector
    // Other scripts that this script interacts with: SceneLoadManager

    private Image buttonImage;
    private float pressTime = 0f;
    private bool isDown = false;
    [SerializeField] private TextMeshProUGUI buttonText;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        pressTime = 0f;
        isDown = false;
    }

    private void FixedUpdate()
    {
        if (isDown == true)
        {
            // If the button is being pressed, change the color of the button to red and display the press time
            pressTime += Time.deltaTime; // Increment the press time
            buttonImage.color = new Color(1f, 1f - pressTime, 1f - pressTime); // Change the color of the button to become more red
            buttonText.text = "PRESS AND HOLD";
            if (pressTime >= 1f)
            {
                // If the button is pressed for 1 second, restart the scene
                SceneLoadManager.instance.RestartScene();
                pressTime = 0f;
            }
        }
        else
        {
            pressTime = 0f;
            buttonImage.color = Color.white;
            buttonText.text = "RESET";
        }
    }

    public void ButtonDown()
    {
        isDown = true;
    }

    public void ButtonUp()
    {
        isDown = false;
    }
}
