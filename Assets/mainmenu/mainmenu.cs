using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class mainmenu : MonoBehaviour
{
    [Header("volume setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumSlider = null;

    [Header("Confirmation prompt")]
    [SerializeField] private GameObject confirmationPrompt = null;

    public void playGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void quit()
    {
        Application.Quit();

    }
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");

    }
    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("master volume", AudioListener.volume);
        //show a prompt
        StartCoroutine(ConfirmationBox()); 
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }


}
