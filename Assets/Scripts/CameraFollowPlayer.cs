using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    //Based on How to Make Camera Follow In UNITY 2D - https://www.youtube.com/watch?v=FXqwunFQuao
    //and Unity 2D Platformer Tutorial 8 - How To Create 2D Camera Bounds - https://www.youtube.com/watch?v=Fqht4gyqFbo
    [SerializeField] private float followSpeed = 2f;
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 minPosition, maxPosition; //For Camera Bounds

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPosition = new Vector3(target.position.x, target.position.y, -10f);

        Vector3 boundPosition = new Vector3(Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x), Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y), -10f); //Mathf.Clamp checks first argument and ensures if it is within min and max

        transform.position = Vector3.Lerp(transform.position, boundPosition, followSpeed * Time.deltaTime);
    }
}
