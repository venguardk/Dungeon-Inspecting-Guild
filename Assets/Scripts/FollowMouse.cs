using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    [SerializeField] private bool isRotatable;
    //Add this script to all images that follow the mouse
    void Update()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //Updating where the mouse is
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        //Setting the image following the mouse to gird position
        worldPosition.x = Mathf.Ceil(worldPosition.x - 0.5f);
        worldPosition.y = Mathf.Ceil(worldPosition.y - 0.5f);
        transform.position = worldPosition;

        if (Input.GetKeyDown(KeyCode.Space) && isRotatable)
        { //If the asset is rotatable, rotate it by 90 when spacebar is pressed
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 90);
        }
    }
}
