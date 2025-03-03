using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int health = 3;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy Health: " + health);
        if (health <= 0)
        {
            LevelEditorManager.instance.DeacitaveObj(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
