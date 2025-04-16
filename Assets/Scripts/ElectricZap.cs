using UnityEngine;

public class Zaps : MonoBehaviour
{
    // Collison handling file
    // This script is attached to the ElectricPlate child within the Zap prefab; The child is the trigger zone that interacts with the player
    // The child is controlled via the ElectricPlate script, which is attached to the parent object

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player collides with the spikes, deal 3 damage to the player
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(3);
            collision.gameObject.GetComponent<PlayerManager>().GetStunned(2);
        }
    }
}
