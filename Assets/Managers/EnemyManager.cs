using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //This script handles all enemy prefabs' stats
    //Other scripts this script interacts with: LevelEditorManager

    [SerializeField] private int health = 3;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy Health: " + health);
        if (health <= 0)
        {
            die();
        }
    }

    public void die()
    {
        LevelEditorManager.instance.DeacitaveObj(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
