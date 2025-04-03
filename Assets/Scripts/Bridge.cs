using UnityEngine;

public class Bridge : MonoBehaviour
{
    // This script is used to control the Bridge prefab and its interactions with gaps
    private Collider2D bridgeCollider;
    [SerializeField] private GameObject gap;

    private void Start()
    {
        bridgeCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the bridge collides with a gap, disable the gap's collider
        if (other.CompareTag("Gap"))
        {
            gap = other.gameObject;
            gap.GetComponent<Collider2D>().enabled = false;
        }
    }

    public void EnableGap()
    {
        // Enable the gap's collider when the bridge is destroyed
        // This method is called from the AssetManager script for when the bridge is removed via right-click
        if (gap != null)
        {
            gap.GetComponent<Collider2D>().enabled = true;
        }
    }
}
