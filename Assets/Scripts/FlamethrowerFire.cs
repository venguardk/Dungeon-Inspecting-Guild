using UnityEngine;

public class FlamethrowerFire : MonoBehaviour
{
    private bool playerInRange;
    [SerializeField] private int damage = 1;
    [SerializeField] private float damageInterval = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            InvokeRepeating("DamagePlayer", 0f, damageInterval);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            CancelInvoke("DamagePlayer");
        }
    }

    private void DamagePlayer()
    {
        if (playerInRange)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().TakeDamage(damage);
        }
    }
}
