using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropdownStepValidator : MonoBehaviour
{
    [System.Serializable]
    public class DropdownAnswer
    {
        public TMP_Dropdown dropdown;
        public string correctOptionText;
    }

    public List<DropdownAnswer> dropdownAnswers;
    private bool stepCompleted = false;

    void OnEnable() // better than Start for slides
    {
        foreach (var item in dropdownAnswers)
        {
            item.dropdown.onValueChanged.RemoveAllListeners();
            item.dropdown.onValueChanged.AddListener(delegate {
                Debug.Log("Dropdown changed: " + item.dropdown.name);
                ValidateAllDropdowns();
            });
        }
    }

    void ValidateAllDropdowns()
    {
        if (stepCompleted) return;

        if (StepFlowController.Instance == null)
        {
            Debug.LogError("StepFlowController Instance is NULL");
            return;
        }

        foreach (var item in dropdownAnswers)
        {
            string selectedText = item.dropdown.options[item.dropdown.value].text;

            Debug.Log("Selected: [" + selectedText +
                      "] Expected: [" + item.correctOptionText + "]");

            if (selectedText != item.correctOptionText)
                return;
        }

        Debug.Log("ALL DROPDOWNS CORRECT -> STEP COMPLETED");
        stepCompleted = true;
        StepFlowController.Instance.MarkCurrentStepCompleted();
    }
}
