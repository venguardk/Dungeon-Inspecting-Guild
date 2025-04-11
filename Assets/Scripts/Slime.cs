using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // For using coroutines

public class Slime : MonoBehaviour
{
    // This script is used to control the Slime prefab, specifically its movement and its interactions with the player
    //The State Machine used here was inspired by Code Class - Build your own State Machines!: https://www.youtube.com/watch?v=-jkT4oFi1vk

    //SLIME STATE MACHINE VARIABLES
    enum SlimeState { LevelEditor, Idle, Chase, Attack };
    private SlimeState currentState;
    private bool currentStateCompleted;

    //SLIME VARIABLES
    [SerializeField] private float chaseRange = 10f, attackRange = 1.5f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float attackDelay = 0.5f, attackDuration = 0.5f, attackCooldown = 1f;
    [SerializeField] private GameObject attackHitBox;
    [SerializeField] private AudioSource attackAudio;
    private Transform player, slimeTransform;
    private Rigidbody2D rb;
    private bool isAttacking = false;
    private bool playerInRadarRange = false;
    private bool playerInAttackRange = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player object in the scene
        rb = GetComponent<Rigidbody2D>();
        slimeTransform = transform;
        attackHitBox.SetActive(false);
        currentStateCompleted = true; // Set the current state to completed
    }

    private void Update()
    {
        if (currentStateCompleted) // If the current state is completed, determine the new state
        {
            DetermineNewState();
        }
        UpdateState(); // Update the current state
    }

    private void FixedUpdate()
    {
        if (player == null) // If the player is not found, return
        {
            return;
        }

        DeterminePlayerInRadarRange(); // Check if the player is in radar range
        DeterminePlayerInAttackRange(); // Check if the player is in attack range
    }

    //STATE MACHINE FUNCTIONS
    private void DetermineNewState()
    {
        currentStateCompleted = false; // Set the current state to not completed
        if (GameManager.instance.IsLevelEditorMode()) // If in level editor mode, stop chasing the player and do nothing
        {
            currentState = SlimeState.LevelEditor;
            StartLevelEditorState();
        }
        else if (playerInAttackRange)
        {
            currentState = SlimeState.Attack;
            StartAttackState();
        }
        else if (playerInRadarRange)
        {
            currentState = SlimeState.Chase;
            StartChaseState();
        }
        else
        {
            currentState = SlimeState.Idle;
            StartIdleState();
        }
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case SlimeState.LevelEditor:
                UpdateLevelEditorState();
                break;
            case SlimeState.Idle:
                UpdateIdleState();
                break;
            case SlimeState.Chase:
                UpdateChaseState();
                break;
            case SlimeState.Attack:
                UpdateAttackState();
                break;
        }
    }

    //INDIVIDUAL STATE FUNCTIONS
    private void StartLevelEditorState()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        attackHitBox.SetActive(false);
        StopChasingPlayer();
    }

    private void UpdateLevelEditorState()
    {
        if (GameManager.instance.IsLevelEditorMode() == false)
        {
            currentStateCompleted = true; // Set the current state to completed, so it can transition to the next state
        }
    }

    private void StartIdleState()
    {
        // <Play Idle Animation in direction>
        StopChasingPlayer();
    }

    private void UpdateIdleState()
    {
        if (playerInRadarRange == true || playerInAttackRange == true || GameManager.instance.IsLevelEditorMode() == true)
        {
            currentStateCompleted = true;
        }
    }

    private void StartChaseState()
    {
        // <Play Chase Animation in direction of player>
    }

    private void UpdateChaseState()
    {
        if (playerInAttackRange == true || playerInRadarRange == false)
        {
            currentStateCompleted = true;
            return;
        }
        ChasePlayer();
    }

    private void StartAttackState()
    {
        // <Play Attack Animation in direction of player>
        isAttacking = true; // Set the attacking state to true
        StartCoroutine(AttackPlayer()); // Start the attack coroutine
    }

    private void UpdateAttackState()
    {
        // Update the attack state
        if (isAttacking == false)
        {
            currentStateCompleted = true;
        }
    }

    //HELPER FUNCTIONS
    private void DeterminePlayerInRadarRange()
    {
        Vector2 slimePos = slimeTransform.position;
        Vector2 playerPos = player.position;
        float distanceToPlayer = Vector2.Distance(slimePos, playerPos); //Acquiring distance to player
        playerInRadarRange = distanceToPlayer <= chaseRange; //Checking if player is within chase range
    }

    private void DeterminePlayerInAttackRange()
    {
        Vector2 slimePos = slimeTransform.position;
        Vector2 playerPos = player.position;
        float distanceToPlayer = Vector2.Distance(slimePos, playerPos); //Acquiring distance to player
        playerInAttackRange = distanceToPlayer <= attackRange; //Checking if player is within attack range
    }

    void ChasePlayer()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        rb.linearVelocity = directionToPlayer * moveSpeed; // Move towards the player
    }

    void StopChasingPlayer()
    {
        rb.linearVelocity = Vector2.zero;
    }

    private IEnumerator AttackPlayer()
    {
        yield return new WaitForSeconds(attackDelay); // Wait for the attack delay

        StopChasingPlayer(); // Stop chasing the player

        if (player == null) // If the player is not found, return
        {
            yield break;
        }

        // Activate the attack hitbox in the direction of the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        attackHitBox.transform.position = transform.position + directionToPlayer * 1f;
        attackHitBox.SetActive(true);
        attackAudio.Play(); // TODO: Consider adjusting this to not play is enemy dies during attack coroutine

        yield return new WaitForSeconds(attackDuration); // Wait for the attack duration
        attackHitBox.SetActive(false); // Deactivate the attack hitbox

        yield return new WaitForSeconds(attackCooldown); // Wait for the attack cooldown
        isAttacking = false; // Set the attacking state to false
    }
}
