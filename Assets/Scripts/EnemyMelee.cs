using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMelee : MonoBehaviour
{
    // This script is used to control the EnemyMelee prefab, specifically its movement and its interactions with the player
    [SerializeField] private float chaseRange = 10f, attackRange = 1.5f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float attackDelay = 0.5f, attackDuration = 0.5f, attackCooldown = 1f;
    [SerializeField] private GameObject attackHitBox;
    [SerializeField] private AudioSource attackAudio;
    private Transform player;
    private Rigidbody2D rb;
    private bool isAttacking = false;
    private string[] possibleTypes =
{
    "fire",
    "none",
    "electric",
    "ice"
    };
    private string currentType = "none";
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player object in the scene
        rb = GetComponent<Rigidbody2D>();
        attackHitBox.SetActive(false);
    }

    void Update()
    {
        if (GameManager.instance.IsLevelEditorMode()) // If in level editor mode, stop chasing the player and do nothing
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            rb = GetComponent<Rigidbody2D>();
            attackHitBox.SetActive(false);
            StopChasingPlayer();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position); //Acquiring distance to player

        if (distanceToPlayer <= chaseRange && isAttacking == false) //If player is within chase range chase them
        {
            ChasePlayer();
            if (distanceToPlayer <= attackRange && isAttacking == false) //If player is within attack range, stop moving, and attack
            {
                isAttacking = true;
                Invoke("AttackPlayer", attackDelay); //There is a small delay before attacking
            }
        }
        else //If player is outside of chase range, stop chasing
        {
            StopChasingPlayer();
        }

    }



    void ChasePlayer()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(directionToPlayer.x * moveSpeed, directionToPlayer.y * moveSpeed);
    }

    void StopChasingPlayer()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void typeChange(string type)
    {
        bool inType = false;
        for (int i = 0; i < possibleTypes.Length; i++){
           if(type == possibleTypes[i])
              {
               inType = true;
              }
            }
            if (!inType)
            {
            return;
            }
        else
        {
            currentType = type;
        }
    } 

    void AttackPlayer()
    {
        // Stop moving
        rb.linearVelocity = Vector2.zero;

        // Activate the attack hitbox in the direction of the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        attackHitBox.transform.position = transform.position + directionToPlayer * 1f;
        attackHitBox.SetActive(true);
        attackAudio.Play();

        Invoke("AttackReset", attackDuration); // There is a small delay before the attack hitbox is deactivated
    }

    void AttackReset()
    {
        attackHitBox.SetActive(false);
        Invoke("ReturnToChase", attackCooldown);
    }

    void ReturnToChase()
    {
        isAttacking = false;
    }
}

