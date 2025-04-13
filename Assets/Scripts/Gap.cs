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

    private bool isFalling = false;
    //private Vector3 temPos;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player enter gap");   
            FallIntoPit(other);
        }
    }

    private void FallIntoPit(Collider2D other)
    {
        if (!isFalling)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            //Prevent the player from moving after stepping into the gap
            isFalling = true;

            

            //Visually show the player falling
            
            //Player takes 1 heart damage after falling into gap
            other.GetComponent<PlayerManager>().TakeDamage(2);

            // player spawns back to a safe spot after entering a gap
            other.transform.position = player.initialSpawn;
        }
    }
}
