using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void quit()
    {
        Application.Quit();

    }
}