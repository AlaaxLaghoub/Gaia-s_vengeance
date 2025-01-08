using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Spells and Shooting Settings")]
    public Transform shootPointRight; // Shooting point for the right direction
    public Transform shootPointLeft; // Shooting point for the left direction
    public GameObject fireballPrefab; // Fireball prefab
    public GameObject iceShardPrefab; // Ice Shard prefab
    public float fireballSpeed = 10f; // Speed of the fireball
    public float iceShardSpeed = 7f; // Speed of the ice shard
    public float cooldownTime = 0.5f; // Cooldown time between shots

    [Header("Unlocked Spells")]
    public bool fireballUnlocked = false; // Is the fireball unlocked?
    public bool iceShardUnlocked = false; // Is the ice shard unlocked?

    [Header("Animator")]
    private Animator anim; // Player Animator for shooting animation

    [Header("Audio Clips")]
    public AudioClip fireballSound; // Sound for fireball
    public AudioClip iceShardSound; // Sound for ice shard
    private AudioSource audioSource; // Audio source for playing sounds

    private float lastShootTime; // Time when the last spell was shot

    private void Start()
    {
        anim = GetComponent<Animator>(); // Get the Animator component
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    private void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        // Prevent shooting if cooldown has not elapsed
        if (Time.time < lastShootTime + cooldownTime) return;

        if (fireballUnlocked && Input.GetKeyDown(KeyCode.Alpha1)) // Fireball
        {
            Shoot(fireballPrefab, fireballSpeed, fireballSound);
        }
        else if (iceShardUnlocked && Input.GetKeyDown(KeyCode.Alpha2)) // Ice Shard
        {
            Shoot(iceShardPrefab, iceShardSpeed, iceShardSound);
        }
    }

    private void Shoot(GameObject spellPrefab, float speed, AudioClip sound)
    {
        if (spellPrefab == null || (shootPointRight == null && shootPointLeft == null)) return;

        // Determine the direction and choose the appropriate shooting point
        Transform shootPoint = transform.localScale.x > 0 ? shootPointRight : shootPointLeft;

        // Play shooting animation
        if (anim != null)
        {
            anim.SetTrigger("shoot");
        }

        // Play shooting sound
        if (audioSource != null && sound != null)
        {
            audioSource.PlayOneShot(sound);
        }

        // Instantiate the spell prefab at the selected shoot point
        GameObject spell = Instantiate(spellPrefab, shootPoint.position, Quaternion.identity);

        // Ensure the spell's local scale is set based on the player's direction
        Vector3 spellScale = spell.transform.localScale;
        spellScale.x = transform.localScale.x > 0 ? 1 : -1; // Positive for right, negative for left
        spell.transform.localScale = spellScale;

        // Add velocity to the spell
        Rigidbody2D rb = spell.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            rb.velocity = direction * speed;
        }

        lastShootTime = Time.time; // Reset cooldown
    }

    // Method to unlock fireball
    public void UnlockFireball()
    {
        fireballUnlocked = true;
        Debug.Log("Fireball spell unlocked!");
    }

    // Method to unlock ice shard
    public void UnlockIceShard()
    {
        iceShardUnlocked = true;
        Debug.Log("Ice Shard spell unlocked!");
    }
}
