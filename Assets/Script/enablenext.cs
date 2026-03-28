using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class enablenext : MonoBehaviour
{
    public GameObject[] objects;   // add any number
    public UnityEvent onAllTrue;   // assign in Inspector

    void Update()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (!objects[i].GetComponent<MeshRenderer>().enabled)
                return; // if any one is false, do nothing
        }

        onAllTrue.Invoke(); // all are true
        enabled = false; // stop checking (important)
    }
}
