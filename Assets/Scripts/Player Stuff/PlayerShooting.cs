using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    [Header("Spells and Shooting Settings")]
    public Transform shootPointRight;
    public Transform shootPointLeft;
    public GameObject fireballPrefab;
    public GameObject iceShardPrefab;
    public float fireballSpeed = 10f;
    public float iceShardSpeed = 7f;
    public float cooldownTime = 0.6f;
    public AudioClip fireballSound;

    [Header("Unlocked Spells")]
    public bool fireballUnlocked = false;
    public bool iceShardUnlocked = false;

    [Header("Potion Display Parent")]
    public GameObject potionDisplay; // Parent UI container
    public GameObject firePotionImage;
    public GameObject icePotionImage;
    public TextMeshProUGUI firePotionText;
    public TextMeshProUGUI icePotionText;

    [Header("UI Display Duration")]
    public float displayDuration = 2f; // Time UI stays visible after shooting

    private Animator anim;
    private float lastShootTime;
    private Coroutine hideUICoroutine;
    private AudioSource playerAudio;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        InventoryUI inventory = FindObjectOfType<InventoryUI>();
        if (inventory != null)
        {
            foreach (var item in inventory.inventoryItems)
            {
                if (item.itemName == "Fire Potion") fireballUnlocked = true;
                if (item.itemName == "Ice Potion") iceShardUnlocked = true;
            }
        }

        HidePotionDisplay();
    }

    private void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (Time.time < lastShootTime + cooldownTime) return;

        bool isShooting = false;

        if (fireballUnlocked && Input.GetKey(KeyCode.Alpha1))
        {
            ShootSpell(fireballPrefab, fireballSpeed, firePotionText, firePotionImage, "Fireball Activated!");
            playerAudio.PlayOneShot(fireballSound, 1.0f);
            isShooting = true;
        }
        else if (iceShardUnlocked && Input.GetKey(KeyCode.Alpha2))
        {
            ShootSpell(iceShardPrefab, iceShardSpeed, icePotionText, icePotionImage, "Ice Shard Activated!");
            isShooting = true;
        }

        if (!isShooting)
        {
            // Start hide UI coroutine if not already running
            if (hideUICoroutine == null)
            {
                hideUICoroutine = StartCoroutine(HidePotionDisplayAfterDelay());
            }
        }
        else
        {
            // Reset hide timer if player shoots again
            if (hideUICoroutine != null)
            {
                StopCoroutine(hideUICoroutine);
                hideUICoroutine = null;
            }
        }
    }

    private void ShootSpell(GameObject spellPrefab, float speed,
        TextMeshProUGUI textToShow, GameObject imageToShow, string message)
    {
        if (spellPrefab == null || (shootPointRight == null && shootPointLeft == null))
        {
            Debug.LogError("Spell Prefab or Shoot Point is missing!");
            return;
        }

        Transform shootPoint = transform.localScale.x > 0 ? shootPointRight : shootPointLeft;

        if (anim != null) anim.SetTrigger("shoot");

        ShowPotionDisplay(textToShow, imageToShow, message);

        GameObject spell = Instantiate(spellPrefab, shootPoint.position, Quaternion.identity);
        if (spell == null)
        {
            Debug.LogError("Failed to instantiate spell!");
            return;
        }

        Rigidbody2D rb = spell.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            rb.velocity = direction * speed;
        }

        lastShootTime = Time.time;
    }

    private void ShowPotionDisplay(TextMeshProUGUI text, GameObject image, string message)
    {
        potionDisplay.SetActive(true);

        // Reset all UI elements
        firePotionImage.SetActive(false);
        icePotionImage.SetActive(false);
        firePotionText.gameObject.SetActive(false);
        icePotionText.gameObject.SetActive(false);

        // Activate only the relevant UI
        text.gameObject.SetActive(true);
        text.text = message;
        text.alpha = 1f;
        image.SetActive(true);
    }

    private IEnumerator HidePotionDisplayAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        HidePotionDisplay();
        hideUICoroutine = null; // Reset coroutine reference
    }

    private void HidePotionDisplay()
    {
        potionDisplay.SetActive(false);
        firePotionImage.SetActive(false);
        icePotionImage.SetActive(false);
        firePotionText.gameObject.SetActive(false);
        icePotionText.gameObject.SetActive(false);
    }

    public void UnlockFireball()
    {
        fireballUnlocked = true;
        Debug.Log("Fireball spell unlocked!");
    }

    public void UnlockIceShard()
    {
        iceShardUnlocked = true;
        Debug.Log("Ice Shard spell unlocked!");
    }
}
