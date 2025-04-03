using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    //Based on How to Make Camera Follow In UNITY 2D - https://www.youtube.com/watch?v=FXqwunFQuao
    //and Unity 2D Platformer Tutorial 8 - How To Create 2D Camera Bounds - https://www.youtube.com/watch?v=Fqht4gyqFbo
    //This script is used to control the camera's position and movement in the game during play mode; It follows the player doesn't go out of the map bounds

    [SerializeField] private float followSpeed = 2f;
    [SerializeField] private Transform target; //The target is the Player game object, assigned in the Unity inspector
    [SerializeField] private Vector2 minPosition, maxPosition; //For Camera Bounds

    void FixedUpdate()
    {
        Vector3 newPosition = new Vector3(target.GetChild(0).position.x, target.GetChild(0).position.y, -10f); //Acquire position of target

        Vector3 boundPosition = new Vector3(Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x), Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y), -10f); //Mathf.Clamp checks first argument and ensures if it is within min and max

        transform.position = Vector3.Lerp(transform.position, boundPosition, followSpeed * Time.deltaTime); //Smoothly move the camera to the new position
    }
}
