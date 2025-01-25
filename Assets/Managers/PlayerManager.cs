using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Vector2 movementInput, movementVelocity, playerDirection;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private GameObject attackHitBox;
    [SerializeField] private float attackDuration = 0.1f, attackRange = 1f;
    private bool isAttacking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        playerDirection = Vector2.down;
        attackHitBox.SetActive(false);
    }

    private void Update()
    {
        if (isAttacking)
        {
            movementInput = Vector2.zero;
            return; //Stops player from moving while attacking
        }

        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (movementInput != Vector2.zero)
        {
            playerDirection = movementInput;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isAttacking == false)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            movementVelocity = movementInput * moveSpeed;
            rb.linearVelocity = movementVelocity;
            FlipSprite();
        }
    }

    private void Attack()
    {
        isAttacking = true;
        Vector3 direction = playerDirection;
        attackHitBox.transform.position = transform.position + direction * attackRange;

        attackHitBox.SetActive(true);
        Invoke(nameof(EndAttack), attackDuration); //Calls EndAttack after attackDuration seconds
    }

    private void EndAttack()
    {
        attackHitBox.SetActive(false);
        isAttacking = false;
    }

    private void FlipSprite()
    {
        if (playerDirection == Vector2.left)
        {
            sr.flipX = true;
        }
        else if (playerDirection == Vector2.right)
        {
            sr.flipX = false;
        }
    }
}
