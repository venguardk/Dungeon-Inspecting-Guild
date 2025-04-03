using UnityEngine;

public class Door : MonoBehaviour
{
    // This script is used to control the Door prefab and its interactions with the player
    // The door check script is used on the Door prefab's trigger zone to determine when this object's locked value becomes true

    [SerializeField] public bool locked = false; // Indicates if the door is locked; Adjustable in the Unity inspector
    [SerializeField] private GameObject doorBlock;
    public Collider2D doorCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorCollider = GetComponent<Collider2D>();
        if (locked)
        {
            doorCollider.isTrigger = false;
            doorBlock.SetActive(true);
        }
        else
        {
            doorCollider.isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // If the door is unlocked and the player collides with it, win the level
        if (collision.gameObject.CompareTag("Player") && !locked)
        {
            SceneLoadManager.instance.LoadScene("GameWin");
        }
    }
}
