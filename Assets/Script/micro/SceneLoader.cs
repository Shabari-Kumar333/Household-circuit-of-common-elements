using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene List")]
    public string[] sceneNames; // Assign multiple scenes in the Inspector

    /// <summary>
    /// Loads a scene by index from the assigned list.
    /// Example: LoadAssignedScene(0) will load the first scene in the list.
    /// </summary>
    /// <param name="index">Index of the scene in the array</param>
    public void LoadAssignedScene(int index)
    {
       SceneManager.LoadScene (index);
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting application...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
