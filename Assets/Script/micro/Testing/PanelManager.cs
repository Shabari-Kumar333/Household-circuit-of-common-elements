using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [Tooltip("Drag and drop the UI panels you want to manage.")]
    public GameObject[] panels;

    private GameObject currentActivePanel;

    void Start()
    {
        // Initially, activate the first panel and deactivate the rest.
        if (panels.Length > 0)
        {
            foreach (var panel in panels)
            {
                panel.SetActive(false);
            }
            panels[0].SetActive(true);
            currentActivePanel = panels[0];
        }
        else
        {
            Debug.LogWarning("No panels assigned to the PanelManager.");
        }
    }

    // This is the function you'll call from your UI buttons.
    public void SwitchToPanel(GameObject panelToActivate)
    {
        if (panelToActivate == null)
        {
            Debug.LogWarning("The panel to activate is null.");
            return;
        }

        // Deactivate the currently active panel
        if (currentActivePanel != null)
        {
            currentActivePanel.SetActive(false);
        }

        // Activate the new panel
        panelToActivate.SetActive(true);
        currentActivePanel = panelToActivate;
    }
}