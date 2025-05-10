using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainscene : MonoBehaviour
{
    [SerializeField] private ScreenFader fadeScreen; // Drag and drop or auto-find
    [SerializeField] private string sceneToLoad = "mainmenu";

    private bool isTransitioning = false;

    private void Awake()
    {
        if (fadeScreen == null)
        {
            fadeScreen = FindObjectOfType<ScreenFader>();
            if (fadeScreen == null)
            {
                Debug.LogError("ScreenFader not found in the scene.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTransitioning && other.CompareTag("Player"))
        {
            isTransitioning = true;
            StartCoroutine(TransitionToScene());
        }
    }

    private IEnumerator TransitionToScene()
    {
        yield return StartCoroutine(fadeScreen.FadeOut());
        SceneManager.LoadScene(sceneToLoad);
    }
}
