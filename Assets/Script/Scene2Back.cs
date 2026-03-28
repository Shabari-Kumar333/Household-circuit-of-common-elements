using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene2Back : MonoBehaviour
{
    public bool reverseOrder = false;

    public void GoBack()
    {
        if (reverseOrder)
        {
            ProgressManager.Instance.currentStep--;
        }

        SceneManager.LoadScene(0);
    }
}
