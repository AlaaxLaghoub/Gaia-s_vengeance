using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float doubleJumpForce = 8f;
    [SerializeField] private float invincibleMoveSpeedFactor = 0.5f;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckRadius = 0.2f;

    [Header("Health Check")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    [Header("Invincibility Settings")]
    [SerializeField] private float invincibilityDuration = 2f;
    private bool isInvincible = false;

    private Rigidbody2D playerRb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private int jumpCount = 0;

    // To store the original move speed
    private float originalMoveSpeed;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        playerRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Store the original move speed
        originalMoveSpeed = moveSpeed;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        UpdateAnimation();
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 newVelocity = new Vector2(horizontalInput * moveSpeed, playerRb.velocity.y);
        playerRb.velocity = newVelocity;

        if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);

        if (isGrounded)
        {
            jumpCount = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || jumpCount < 1)
            {
                float currentJumpForce = (jumpCount == 0) ? jumpForce : doubleJumpForce;
                playerRb.velocity = new Vector2(playerRb.velocity.x, currentJumpForce);
                jumpCount++;
            }
        }

        if (Input.GetButtonUp("Jump") && playerRb.velocity.y > 0)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y * 0.5f);
        }
    }

    private void UpdateAnimation()
    {
        anim.SetFloat("MoveSpeed", Mathf.Abs(playerRb.velocity.x));
        anim.SetBool("isGrounded", isGrounded);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }

    public void takeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);

            // Trigger the hurt animation when the player takes damage
            StartCoroutine(HurtAnimationCoroutine());

            // After hurt animation, check for death
            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(InvincibilityCoroutine());
            }
        }
    }

    private void Die()
    {
        // Play the death animation
        anim.SetTrigger("Die");

        // Start the scene reload after a delay to allow the death animation to play
        StartCoroutine(ReloadScene());
    }

    private IEnumerator ReloadScene()
    {
        // Wait for the duration of the death animation to play (adjust time for your animation length)
        yield return new WaitForSeconds(0.8f);
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator HurtAnimationCoroutine()
    {
        // Play the hurt animation once when the player takes damage
        anim.SetTrigger("Hurt");

        // Wait for the hurt animation to finish before continuing
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        // Reduce player speed during invincibility
        moveSpeed = originalMoveSpeed * invincibleMoveSpeedFactor;

        // Wait for the invincibility duration
        yield return new WaitForSeconds(invincibilityDuration);

        // Restore original player speed after invincibility ends
        moveSpeed = originalMoveSpeed;
        isInvincible = false;
    }

    // Detect collision with any hazard
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object colliding with the player has the "Hazard" tag
        if (other.CompareTag("Hazard"))
        {
            // Apply damage when the player collides with a hazard
            takeDamage(20);  // Adjust damage value as needed
        }
    }
}
