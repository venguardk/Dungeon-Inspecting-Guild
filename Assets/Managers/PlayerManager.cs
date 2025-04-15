using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    //This script handles the player movement and stats
    //Other scripts this script interacts with: GameManager, UIManager, PlayerControlsOption
    //The State Machine used here was inspired by Code Class - Build your own State Machines!: https://www.youtube.com/watch?v=-jkT4oFi1vk
    public static PlayerManager instance;

    //PLAYER STATE MACHINE VARIABLES
    enum PlayerState { LevelEditor, Idle, Walking, Attacking } //Player states
    private PlayerState currentState; //Current state of the player
    private bool currentStateCompleted; //Boolean value to determine when to start new state

    private PlayerActions playerInput;    //input system declaration
    Vector2 aimVect;
    private Vector2 movementInput, movementVelocity, inputVect, playerDirection;

    //PLAYER VARIABLES
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
    public Vector2 initialSpawn;
    public bool canMove = true;

    private void Awake()
    {
        instance = this;

        playerInput = new PlayerActions();    // input system instance, not sure if needed
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
        currentStateCompleted = true; //Set to true so that the state machine can start
        initialSpawn = transform.position;
    }

    private void Update()
    {
        if (GameManager.instance.IsLevelEditorMode() == false) //If not in level editor mode...
        {
            if (PlayerControlsOption.instance.isOneHandMode) //If Limited Dexterity Mode is active...
            {
                if (Input.GetButton("Fire2")) //If right mouse button is down
                {
                    //Calculate the direction to the mouse position and move player towards it
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 0; // Set z to 0 to ignore depth
                    Vector3 direction = mousePosition - transform.position;

                    if (direction.magnitude > 0.5f) //Deadzone: If player is within 0.5f of mouse, do not move; This prevents jittering
                    {
                        playerDirection = direction.normalized;
                        movementInput = playerDirection;
                    }
                    else
                    {
                        movementInput = Vector2.zero;
                    }
                }
                else
                {
                    movementInput = Vector2.zero;
                }
            }
            else
            {
                movementInput = inputVect.normalized; //Get the movement input from the player
            }

            //Disable player movement. This is for when the player falls into a gap
            if (!canMove)
            {
                movementInput = Vector2.zero;
            }
        }

        if (currentStateCompleted == true) //If the current state is completed, determine the next state
        {
            DetermineNewState();
        }
        UpdateState(); //Call to respective state's update function
    }

    //STATE MACHINE FUNCTIONS
    private void DetermineNewState() //Determine state that player should access
    {
        currentStateCompleted = false;
        if (GameManager.instance.IsLevelEditorMode())
        {
            currentState = PlayerState.LevelEditor;
            StartLevelEditorState();
        }
        else if (isAttacking)
        {
            currentState = PlayerState.Attacking;
            StartAttackingState();
        }
        else if (movementInput != Vector2.zero)
        {
            currentState = PlayerState.Walking;
            StartWalkingState();
        }
        else
        {
            currentState = PlayerState.Idle;
            StartIdleState();
        }
    }

    private void UpdateState() //Update player based on current state by calling its respective function
    {
        switch (currentState)
        {
            case PlayerState.LevelEditor:
                UpdateLevelEditorState();
                break;
            case PlayerState.Idle:
                UpdateIdleState();
                break;
            case PlayerState.Walking:
                UpdateWalkingState();
                break;
            case PlayerState.Attacking:
                UpdateAttackingState();
                break;
        }
    }

    //INDIVIDUAL STATE FUNCTIONS
    private void StartLevelEditorState()
    {
        rb.linearVelocity = Vector2.zero;
        //<Reset player sprite to default HERE>
    }

    private void UpdateLevelEditorState()
    {
        if (GameManager.instance.IsLevelEditorMode() == false)
        {
            currentStateCompleted = true;
        }
    }

    private void StartIdleState()
    {
        //<Play idle animation of current direction>
        rb.linearVelocity = Vector2.zero;
    }

    private void UpdateIdleState()
    {
        if (movementInput != Vector2.zero || isAttacking || GameManager.instance.IsLevelEditorMode())
        {
            currentStateCompleted = true;
        }
    }

    private void StartWalkingState()
    {
        //<Play walking animation of current direction>
    }

    private void UpdateWalkingState()
    {
        if (movementInput == Vector2.zero || isAttacking || GameManager.instance.IsLevelEditorMode())
        {
            rb.linearVelocity = Vector2.zero;
            currentStateCompleted = true;
            return;
        }
        playerDirection = movementInput;
        movementVelocity = movementInput * moveSpeed;
        rb.linearVelocity = movementVelocity;
        //<Update animation based on direction>
    }

    private void StartAttackingState()
    {
        rb.linearVelocity = Vector2.zero;
        //<Play attack animation of mouse direction>
    }

    private void UpdateAttackingState()
    {
        if (isAttacking == false || GameManager.instance.IsLevelEditorMode())
        {
            currentStateCompleted = true;
        }
    }

    //PLAYER FUNCTIONS
    private void Attack(Vector2 aimVect) //Player attack
    {
        isAttacking = true;
        Vector3 direction;
        if (deviceType == "position")
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(aimVect); // using old input system

            mousePosition.z = 0;

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

            if (health <= 0) //If health is less than or equal to 0, go back to level editor
            {
                FullHeal();
                keyCount = 0;
                UIManager.instance.SwitchToLevelEditor();
                UIManager.instance.ShowLossScreen();
            }
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
}
