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
    [SerializeField] private SpriteRenderer playerSprite;
    private bool isAttacking, isInvincible, isHurt;
    private int keyCount = 0;
    private Animator animator;  // animation component

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
        animator = GetComponent<Animator>();    // establishing animator
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
        //Debug.Log(movementInput);
        if (movementInput != Vector2.zero)
        {
            animator.SetBool("isWalking", true);    // sets to walking
            playerDirection = movementInput;
            animator.SetFloat("lastInputX", movementInput.x);   // grabs last input
            animator.SetFloat("lastInputY", movementInput.y);

        }
        else
        {
            animator.SetBool("isWalking", false);    // sets to idle

        }

        animator.SetFloat("inputX", movementInput.x); // conditionals for blend state
        animator.SetFloat("inputY", movementInput.y);

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
        }
    }

    private void Attack()
    {
        isAttacking = true;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - transform.position).normalized;

        attackHitBox.transform.position = transform.position + direction * attackRange;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90;
        attackHitBox.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        attackHitBox.SetActive(true);
        Invoke(nameof(EndAttack), attackDuration); //Calls EndAttack after attackDuration seconds
    }

    private void EndAttack()
    {
        attackHitBox.SetActive(false);
        isAttacking = false;
    }

    // private void FlipSprite()
    // {
    //     if (playerDirection == Vector2.left)
    //     {
    //         sr.flipX = true;
    //     }
    //     else if (playerDirection == Vector2.right)
    //     {
    //         sr.flipX = false;
    //     }
    // }

    public void TakeDamage(int damage)
    {
        if (isInvincible == false && !GameManager.instance.IsLevelEditorMode())
        {
            if (isHurt == false)
            {
                isHurt = true;
                playerSprite.color = Color.red;
                Invoke(nameof(EndDamageFlash), 0.2f);
            }
            health -= damage;
            UIManager.instance.UpdatePlayerStats();
        }
    }

    private void EndDamageFlash()
    {
        playerSprite.color = Color.white;
        isHurt = false;
    }

    public void FullHeal()
    {
        health = 10;
        UIManager.instance.UpdatePlayerStats();
    }

    public void AddKey()
    {
        keyCount++;
        UIManager.instance.UpdatePlayerStats();
    }

    public void RemoveKey()
    {
        keyCount--;
        UIManager.instance.UpdatePlayerStats();
    }

    public bool HasKey()
    {
        return keyCount > 0;
    }

    public void BecomeInvincible()
    {
        playerSprite.color = new Color(178.5f / 255f, 0, 178.5f / 255f);
        isInvincible = true;
        Invoke(nameof(EndInvincibility), 5f); //Calls EndInvincibility after 5 seconds
    }

    private void EndInvincibility()
    {
        playerSprite.color = Color.white;
        isInvincible = false;
    }

    //FOR UI
    public int GetPlayerHealth()
    {
        return health;
    }

    public int GetKeyCount()
    {
        return keyCount;
    }

    //FOR RESET
    public void DestroyPlayer()
    {
        Destroy(this);
    }
}
