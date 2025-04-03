using UnityEngine;

public class Shield : MonoBehaviour
{
    // This script is attached to the Shield prefab
    void Start()
    {
        this.gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If the player collides with the shield, make the player invincible
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerManager>().BecomeInvincible();
            LevelEditorManager.instance.DeacitaveObj(this.gameObject); // Deactivate the shield as opposed to destroying it
            this.gameObject.SetActive(false);
        }
    }
}
