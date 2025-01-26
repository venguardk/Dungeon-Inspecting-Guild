using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    //Add this script to all images that follow the mouse
    void Update()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //Updating where the mouse is
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        transform.position = worldPosition;
    }
}
