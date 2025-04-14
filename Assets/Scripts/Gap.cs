using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine;
using System.Threading;


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

    public bool isFalling = false;
    PlayerManager player;
    private Vector3 temPos;

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
            isFalling = true;
            player = other.GetComponent<PlayerManager>();

            //Prevent the player from moving after stepping into the gap
            DisableMovement();

            //Visually show the player falling
            Debug.Log("Show player falling");
            //Move game object to the center of the gap and shrink
            Vector3 fromPos = other.transform.position;
            Vector3 toPos = transform.position;

            Vector3 from = other.transform.localScale;
            Vector3 to = new Vector3(0f, 0f, 0f);
            StartCoroutine(SmoothScale(other,fromPos, toPos, from, to, 1.5f));

            //Player takes 1 heart damage after falling into gap
            other.GetComponent<PlayerManager>().TakeDamage(2);

            // player spawns back to a safe spot after entering a gap
            StartCoroutine(Respawn(other));

            Invoke("ResetFallFlag", 1f);
        }
    }

    //Respawns the player back to the safe zone
    IEnumerator Respawn(Collider2D other)
    {

        yield return new WaitForSeconds(2f);
        //Returns the player back to the initial spot where it was first spawned at
        other.transform.position = player.initialSpawn;
        player.canMove = true;
        Debug.Log("Player can move");
        //Rescales the player back to normal size
        other.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    //Disables the player's movement
    private void DisableMovement()
    {
        Debug.Log("Disable player movement");
        player.canMove = false;
    }

    //To create the smooth animation of the player falling deeper into the gap.
    //It smoothly scales down the player to 0
    //Using Lerp to create the smooth transition
    IEnumerator SmoothScale(Collider2D other, Vector3 startPos, Vector3 endPos, Vector3 startScale, Vector3 endScale, float duration)
    {
        float time = 0f;
        
        //Slowly change the scale of the game object
        while (time < duration)
        {
            other.transform.localScale = Vector3.Lerp(startScale, endScale, time / duration);
            other.transform.position = Vector3.Lerp(startPos, endPos, time / duration);
            time += Time.deltaTime;
               
            yield return null; // Wait one frame
        }

        //Snaps to final value
        other.transform.localScale = endScale;
    }

    private void ResetFallFlag()
    {
        isFalling = false;
    }
}
