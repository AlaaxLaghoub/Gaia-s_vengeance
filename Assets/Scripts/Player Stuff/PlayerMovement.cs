using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public bool dialoguePlaying = false;

    [Header("Movement Settings")]
    private Vector2 windForce = Vector2.zero;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float doubleJumpForce = 8f;
    [SerializeField] private float invincibleMoveSpeedFactor = 0.5f;

    [Header("Water Settings")]
    [SerializeField] private float waterMoveSpeedFactor = 0.5f; // Speed reduction in water
    [SerializeField] private float waterJumpForceFactor = 0.7f; // Jump reduction in water
    private bool isInWater = false; // Check if the player is in water

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckRadius = 0.2f;

    [Header("Health Check")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public AudioClip Death1; 

    [Header("Invincibility Settings")]
    [SerializeField] private float invincibilityDuration = 2f;
    private bool isInvincible = false;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashAcceleration = 30f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private AudioClip dashSound;

    private bool isDashing = false;
    private bool canDash = true;

    [Header("Dash Particle Effect")]
    [SerializeField] private ParticleSystem dashEffect;

    private Rigidbody2D playerRb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private int jumpCount = 0;
    private AudioSource playerAudio;
    private float originalMoveSpeed;
   

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        playerRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAudio = GetComponent<AudioSource>();
        originalMoveSpeed = moveSpeed;
        

        // Ensure the dash effect is stopped initially
        if (dashEffect != null)
        {
            dashEffect.Stop();
        }
    }

    void Update()
    {
        if (dialoguePlaying)
        {
            // Freeze all movement input
            playerRb.velocity = Vector2.zero;
            anim.SetFloat("MoveSpeed", 0);
            return;
        }
        // Debug the player's facing direction
        Debug.Log($"Player scale X: {transform.localScale.x}");
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
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround | LayerMask.GetMask("MovableBlock"));

    }
    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Calculate effective move speed (reduced by wind force)
        float effectiveMoveSpeed = moveSpeed;
        if (windForce != Vector2.zero)
        {
            effectiveMoveSpeed -= Mathf.Abs(Vector2.Dot(windForce.normalized, Vector2.right)) * windForce.magnitude;
            effectiveMoveSpeed = Mathf.Max(effectiveMoveSpeed, 1f); // Prevent movement from completely stopping
        }

        // Set velocity based on player input
        Vector2 newVelocity = new Vector2(horizontalInput * effectiveMoveSpeed, playerRb.velocity.y);
        playerRb.velocity = newVelocity;

        // Flip the player's localScale based on movement direction
        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
    }


    private void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);

        if (isGrounded && playerRb.velocity.y <= 0)
        {
            jumpCount = 0; // Reset only when player lands (optionally: also check falling velocity)
        }

        if (Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            if (jumpCount == 0)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
                Debug.Log("First jump executed");
            }
            else if (jumpCount == 1)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, doubleJumpForce);
                Debug.Log("Double jump executed");
            }

            jumpCount++;
        }

        if (Input.GetButtonUp("Jump") && playerRb.velocity.y > 0)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y * 0.5f);
        }

        Debug.Log($"Grounded: {isGrounded}, JumpCount: {jumpCount}, VelocityY: {playerRb.velocity.y}");
    }


    private void UpdateAnimation()
    {
        anim.SetFloat("MoveSpeed", Mathf.Abs(playerRb.velocity.x));
        //anim.SetBool("isGrounded", isGrounded);
    }

    private IEnumerator SmoothDashCoroutine()
    {
        isDashing = true;
        canDash = false;

        // Start particle effect
        if (dashEffect != null)
        {
            dashEffect.Play();
            playerAudio.PlayOneShot(dashSound, 1.0f);
        }

        float direction = Mathf.Sign(transform.localScale.x); // Determine direction based on player's scale (-1 for left, 1 for right)
        float originalGravity = playerRb.gravityScale;
        playerRb.gravityScale = 0; // Disable gravity during dash

        float elapsedTime = 0f;
        float currentSpeed = dashSpeed; // Start with max dash speed

        while (elapsedTime < dashDuration)
        {
            // Gradually decrease the speed for a smoother stop
            currentSpeed = Mathf.Lerp(dashSpeed, 0, elapsedTime / dashDuration);

            // Apply velocity based on the current speed
            playerRb.velocity = new Vector2(currentSpeed * direction, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Stop movement at the end of the dash
        playerRb.velocity = Vector2.zero;

        // Restore gravity
        playerRb.gravityScale = originalGravity;

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
        if (currentHealth <= 0 || isInvincible)
        {
            Debug.Log("Damage ignored. Invincible: " + isInvincible);
            return;
        }

        currentHealth -= damage;
        Debug.Log("Player took damage. Current health: " + currentHealth);
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead.");
            Die();
        }
        else
        {
            StartCoroutine(HurtAnimationCoroutine());
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private void Die()
    {
        anim.SetTrigger("Die");
        playerAudio.PlayOneShot(Death1, 1.0f);
        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(0.8f); // Wait for the death animation
        CheckpointController.instance.Respawn(); // Respawn the player
    }
    public void ResetAnimationState()
    {
        anim.ResetTrigger("Die"); // Reset the death trigger
        anim.SetBool("Respawn", true); // Set respawn state to true
        anim.ResetTrigger("Hurt"); // Reset Hurt trigger

        // Optional: Reset Respawn after some time
        StartCoroutine(ResetRespawnBool());
        anim.Play("Idle"); // Force Idle state

    }

    private IEnumerator ResetRespawnBool()
    {
        yield return new WaitForSeconds(10f); // Wait for the death animation
        anim.SetBool("Respawn", false);
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
        if (other.CompareTag("Water"))
        {
            EnterWater();
        }

        if (other.CompareTag("Enemy"))
        {
            takeDamage(20);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            ExitWater();
        }
    }
    private void EnterWater()
    {
        isInWater = true;
        moveSpeed *= waterMoveSpeedFactor; // Reduce speed in water
        Debug.Log("Entered water. Speed reduced.");
    }

    private void ExitWater()
    {
        isInWater = false;
        moveSpeed = originalMoveSpeed; // Restore speed
        Debug.Log("Exited water. Speed restored.");
    }

    public void Teleport(Vector3 targetPosition)
    {
        transform.position = targetPosition;
        Debug.Log("Teleported to " + targetPosition);
    }

    //Smooth Teleportation 
    public IEnumerator SmoothMove(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }


    // Healing Potion
    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth); // Ensure health doesn't exceed maxHealth
        healthBar.SetHealth(currentHealth);
    }
    public void EnterWindZone(Vector2 force)
    {
        windForce = force;
        Debug.Log("Entered wind zone. Wind force: " + force);
    }

    public void ExitWindZone()
    {
        windForce = Vector2.zero;
        Debug.Log("Exited wind zone. Wind force stopped.");
    }
    private void ApplyWindForce()
    {
        if (windForce != Vector2.zero)
        {
            playerRb.AddForce(windForce, ForceMode2D.Force);
        }
    }
}
