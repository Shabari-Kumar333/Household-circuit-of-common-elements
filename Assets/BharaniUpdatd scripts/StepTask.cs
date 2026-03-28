using UnityEngine;

public class StepTask : MonoBehaviour
{
    private bool completed = false;

    public void CompleteStep()
    {
        if (completed) return;

        completed = true;
        StepFlowController.Instance.MarkCurrentStepCompleted();
    }
}
