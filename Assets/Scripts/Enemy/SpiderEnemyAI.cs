using System.Collections;
using UnityEngine;
using Pathfinding;

public class SpiderEnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeight = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    private bool isGrounded = false;
    private Seeker seeker;
    private Rigidbody2D rb;

    // Freezing-related variables
    private bool isFrozen = false;
    private float originalSpeed;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSpeed = speed;

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        if (isFrozen) return; // Skip behavior if frozen

        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if (isFrozen) return; // Skip path updates if frozen

        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count) return;

        isGrounded = Physics2D.Raycast(transform.position, -Vector3.up, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        if (jumpEnabled && isGrounded)
        {
            if (direction.y > jumpNodeHeight)
            {
                rb.AddForce(Vector2.up * speed * jumpModifier);
            }
        }

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Freezing logic
    public void ApplyFreeze(float freezeDuration)
    {
        if (isFrozen) return; // Avoid multiple freezes

        isFrozen = true;
        speed = 0; // Stop movement

        if (anim != null)
        {
            anim.enabled = false; // Stop animations
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.cyan; // Indicate freezing visually
        }

        StartCoroutine(UnfreezeAfterDelay(freezeDuration));
    }

    private IEnumerator UnfreezeAfterDelay(float freezeDuration)
    {
        yield return new WaitForSeconds(freezeDuration);

        isFrozen = false;
        speed = originalSpeed; // Restore speed

        if (anim != null)
        {
            anim.enabled = true; // Resume animations
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white; // Reset color
        }

        Debug.Log($"{gameObject.name} is now unfrozen.");
    }
}
