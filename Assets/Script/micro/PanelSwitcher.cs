using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelSwitcher : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject[] uiPanels;

    public GameObject[] panels;

    public PanelSwitcher(GameObject[] panels)
    {
        this.panels = panels;
    }

    public TextMeshProUGUI[] pageIndicatorTexts;

    [Header("Objects to Disable on Specific Panels")]
    public GameObject[] objectsToDisable;
    public int[] disableOnPanelIndices;

    private List<int> navigablePanels = new List<int>();
    private int currentPage = 0;

    private void Start()
    {
        // Only include panels that have page indicators
        for (int i = 0; i < uiPanels.Length; i++)
        {
            if (i < pageIndicatorTexts.Length)
            {
                navigablePanels.Add(i);
            }
        }

        if (navigablePanels.Count > 0)
        {
            SwitchToPanel(0); // Show first panel
        }
    }

    public void SwitchToPanel(int navIndex)
    {
        if (navIndex < 0 || navIndex >= navigablePanels.Count)
        {
            Debug.LogError($"Panel navigation index out of bounds! Index: {navIndex}. Total navigable panels: {navigablePanels.Count}");
            return;
        }

        int panelIndex = navigablePanels[navIndex];

        // Activate only the selected panel
        for (int i = 0; i < uiPanels.Length; i++)
        {
            uiPanels[i].SetActive(i == panelIndex);
        }

        currentPage = navIndex;
        UpdatePageIndicators();
        HandleObjectDisabling(panelIndex);
    }

    public void UpdatePageIndicators()
    {
        if (pageIndicatorTexts != null && navigablePanels.Count > 0)
        {
            string pageText = $"{currentPage + 1} / {navigablePanels.Count}";

            foreach (var tmp in pageIndicatorTexts)
            {
                if (tmp != null)
                {
                    tmp.text = pageText;
                }
            }
        }
    }

    private void HandleObjectDisabling(int panelIndex)
    {
        if (objectsToDisable == null || disableOnPanelIndices == null) return;

        for (int i = 0; i < objectsToDisable.Length; i++)
        {
            if (i < disableOnPanelIndices.Length)
            {
                objectsToDisable[i].SetActive(panelIndex != disableOnPanelIndices[i]);
            }
        }
    }
}
