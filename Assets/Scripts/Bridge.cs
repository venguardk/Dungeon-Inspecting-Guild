using UnityEngine;

public class Bridge : MonoBehaviour
{
    private Collider2D bridgeCollider;
    [SerializeField] private GameObject gap;

    private void Start()
    {
        bridgeCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gap"))
        {
            gap = other.gameObject;
            gap.GetComponent<Collider2D>().enabled = false;
        }
    }

    public void EnableGap()
    {
        if (gap != null)
        {
            gap.GetComponent<Collider2D>().enabled = true;
        }
    }
}
