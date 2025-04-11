using UnityEngine;

public class Gap : MonoBehaviour
{
    // private void OnTriggerEnter2D(Collider2D other)
    // this will check if the player is on top of the the gap, if so
    // - prevent player from moving
    // - show fall indicator
    // - player loses a heart
    // - spawn player to a safe spot
    // if a monster wanders into the gap
    // - it dies
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }
}
