using UnityEngine;

public class PlayerAttackbox : MonoBehaviour
{
    // This script is attached to the Player attack hitbox game object within the player prefab
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If an enemy collides with the attack hitbox, deal damage to the enemy
        if (other.CompareTag("Enemy"))
        {
            Vector2 knockbackDirection = (other.transform.position - PlayerManager.instance.transform.position).normalized;
            other.GetComponent<EnemyManager>().TakeDamage(1, knockbackDirection);
        }
    }
}
