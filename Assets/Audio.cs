using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [Header("Audio sources")]
    [SerializeField] private AudioSource audioSource;

    [Header("Audio clips")]
    public AudioClip dashSound;
    public AudioClip deathSound;
    public AudioClip deathSound2;
    public AudioClip skeletonDeathSound;
    

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component if not assigned
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
