using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //This movement script is based off of Introduction to Isometric Movement & Animation (8 directions) in Unity - https://youtu.be/UM9BSyGyGf0?si=Znou_TFHG17EaEHb
    private Rigidbody2D rb;
    private float moveHorizontal, moveVertical;
    [SerializeField] private float moveSpeed = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed;
        moveVertical = Input.GetAxis("Vertical") * moveSpeed;
        rb.linearVelocity = new Vector2(moveHorizontal, moveVertical);
    }
}
