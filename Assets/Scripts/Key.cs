using UnityEngine;

public class Key : MonoBehaviour
{
    // This script is attached to the Key prefab
    void Start()
    {
        this.gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // If the player collides with the key, add it to the player's inventory
            PlayerManager.instance.AddKey();
            LevelEditorManager.instance.DeacitaveObj(this.gameObject); // Deactivate the key as opposed to destroying it
            this.gameObject.SetActive(false);
        }
    }
}
