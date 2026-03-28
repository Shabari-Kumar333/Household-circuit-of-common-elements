using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LineNextEnable : MonoBehaviour
{
    [Header("References")]
    public LineRenderer[] lineRenderers;

    [Header("Events")]
    public UnityEvent onAnyLineEnabled;

    private bool hasInvoked = false;

    void Update()
    {
        if (hasInvoked) return;

        for (int i = 0; i < lineRenderers.Length; i++)
        {
            if (lineRenderers[i] != null && lineRenderers[i].enabled)
            {
                hasInvoked = true;
                onAnyLineEnabled.Invoke();
                break;
            }
        }
    }

    // Optional: reuse again
    public void ResetEvent()
    {
        hasInvoked = false;
    }
}
