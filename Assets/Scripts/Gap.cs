using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine;
using System.Threading;
using UnityEngine.Rendering;


public class Gap : MonoBehaviour
{
    public bool playerIsFalling = false;
    PlayerManager player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Checks the type of game object that's interacting with the gaps
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player enter gap");
            player = other.GetComponent<PlayerManager>();
            PlayerFallIntoPit(other);
        }
        else if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy enter gap");
            EnemyFallIntoPit(other);
        }
        else if (other.name == "Pushable_Box(Clone)")
        {
            Debug.Log("Box enter gap");
            StartCoroutine(BoxToBridge(other));
        }
    }

    private void PlayerFallIntoPit(Collider2D other)
    {
        if (!playerIsFalling)
        {
            playerIsFalling = true;
           
            //Prevent the player from moving after stepping into the gap
            DisableMovement();

            //Visually show the player falling
            Debug.Log("Show player falling");
            //Move game object to the center of the gap and shrink
            FallEffect(other);

            //Player takes 1 heart damage after falling into gap
            other.GetComponent<PlayerManager>().TakeDamage(2);

            //Player spawns back to a safe spot after entering a gap
            StartCoroutine(Respawn(other));

            //Cooldown to prevent multiple triggers from interacting with the gaps
            Invoke("ResetFallFlag", 1f);
        }
    }

    private void EnemyFallIntoPit(Collider2D other)
    {
        FallEffect(other);

        StartCoroutine(RemoveEnemy(other)); //Used to let the fall animation play out before killing off the enemy
    }

    IEnumerator BoxToBridge(Collider2D other)
    {
        // ===== Shift box into position ====
        Vector3 fromPos = other.transform.position;
        Vector3 toPos = transform.position;

        float time = 0f;
        float duration = 0.3f;

        while (time < duration)
        {
            other.transform.position = Vector3.Lerp(fromPos, toPos, time / duration);
            time += Time.deltaTime;

            yield return null;
        }

        other.transform.position = toPos;

        // ==================================

        //Scale up the box to fill up the gap. Will change this when we have a sprite
        other.transform.localScale = new Vector3(4f, 4f, 4f);

        //Set layers to gap to prevent pushable box being underneath the placed box
        SpriteRenderer sr = other.GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "Default";
        sr.sortingOrder = 0;

        //Disables collider for gap and box
        this.GetComponent<Collider2D>().enabled = false;
        other.GetComponent<Collider2D>().enabled = false;
    }

    IEnumerator RemoveEnemy(Collider2D other)
    {
        //Wait 2 seconds before killing the enemy
        yield return new WaitForSeconds(2f);
        other.GetComponent<EnemyManager>().die();
    }

    private void FallEffect(Collider2D other)
    {
        Vector3 fromPos = other.transform.position; //The game object that interacted with the gap
        Vector3 toPos = transform.position;         //The gap's position

        Vector3 from = other.transform.localScale;  //The current scale of the game object
        Vector3 to = new Vector3(0f, 0f, 0f);       //The desire scale size when we want to scale the game object to
        StartCoroutine(SmoothFall(other, fromPos, toPos, from, to, 1.5f));
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
    IEnumerator SmoothFall(Collider2D other, Vector3 startPos, Vector3 endPos, Vector3 startScale, Vector3 endScale, float duration)
    {
        float time = 0f;
        
        //Slowly overtime scales down the game object
        while (time < duration)
        {
            other.transform.localScale = Vector3.Lerp(startScale, endScale, time / duration);   //slowly scales down the object
            other.transform.position = Vector3.Lerp(startPos, endPos, time / duration);         //slowly positions the object to the center of the gap
            time += Time.deltaTime;
               
            yield return null; // Wait one frame
        }

        //Snaps to final value
        other.transform.localScale = endScale;
    }

    private void ResetFallFlag()
    {
        playerIsFalling = false;
    }
}
