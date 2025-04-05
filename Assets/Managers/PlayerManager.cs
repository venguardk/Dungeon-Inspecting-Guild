using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    //This script handles the player movement and stats
    //Other scripts this script interacts with: GameManager, UIManager, PlayerControlsOption
    public static PlayerManager instance;
    private PlayerActions playerInput;    //input system declaration
    Vector2 aimVect;
    private Vector2 movementInput, movementVelocity, playerDirection, inputVect;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private GameObject attackHitBox;
    [SerializeField] private float attackDuration = 0.1f, attackRange = 1f;
    [SerializeField] private int health = 10;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip attackAudioClip;
    private bool isAttacking, isInvincible, isHurt;
    private int keyCount = 0;
    private Animator animator;  // animation component

    public string deviceType;

    private void Awake()
    {

        instance = this;
        //playerInput = GetComponent<playerInput>();
        //playerInput.SwitchCurrentActionMap();

        playerInput = new PlayerActions();    // input system instance, not sure if needed
        //playerInput.QuestMode.Movement.performed += MovementPerformed;
        //playerInput.QuestMode.Attack.performed += OnAttack;
        //playerInput.QuestMode.Aim.performed += OnAim;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
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
        aimVect = Vector2.zero;
    }

    private void Update()
    {
        if (health <= 0) //If health is less than or equal to 0, go back to level editor
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

        if (GameManager.instance.IsLevelEditorMode()) //Preventing player from moving whilst in level editor mode
        {
            playerInput.QuestMode.Disable();
            playerInput.EditMode.Enable();
            return;
        }

        if (isAttacking)
        {
            movementInput = Vector2.zero; //Stops player from moving while attacking
            return;
        }

        if (PlayerControlsOption.instance.isOneHandMode)
        {
            // One hand mode: use right-click is down for movement
            if (Input.GetMouseButton(1))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0; // Set z to 0 to ignore depth
                playerDirection = (mousePosition - transform.position).normalized;
                movementInput = playerDirection;
            }
            else
            {
                movementInput = Vector2.zero;
            }
        }
        else
        {
            // Normal mode: use WASD or arrow keys for movement
            movementInput = inputVect;
        }

        if (movementInput != Vector2.zero) //Animator
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
    }

    private void FixedUpdate() //Additional checks whilst in play mode
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

    // Keven's Additions ===========================================================

    // Quest mode -----------------------------------------------
    // input movement

    public void OnMovement(InputAction.CallbackContext context)
    {
        //Debug.Log("Moving" + context.ReadValue<Vector2>());
        inputVect = context.ReadValue<Vector2>();
        deviceType = context.control.name;
        //Debug.Log("Device: " + deviceType);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && GameManager.instance.IsLevelEditorMode() == false && isAttacking == false)
        {
            //playerAnimationBehaviour.PlayAttackAnimation();
            Attack(aimVect);
            // Debug.Log("Attacking" + aimVect);
        }
    }


    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            aimVect = context.ReadValue<Vector2>();
            deviceType = context.control.name;  // name of action

            // Debug.Log("Pos:" + aimVect);
            // Debug.Log("device: " + deviceType);
        }

    }

    // Edit Mode -------------------------------------------------

    // End of Additions ===========================================================

    private void Attack(Vector2 aimVect) //Player attack
    {
        isAttacking = true;
        Vector3 direction;
        if (deviceType == "position")
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(aimVect); // using old input system

            //Vector3 mousePosition = aimVect;
            mousePosition.z = 0;

            // Debug.Log("Mouse Pos: " + mousePosition);
            // Debug.Log("Aiming" + aimVect);

            direction = (mousePosition - transform.position).normalized;
        }
        else
        {
            direction = playerDirection;
        }


        attackHitBox.transform.position = transform.position + direction * attackRange;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90;
        attackHitBox.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        attackHitBox.SetActive(true);
        playerAudioSource.clip = attackAudioClip; //Playing Attack SFX
        playerAudioSource.Play();
        Invoke(nameof(EndAttack), attackDuration); //Calls EndAttack after attackDuration seconds
    }

    private void EndAttack()
    {
        attackHitBox.SetActive(false);
        isAttacking = false;
    }

    public void TakeDamage(int damage) //Player takes damage
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

    public void FullHeal() //Reset player health to max
    {
        health = 10;
        UIManager.instance.UpdatePlayerStats();
    }

    public void AddKey() //Add a key to the player stats
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

    //FOR UI MANAGER
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
