using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResetButton : MonoBehaviour
{
    private Image buttonImage;
    private float pressTime = 0f;
    private bool isDown = false;
    [SerializeField] private TextMeshProUGUI buttonText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            pressTime += Time.deltaTime;
            buttonImage.color = new Color(1f, 1f - pressTime, 1f - pressTime);
            buttonText.text = "PRESS AND HOLD";
            if (pressTime >= 1f)
            {
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
