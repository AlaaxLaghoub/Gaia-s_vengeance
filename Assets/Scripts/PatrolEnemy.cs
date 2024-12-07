using System.Collections;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float speed = 2f;
    public Transform[] patrolPoints;
    public float waitTime = 2f;

    [Header("Proximity Settings")]
    [SerializeField] private float reactRange = 3f;
    [SerializeField] private float attackRange = 1f;

    [Header("Detection Settings")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

    [Header("Damage Settings")]
    public int damage = 20;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Animator")]
    public Animator anim;

    private int currentPointIndex = 0;
    private bool isWaiting = false;
    private bool canMove = true;

    // New State Variables
    private bool hasReacted = false;

    private void Update()
    {
        // Patrol movement if the enemy hasn't reacted
        if (canMove && !hasReacted)
        {
            Patrol();
        }

        // Update cooldown timer
        cooldownTimer += Time.deltaTime;

        // Detect the player
        if (PlayerInSight())
        {
            float playerDistance = Vector2.Distance(transform.position, GetPlayerPosition());

            // React once when the player enters react range
            if (playerDistance <= reactRange && !hasReacted)
            {
                ReactToPlayer();
            }
            // Attack if the player is within attack range and cooldown is complete
            else if (playerDistance <= attackRange && cooldownTimer >= 1f)
            {
                AttackPlayer();
            }
        }
    }

    private void Patrol()
    {
        float distance = Vector2.Distance(transform.position, patrolPoints[currentPointIndex].position);

        if (distance > 0.1f) // Move toward the current patrol point
        {
            Vector3 direction = (patrolPoints[currentPointIndex].position - transform.position).normalized;
            transform.localScale = new Vector3(direction.x < 0 ? -1 : 1, 1, 1); // Flip sprite based on direction
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPointIndex].position, speed * Time.deltaTime);
            anim.SetBool("moving", true);
        }
        else if (!isWaiting) // Arrived at patrol point, start waiting
        {
            isWaiting = true;
            anim.SetBool("moving", false);
            StartCoroutine(WaitBeforeMoving());
        }
    }

    private IEnumerator WaitBeforeMoving()
    {
        yield return new WaitForSeconds(waitTime);
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length; // Loop through patrol points
        isWaiting = false;
    }

    private bool PlayerInSight()
    {
        // Define the detection area
        Vector2 boxCenter = boxCollider.bounds.center + transform.right * reactRange * transform.localScale.x;
        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x * reactRange, boxCollider.bounds.size.y);

        // Check for player in the detection area
        RaycastHit2D hit = Physics2D.BoxCast(boxCenter, boxSize, 0f, Vector2.zero, 0f, playerLayer);

        return hit.collider != null;
    }

    private void ReactToPlayer()
    {
        hasReacted = true; // React only once
        canMove = false;   // Stop patrolling
        anim.SetTrigger("react");
        Debug.Log("Reacting to player proximity.");
    }

    private void AttackPlayer()
    {
        anim.SetTrigger("hit");
        cooldownTimer = 0f; // Reset cooldown
        Debug.Log("Attacking player.");

        // Check if the player is within attack range
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        if (playerCollider != null)
        {
            PlayerMovement player = playerCollider.GetComponent<PlayerMovement>();
            if (player != null)
            {
                Debug.Log("Damaging player...");
                player.takeDamage(damage);
            }
        }
    }

    private Vector3 GetPlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player not found. Ensure the Player GameObject is tagged 'Player'.");
            return Vector3.zero;
        }
        return player.transform.position;
    }

    private void OnDrawGizmos()
    {
        // Visualize detection and attack ranges
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, reactRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Visualize detection box
        if (boxCollider != null)
        {
            Gizmos.color = Color.green;
            Vector2 boxCenter = boxCollider.bounds.center + transform.right * reactRange * transform.localScale.x;
            Vector2 boxSize = new Vector2(boxCollider.bounds.size.x * reactRange, boxCollider.bounds.size.y);
            Gizmos.DrawWireCube(boxCenter, boxSize);
        }
    }
}
