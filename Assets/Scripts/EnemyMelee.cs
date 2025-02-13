using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] private float chaseRange = 10f, attackRange = 0.95f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float attackDelay = 0.5f, attackDuration = 0.5f, attackCooldown = 1f;
    [SerializeField] private GameObject attackHitBox;
    private Transform player;
    private Rigidbody2D rb;
    private bool isAttacking = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        attackHitBox.SetActive(false);
    }

    void Update()
    {
        if (GameManager.instance.IsLevelEditorMode())
        {
            return;
        }
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange && isAttacking == false)
        {
            ChasePlayer();
            if (distanceToPlayer <= attackRange && isAttacking == false)
            {
                Invoke("AttackPlayer", attackDelay);
            }
        }
        else
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

    void AttackPlayer()
    {
        //Fail if enemy gets hurt
        rb.linearVelocity = Vector2.zero;
        isAttacking = true;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        attackHitBox.transform.position = transform.position + directionToPlayer * 1f;
        attackHitBox.SetActive(true);

        Invoke("AttackReset", attackDuration);
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
