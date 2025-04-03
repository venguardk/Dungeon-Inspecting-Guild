using UnityEngine;

public class Spikes : MonoBehaviour
{
    // This script is attached to the Spikes child within the Spikes prefab; The child is the trigger zone that interacts with the player
    // The child is controlled via the SpikeTrap script, which is attached to the parent object

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player collides with the spikes, deal 3 damage to the player
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(3);
        }
    }
}
