using UnityEngine;

public class InkDialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Emote Animator")]
    [SerializeField] private Animator emoteAnimator;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange = false;
    private bool dialogueStarted = false;
    private BoxCollider2D triggerCollider;

    private void Awake()
    {
        visualCue.SetActive(false);
        triggerCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (playerInRange && !InkDialogueManager.GetInstance().DialogueIsPlaying)
        {
            visualCue.SetActive(true);

            if (Input.GetKeyDown(KeyCode.I) && !dialogueStarted)
            {
                InkDialogueManager.GetInstance().EnterDialogueMode(inkJSON, emoteAnimator);
                dialogueStarted = true;
                triggerCollider.enabled = false;
                visualCue.SetActive(false);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
