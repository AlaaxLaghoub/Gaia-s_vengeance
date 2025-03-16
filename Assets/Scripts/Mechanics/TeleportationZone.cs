using System.Collections;
using UnityEngine;

public class TeleportationZone : MonoBehaviour
{
    [SerializeField] private Transform teleportDestination;
    [SerializeField] private ScreenFader screenFader;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null && teleportDestination != null)
            {
                StartCoroutine(SmoothTeleport(player));
            }
        }
    }

    private IEnumerator SmoothTeleport(PlayerMovement player)
    {
        // Disable player movement and stop Rigidbody2D velocity
        player.enabled = false;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Animator anim = player.GetComponent<Animator>();
        if (rb != null) 
        {
            rb.velocity = Vector2.zero; // Stop movement
            rb.isKinematic = true;
        } 

        if (anim != null) anim.SetFloat("MoveSpeed", 0f); // Stop animations

        // Fade out
        yield return screenFader.FadeOut();

        // Teleport the player
        player.Teleport(teleportDestination.position);

        // Fade in
        yield return screenFader.FadeIn();

        // Re-enable player movement
        player.enabled = true;
        rb.isKinematic = false;
    }
}
