using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    // This script is used on the Door prefab's trigger zone to determine when this object's locked value becomes true
    [SerializeField] private GameObject door, doorBlock;
    [SerializeField] private AudioSource doorUnlockAudio;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // If the door is locked and the player enters the trigger zone and they have a key, unlock the door
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerManager>().HasKey())
        {
            doorUnlockAudio.Play(); // Play the door unlock sound
            door.GetComponent<Door>().locked = false;
            door.GetComponent<Door>().doorCollider.isTrigger = true;
            door.GetComponent<SpriteRenderer>().enabled = false;
            doorBlock.SetActive(false);

            // adjusting to get rid of locked door sprites
            for (int i = 1; i < door.transform.childCount - 1; i++)
            {
                GameObject door_lock = door.transform.GetChild(i).gameObject;
                door_lock.GetComponent<SpriteRenderer>().enabled = false;
            }
            // end of additional code

            collision.gameObject.GetComponent<PlayerManager>().RemoveKey(); // Subtract key from player stats
        }
    }
}
