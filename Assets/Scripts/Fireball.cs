using System;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    // This script is used on the Fireball prefab that is produced by the Flamethrower prefab, and its interactions with the player and other objects
    [SerializeField] private float maxDistance = 5f;
    private Vector3 startPosition;
    private float distanceTravelled;
    private string currentType = "fire";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UnityEngine.Debug.Log(collision.gameObject.name);
        // If the fireball hits the player, deal 1 damage and deactivate the fireball
        if (collision.gameObject.CompareTag("Enemy"))
        {
            UnityEngine.Debug.Log("firefire");
            EnemyMelee enemyMelee = collision.gameObject.GetComponent<EnemyMelee>();
            if (enemyMelee != null)
            {
                enemyMelee.typeChange(currentType);
                UnityEngine.Debug.Log("firechu");
            }
        }
        else if (collision.gameObject.CompareTag("Projectile"))
        {
            Dart dart = collision.gameObject.GetComponent<Dart>();
            if (dart != null)
            {
                dart.typeChange(currentType);
                UnityEngine.Debug.Log("firearrow");
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(1);
            DeactivateFireball();
        }
        else if (collision.gameObject.CompareTag("AddedItem") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Terrain"))
        {
            // Fireball disappear when they hit non-player objects such as walls
            DeactivateFireball();
        }

    }

    public void StartDistanceCount() //When initially fired, the fireball will be 1/3 its normal size
    {
        startPosition = transform.position;
        transform.localScale = new Vector3(1f / 3f, 1f / 3f, 1f / 3f);
    }

    private void Update()
    {
        // Calculate the distance travelled by the fireball
        distanceTravelled = Vector3.Distance(startPosition, transform.position);

        // The farther the fireball travels, the larger it becomes until it reaches its normal size
        float scaleMultiplier = Mathf.Lerp(1f / 3f, 1f, distanceTravelled / maxDistance);
        transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);

        // If the fireball travels beyond the max distance, deactivate it
        if (distanceTravelled >= maxDistance || GameManager.instance.IsLevelEditorMode())
        {
            DeactivateFireball();
        }
    }

    private void DeactivateFireball()
    {
        this.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        this.gameObject.SetActive(false);
        distanceTravelled = 0;
    }
}
