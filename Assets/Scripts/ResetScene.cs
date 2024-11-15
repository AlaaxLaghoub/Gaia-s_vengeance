using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class ResetScene : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(ResetSceneWithDelay(0.7f)); // Start coroutine with 1-second delay
        }
    }

    private IEnumerator ResetSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the active scene
    }
}
