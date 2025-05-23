using System.Collections;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float speed = 2f;
    public Transform[] patrolPoints;
    public float waitTime = 2f;

    [Header("Proximity Settings")]
    [SerializeField] private float attackRange = 1f;

    [Header("Detection Settings")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Damage Settings")]
    public int damage = 20;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Animator")]
    public Animator anim;
    public AudioClip axeSound;

    private AudioSource skeletonAudio;
    private int currentPointIndex = 0;
    private bool isWaiting = false;
    private bool isAttacking = false;
    public bool isFrozen = false; // To track if the enemy is frozen


    private void Start()
    {
        skeletonAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        FreezeHandler freezeHandler = GetComponent<FreezeHandler>();
        if (freezeHandler != null && freezeHandler.IsFrozen()) return;

        if (!isWaiting && !isAttacking)
        {
            Patrol();
        }

        cooldownTimer += Time.deltaTime;

        if (PlayerInSight() && !isAttacking && cooldownTimer >= 2.5f)
        {
            StartCoroutine(AttackPlayer());
        }
    }

    private void Patrol()
    {
        float distance = Vector2.Distance(transform.position, patrolPoints[currentPointIndex].position);

        if (distance > 0.1f) // Move toward the current patrol point
        {
            Vector3 direction = (patrolPoints[currentPointIndex].position - transform.position).normalized;
            FlipDirection(direction.x);
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
        // Check for player within attack range
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        if (playerCollider != null)
        {
            // Flip to face the player
            FlipDirection(playerCollider.transform.position.x - transform.position.x);
        }
        return playerCollider != null;
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        skeletonAudio.PlayOneShot(axeSound, 1.0f);
        anim.SetBool("moving", false); // Stop moving during attack
        anim.SetTrigger("hit");
        cooldownTimer = 0f; // Reset cooldown

        // Wait for the attack animation to reach the damage point
        yield return new WaitForSeconds(0.6f);

        // Apply damage if the player is still within attack range
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        if (playerCollider != null)
        {
            PlayerMovement player = playerCollider.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.takeDamage(damage);
            }
        }

        // Wait for the attack animation to finish before resuming patrol
        yield return new WaitForSeconds(2f);
        isAttacking = false;
    }

    private void FlipDirection(float direction)
    {
        if ((direction > 0 && transform.localScale.x < 0) || (direction < 0 && transform.localScale.x > 0))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (patrolPoints != null)
        {
            Gizmos.color = Color.green;
            foreach (Transform point in patrolPoints)
            {
                Gizmos.DrawWireSphere(point.position, 0.1f);
            }
        }

        Gizmos.color = Color.blue;
        for (int i = 0; i < patrolPoints.Length - 1; i++)
        {
            Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
        }
    }
}
