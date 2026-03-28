using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepFlowController : MonoBehaviour
{
    public static StepFlowController Instance;

    [Header("Logical Steps (just for count)")]
    public List<string> steps;
    // This list is ONLY for size. Names are optional.

    [Header("All Next Buttons in scene")]
    public List<Button> nextButtons;

    [Header("All Back Buttons in scene")]
    public List<Button> backButtons;

    private int currentIndex = 0;
    private bool[] completedSteps;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        completedSteps = new bool[steps.Count];

        // Initial state
        SetNextButtons(false);   // locked
        SetBackButtons(false);   // at first step, can't go back
    }

    public void Next()
    {
        if (!completedSteps[currentIndex]) return;

        if (currentIndex < steps.Count - 1)
        {
            currentIndex++;
            UpdateButtons();
        }
    }

    public void Back()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateButtons();
        }
    }

    // Call this from ANY script when step is done
    public void MarkCurrentStepCompleted()
    {
        completedSteps[currentIndex] = true;
        SetNextButtons(true);
    }

    void UpdateButtons()
    {
        // Next allowed only if current step completed
        SetNextButtons(completedSteps[currentIndex]);

        // Back allowed only if not first step
        SetBackButtons(currentIndex > 0);
    }

    void SetNextButtons(bool value)
    {
        foreach (var btn in nextButtons)
            btn.interactable = value;
    }

    void SetBackButtons(bool value)
    {
        foreach (var btn in backButtons)
            btn.interactable = value;
    }

    // Optional helpers
    public int GetCurrentStepIndex()
    {
        return currentIndex;
    }

    public bool IsCurrentStepCompleted()
    {
        return completedSteps[currentIndex];
    }
}
