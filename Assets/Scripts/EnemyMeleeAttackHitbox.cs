using UnityEngine;

public class EnemyMeleeAttackHitbox : MonoBehaviour
{
    // This script is used on the Enemy prefab's attack hitbox to deal damage to the player
    private void OnTriggerEnter2D(Collider2D other)
    {
        //If the enemy's attack hitbox collides with the player, deal 2 damage
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerManager>().TakeDamage(2);
        }
        
    }
}
