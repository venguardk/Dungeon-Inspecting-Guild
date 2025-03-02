using UnityEngine;
using UnityEngine.Tilemaps;

public class FollowMouse : MonoBehaviour
{
    [SerializeField] private bool isRotatable;
    //Add this script to all images that follow the mouse
    void Update()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //Updating where the mouse is
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        //Setting the image following the mouse to gird position
        worldPosition.x = Mathf.Ceil((worldPosition.x - 0.5f) / 0.96f) * 0.96f + 0.06f;
        worldPosition.y = Mathf.Ceil((worldPosition.y - 0.5f) / 0.96f) * 0.96f + 0.34f;
        transform.position = worldPosition;
        if(LevelEditorManager.instance.FloorChecker(worldPosition) == false)
        {
            if(gameObject.name != "Bridge_Image(Clone)")
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
            if(gameObject.name != "Bridge_Image(Clone)")
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
