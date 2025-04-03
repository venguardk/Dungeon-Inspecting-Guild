using UnityEngine;

public class Potion : MonoBehaviour
{
    // This script is attached to the Potion prefab
    void Start()
    {
        this.gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // If the player collides with the potion, heal the player to full health
            other.GetComponent<PlayerManager>().FullHeal(); // Heal the player to full health
            LevelEditorManager.instance.DeacitaveObj(this.gameObject); // Deactivate the potion as opposed to destroying it
            this.gameObject.SetActive(false);
        }
    }
}
