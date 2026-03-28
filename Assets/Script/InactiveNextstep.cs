using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InactiveNextstep : MonoBehaviour
{
    public GameObject[] watchObjects;
    public UnityEvent onAnyInactive;

    void Update()
    {
        for (int i = 0; i < watchObjects.Length; i++)
        {
            if (watchObjects[i] != null && !watchObjects[i].activeInHierarchy)
            {
                onAnyInactive.Invoke();
                enabled = false; // stop after first trigger
                break;
            }
        }
    }
}
