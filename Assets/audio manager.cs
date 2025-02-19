using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour
{
    [Header("Audio source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SPXSource;

    [Header("Audio clip")]
    public AudioClip bg;
    public AudioClip btnClick;
}
