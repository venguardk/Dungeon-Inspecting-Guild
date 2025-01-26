using UnityEngine;

public class Dart : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            this.gameObject.SetActive(false);
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(2);
        }
    }
}
