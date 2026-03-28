using UnityEngine;

public class StepManager : MonoBehaviour
{
    [System.Serializable]
    public class Step
    {
        public GameObject[] objects;   // objects for this step
    }

    public Step[] steps;
    private int currentStep = 0;

    void Start()
    {
        ShowStep(0);
    }

    public void Next()
    {
        if (currentStep < steps.Length - 1)
        {
            currentStep++;
            ShowStep(currentStep);
        }
    }

    public void Back()
    {
        if (currentStep > 0)
        {
            currentStep--;
            ShowStep(currentStep);
        }
    }

    void ShowStep(int stepIndex)
    {
        // Hide all
        for (int i = 0; i < steps.Length; i++)
        {
            foreach (GameObject obj in steps[i].objects)
                obj.SetActive(false);
        }

        // Show current
        foreach (GameObject obj in steps[stepIndex].objects)
            obj.SetActive(true);
    }
}
