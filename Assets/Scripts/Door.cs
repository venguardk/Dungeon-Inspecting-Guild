using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] public bool locked = false;
    public Collider2D doorCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorCollider = GetComponent<Collider2D>();
        if (locked)
        {
            doorCollider.isTrigger = false;
        }
        else
        {
            doorCollider.isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !locked)
        {
            SceneLoadManager.instance.LoadScene("GameWin");
        }
    }
}
