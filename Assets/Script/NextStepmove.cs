using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NextStepmove : MonoBehaviour
{
    public GameObject[] watchObjects;   // objects to watch
    //public GameObject targetObject;     // object to enable
    public UnityEvent onAnyActive;

    void Update()
    {
        for (int i = 0; i < watchObjects.Length; i++)
        {
            if (watchObjects[i] != null && watchObjects[i].activeInHierarchy)
            {
               // targetObject.SetActive(true);
                onAnyActive.Invoke();
                enabled = false; // stop after first trigger
                break;
            }
        }
    }
}
