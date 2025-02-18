using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float maxDistance = 5f;
    private Vector3 startPosition;
    private float distanceTravelled;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(1);
            DeactivateFireball();
        }
        else if (collision.gameObject.CompareTag("AddedItem") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Terrain"))
        {
            DeactivateFireball();
        }
    }

    public void StartDistanceCount()
    {
        startPosition = transform.position;
        transform.localScale = new Vector3(1f / 3f, 1f / 3f, 1f / 3f);

    }

    private void Update()
    {
        distanceTravelled = Vector3.Distance(startPosition, transform.position);

        float scaleMultiplier = Mathf.Lerp(1f / 3f, 1f, distanceTravelled / maxDistance);
        transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);

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
