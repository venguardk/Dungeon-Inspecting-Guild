using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //This script handles all enemy prefabs' stats
    //Other scripts this script interacts with: LevelEditorManager

    [SerializeField] private int health = 3;
    [SerializeField] private float knockbackForce = 5f;

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        health -= damage;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Apply knockback force to the enemy
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        if (health <= 0)
        {
            LevelEditorManager.instance.DeacitaveObj(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
