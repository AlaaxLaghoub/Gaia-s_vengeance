using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
public class HealingPotion : MonoBehaviour
{
    [SerializeField] private int healAmount = 20; // Amount of health restored
    [SerializeField] private float hideDuration = 80.9f; // Time to hide before destroying
    private AudioSource healAudio;
    public AudioClip healSound;

    void Start()
    {
        healAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Heal(healAmount); // Heal the player
                healAudio.PlayOneShot(healSound, 1.0f);
                HidePotion(); // Hide the potion
            }
        }
    }
    private void HidePotion()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(DelayedDestroy());
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(hideDuration); // Wait for the specified duration
        Destroy(gameObject); // Destroy after delay
        print("Item destroyed");
    }


}
