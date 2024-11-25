using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        // Fade out
        yield return screenFader.FadeOut();

        // Teleport the player
        player.Teleport(teleportDestination.position);

        // Fade in
        yield return screenFader.FadeIn();
    }
}
