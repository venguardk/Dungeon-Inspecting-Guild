using UnityEngine;

public class PushableBox : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // void FixedUpdate()
    // {
    //     if (rb.linearVelocity.magnitude < 0.1f)
    //     {
    //         rb.linearVelocity = Vector2.zero;
    //     }
    // }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("Enemy"))
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
