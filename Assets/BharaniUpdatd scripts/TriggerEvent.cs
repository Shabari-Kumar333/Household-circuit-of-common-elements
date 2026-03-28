using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [Header("Trigger Event")]
    public UnityEvent onTriggered;   // Assign actions in Inspector

    [Header("Trigger Settings")]
    public bool triggerOnce = true;  // Optional: fire only once

    bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggerOnce && hasTriggered)
            return;

        hasTriggered = true;
        onTriggered?.Invoke();
        Debug.Log("Triggered");
    }
}