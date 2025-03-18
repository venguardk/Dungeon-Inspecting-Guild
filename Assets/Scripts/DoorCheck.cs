using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    [SerializeField] private GameObject door, doorBlock;
    [SerializeField] private AudioSource doorUnlockAudio;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerManager>().HasKey())
        {
            doorUnlockAudio.Play();
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
            collision.gameObject.GetComponent<PlayerManager>().RemoveKey();
        }
    }
}
