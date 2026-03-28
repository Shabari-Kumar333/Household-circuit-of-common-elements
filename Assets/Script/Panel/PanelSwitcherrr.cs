using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelSwitcherrr : MonoBehaviour
{
    public GameObject[] panelss; // Assign all panels here

    // Call this from UI Button
    public void ShowPanel(int index)
    {
        // Hide all panels
        for (int i = 0; i < panelss.Length; i++)
        {
            panelss[i].SetActive(false);
        }

        // Show the selected panel
        panelss[index].SetActive(true);
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
            