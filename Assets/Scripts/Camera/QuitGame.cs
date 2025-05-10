using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Quit button clicked.");
        Application.Quit();

        // In the Unity editor, this doesn't quit the game,
        // so this line is just for testing in the editor:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
