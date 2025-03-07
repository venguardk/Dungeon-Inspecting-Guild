using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    private Vector2 movementInput, movementVelocity, playerDirection;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private GameObject attackHitBox;
    [SerializeField] private float attackDuration = 0.1f, attackRange = 1f;
    [SerializeField] private int health = 10;
    private bool isAttacking, isInvincible;
    private int keyCount = 0;

    private void Awake()
    {

        instance = this;

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        playerDirection = Vector2.down;
        attackHitBox.SetActive(false);
        health = 10;
        keyCount = 0;
        isInvincible = false;
    }

    private void Update()
    {
        if (health <= 0)
        {
            playerDirection = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
            movementInput = Vector2.zero;
            keyCount = 0;
            FullHeal();
            keyCount = 0;
            UIManager.instance.SwitchToLevelEditor();
            UIManager.instance.ShowLossScreen();
        }

        if (GameManager.instance.IsLevelEditorMode())
        {
            return;
        }

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

        if (Input.GetMouseButtonDown(0) && GameManager.instance.IsLevelEditorMode() == false && isAttacking == false)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.IsLevelEditorMode())
        {
            return;
        }
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

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - transform.position).normalized;

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

    public void TakeDamage(int damage)
    {
        if (isInvincible == false && !GameManager.instance.IsLevelEditorMode())
        {
            health -= damage;
        }
    }

    public void FullHeal()
    {
        health = 10;
    }

    public void AddKey()
    {
        keyCount++;
    }

    public void RemoveKey()
    {
        keyCount--;
    }

    public bool HasKey()
    {
        return keyCount > 0;
    }

    public void BecomeInvincible()
    {
        isInvincible = true;
        Debug.Log("Player is now invincible!");
        Invoke(nameof(EndInvincibility), 5f); //Calls EndInvincibility after 5 seconds
    }

    private void EndInvincibility()
    {
        isInvincible = false;
        Debug.Log("Player is no longer invincible!");
    }

    //FOR UI
    public int GetPlayerHealth()
    {
        return health;
    }

    //FOR RESET
    public void DestroyPlayer()
    {
        Destroy(this);
    }
}
