using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelSwitcher11 : MonoBehaviour
{
    public GameObject[] panels; // Assign all panels here

    // Call this from UI Button
    public void ShowPanel(int index)
    {
        // Hide all panels
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        // Show the selected panel
        panels[index].SetActive(true);
    }
    public void middlescene()
    {
        SceneManager.LoadScene(1);
    }
    public void LastScene()
    {
        SceneManager.LoadScene(0);
    }
}
            