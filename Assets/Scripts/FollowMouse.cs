using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    //Add this script to all images that follow the mouse
    [SerializeField] private bool isRotatable;
    private bool rotateIsOnCooldown = false;
    private float rotateCooldown = 0.2f; //Cooldown for rotation to prevent spamming
    //PlayerActions controls;

    private void Awake()
    {
        //controls = new PlayerActions(); //player controls instance, not sure if needed
        rotateIsOnCooldown = false;
    }

    void Update()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //Updating where the mouse is
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        //Setting the image following the mouse to gird position
        worldPosition.x = Mathf.Ceil(worldPosition.x - 0.75f) + 0.5f;
        worldPosition.y = Mathf.Ceil(worldPosition.y - 0.75f) + 0.5f;
        transform.position = worldPosition;
        if (LevelEditorManager.instance.FloorChecker(worldPosition) == false)
        {
            if (gameObject.name != "Bridge_Image(Clone)")
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            else if (LevelEditorManager.instance.GapChecker(worldPosition) == false)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else
        {
            if (gameObject.name != "Bridge_Image(Clone)")
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }

        }

        if (isRotatable)
        { //If the asset is rotatable, rotate it by 90 right when E is pressed, or 90 left when Q is pressed
            if (PlayerControlsOption.instance.isOneHandMode == true)
            {
                float scrollInput = Input.GetAxis("Mouse ScrollWheel");
                if (scrollInput > 0f && rotateIsOnCooldown == false) // Scroll up
                {
                    rotateIsOnCooldown = true;
                    transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 90);
                    Invoke("ResetRotateCooldown", rotateCooldown); // Start cooldown
                }
                else if (scrollInput < 0f && rotateIsOnCooldown == false) // Scroll down
                {
                    rotateIsOnCooldown = true;
                    transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 90);
                    Invoke("ResetRotateCooldown", rotateCooldown);
                }

                if (Input.GetButtonDown("JoystickButton3"))
                {
                    transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 90);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 90);
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 90);
                }
            }
        }
    }

    private void ResetRotateCooldown()
    {
        rotateIsOnCooldown = false;
    }

    // input system for rotating left
    public void OnRotateLeft(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log("Rotate Left");
            if (isRotatable)
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 90);
            }

        }

    }

    // input system for rotating right
    public void OnRotateRight(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log("Rotate Right");
            if (isRotatable)
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 90);
            }

        }

    }


}
