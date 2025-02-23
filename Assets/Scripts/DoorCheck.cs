using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    [SerializeField] private GameObject door;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerManager>().HasKey())
        {
            door.GetComponent<Door>().locked = false;
            door.GetComponent<Door>().doorCollider.isTrigger = true;
            door.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<PlayerManager>().RemoveKey();
        }
    }
}
