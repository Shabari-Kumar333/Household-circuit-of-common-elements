using UnityEngine;
using UnityEngine.Events;

public class DragGroupController : MonoBehaviour
{
    [Header("Total Objects To Snap")]
    public int requiredCount = 3;

    private int currentCount = 0;

    [Header("When All Snapped")]
    public UnityEvent onAllSnapped;   // ⭐ Slide Completed Trigger

    public void ReportSuccess()
    {
        currentCount++;

        if (currentCount >= requiredCount)
        {
            onAllSnapped?.Invoke();
        }
    }
}
