using UnityEngine;

public class Dart : MonoBehaviour
{
    // This script is used on the Dart prefab and its interactions with the player and other objects
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) //If the dart hits the player, deal 2 damage and deactivate the dart
        {
            this.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            this.gameObject.SetActive(false);
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(2);
        }
        //Dart dissapppear when they hit objects
        else if (collision.gameObject.CompareTag("AddedItem") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("Projectile"))
        {
            this.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            this.gameObject.SetActive(false);
        }
    }
}
