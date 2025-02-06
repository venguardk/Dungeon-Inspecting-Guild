using UnityEngine;

public class PlayerAttackbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyManager>().TakeDamage(1);
            Debug.Log("Enemy Hit");
        }
    }
}
