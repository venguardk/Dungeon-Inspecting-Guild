using UnityEngine;

public class Key : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager.instance.AddKey();
            LevelEditorManager.instance.deeacitaveObj(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
