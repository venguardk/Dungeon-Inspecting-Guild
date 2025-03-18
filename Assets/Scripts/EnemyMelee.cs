using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] private float chaseRange = 10f, attackRange = 1.5f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float attackDelay = 0.5f, attackDuration = 0.5f, attackCooldown = 1f;
    [SerializeField] private GameObject attackHitBox;
    [SerializeField] private AudioSource attackAudio;
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
            player = GameObject.FindGameObjectWithTag("Player").transform;
            rb = GetComponent<Rigidbody2D>();
            attackHitBox.SetActive(false);
            StopChasingPlayer();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange && isAttacking == false)
        {
            ChasePlayer();
            if (distanceToPlayer <= attackRange && isAttacking == false)
            {
                isAttacking = true;
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

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        attackHitBox.transform.position = transform.position + directionToPlayer * 1f;
        attackHitBox.SetActive(true);
        attackAudio.Play();
        Debug.Log("Attack!");

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
