using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterEable : MonoBehaviour
{
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    public GameObject nextButton;

    void Update()
    {
        if (obj1.GetComponent<MeshRenderer>().enabled &&
            obj2.GetComponent<MeshRenderer>().enabled &&
            obj3.GetComponent<MeshRenderer>().enabled)
        {
            nextButton.SetActive(true);
        }
        //else
        //{
        //    nextButton.interactable = false;
        //}
    }
}
