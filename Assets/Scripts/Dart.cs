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
        //Dart dissapppear when they hit objects
        else if (collision.gameObject.CompareTag("AddedItem") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Terrain"))
        {
            this.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            this.gameObject.SetActive(false);
        }
    }
}
