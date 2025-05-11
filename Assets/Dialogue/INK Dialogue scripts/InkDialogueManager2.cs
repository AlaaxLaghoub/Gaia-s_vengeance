using System.Collections;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class InkDialogueManager2 : MonoBehaviour
{
    private static InkDialogueManager2 instance;

    [Header("Parameters")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Ink Globals (Optional)")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Coroutine displayLineCoroutine;
    private Story currentStory;
    private PlayerMovement player;
    private Animator layoutAnimator;

    private AudioSource playerAudio;
    [SerializeField] private AudioClip blip;

    public bool DialogueIsPlaying { get; private set; } = false;
    private bool canContinueToNextLine = false;

    public static InkDialogueManager2 GetInstance() => instance;

    private void Awake()
    {
        playerAudio = GetComponent<AudioSource>();

        if (instance != null && instance != this)
        {
            Debug.LogWarning("Duplicate InkDialogueManager2 found. Destroying this instance.");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        player = FindObjectOfType<PlayerMovement>();
        if (player == null)
        {
            Debug.LogError("PlayerMovement script not found in the scene.");
        }
    }

    private void Update()
    {
        if (!DialogueIsPlaying) return;

        if (player != null)
            player.dialoguePlaying = true;

        if (canContinueToNextLine && Input.GetButtonDown("Submit"))
            ContinueStory();
    }

    public void EnterDialogueMode(TextAsset inkJSON, Animator emoteAnimator = null)
    {
        currentStory = new Story(inkJSON.text);
        layoutAnimator = emoteAnimator;

        DialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        if (player != null)
            player.dialoguePlaying = false;

        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
                StopCoroutine(displayLineCoroutine);

            string nextLine = currentStory.Continue();

            if (string.IsNullOrWhiteSpace(nextLine) && !currentStory.canContinue)
            {
                StartCoroutine(ExitDialogueMode());
            }
            else
            {
                displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
            }
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = "";
        dialogueText.maxVisibleCharacters = 0;
        continueIcon.SetActive(false);
        canContinueToNextLine = false;

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            dialogueText.maxVisibleCharacters++;

            // Optional: play typewriter SFX
            // playerAudio.PlayOneShot(Death1, 1.0f);

            yield return new WaitForSeconds(typingSpeed);
        }

        continueIcon.SetActive(true);
        canContinueToNextLine = true;
    }
}
