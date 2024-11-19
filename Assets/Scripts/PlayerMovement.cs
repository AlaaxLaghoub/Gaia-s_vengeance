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

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float dashAcceleration = 30f;
    [SerializeField] private float dashCooldown = 1f;
    private bool isDashing = false;
    private bool canDash = true;

    [Header("Dash Particle Effect")]
    [SerializeField] private ParticleSystem dashEffect;

    private Rigidbody2D playerRb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private int jumpCount = 0;

    private float originalMoveSpeed;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        playerRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalMoveSpeed = moveSpeed;

        // Ensure the dash effect is stopped initially
        if (dashEffect != null)
        {
            dashEffect.Stop();
        }
    }

    void Update()
    {
        if (!isDashing)
        {
            HandleMovement();
            HandleJump();
        }
        UpdateAnimation();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            StartCoroutine(SmoothDashCoroutine());
        }
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

    private IEnumerator SmoothDashCoroutine()
    {
        isDashing = true;
        canDash = false;

        // Start particle effect
        if (dashEffect != null)
        {
            dashEffect.Play();
        }

        float direction = spriteRenderer.flipX ? -1f : 1f;
        float targetSpeed = dashSpeed * direction;
        float originalGravity = playerRb.gravityScale;
        playerRb.gravityScale = 0; // Disable gravity for smooth horizontal motion

        // Gradually accelerate to dash speed
        while (Mathf.Abs(playerRb.velocity.x) < Mathf.Abs(targetSpeed))
        {
            playerRb.velocity = new Vector2(Mathf.MoveTowards(playerRb.velocity.x, targetSpeed, dashAcceleration * Time.deltaTime), 0);
            yield return null;
        }

        // Maintain dash speed for dash duration
        yield return new WaitForSeconds(dashDuration);

        // Gradually decelerate back to zero
        while (Mathf.Abs(playerRb.velocity.x) > 0)
        {
            playerRb.velocity = new Vector2(Mathf.MoveTowards(playerRb.velocity.x, 0, dashAcceleration * Time.deltaTime), 0);
            yield return null;
        }

        playerRb.gravityScale = originalGravity; // Restore gravity
        isDashing = false;

        // Stop particle effect
        if (dashEffect != null)
        {
            dashEffect.Stop();
        }

        // Wait for cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
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

            StartCoroutine(HurtAnimationCoroutine());

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
        anim.SetTrigger("Die");
        StartCoroutine(ReloadScene());
    }

    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator HurtAnimationCoroutine()
    {
        anim.SetTrigger("Hurt");
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        moveSpeed = originalMoveSpeed * invincibleMoveSpeedFactor;

        yield return new WaitForSeconds(invincibilityDuration);

        moveSpeed = originalMoveSpeed;
        isInvincible = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard"))
        {
            takeDamage(20);
        }
    }
}
