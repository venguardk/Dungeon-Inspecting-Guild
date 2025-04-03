using UnityEngine;

public class PushableBox : MonoBehaviour
{
    // This script is attached to the PushableBox prefab
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("AddedItem"))
        {
            // If the player, projectile, enemy, or added item no longer touches the box, stop the box's movement to prevent it from sliding
            rb.linearVelocity = Vector2.zero;
        }
    }
}
