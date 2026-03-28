using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class ToggleSwitchRaycast : MonoBehaviour
{
    public string switchTag = "Switch";
    public Camera camera;

    public GameObject liight3;
    public GameObject[] lights;

    public GameObject[] allswitches;

    public GameObject lightparent;

    public TextMeshProUGUI statustext;

    bool ison;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag(switchTag))
                {
                    string lightname = hit.collider.gameObject.name;
                    Animator animator = hit.collider.GetComponent<Animator>();
                    
                    if (animator == null)
                        animator = hit.collider.GetComponentInChildren<Animator>();

                    if (animator == null)
                        return;

                    // ✅ Toggle logic using Animator parameter
                    bool isOn = animator.GetBool("IsOn");
          

                    if (isOn)
                    {
                        animator.Play("Off");
                        Transform child = hit.collider.gameObject.transform.GetChild(0);
                        child.gameObject.SetActive(false);
                        animator.SetBool("IsOn", false);
                      liight3.SetActive(false);
                        onlight(lightname, false);
                    }
                    else
                    {
                             liight3.SetActive(true);
                        animator.Play("On");
                        Transform child = hit.collider.gameObject.transform.GetChild(0);
                        child.gameObject.SetActive(true);
                        animator.SetBool("IsOn", true);
                        onlight(lightname, true);
                    }
                }
            }
        }
    }

    public void onlight(string lightname, bool state)
    {
        int index = int.Parse(lightname);
        GameObject light = lights[index].gameObject;
        light.SetActive(state);
    }

    public void turnonoroffallswitches()
    {
        if (!ison)
        {
            statustext.text="All Switches Off";
            ison = true;
             liight3.SetActive(true);
            lights[0].SetActive(true);
            lights[1].SetActive(true);
            lights[2].SetActive(true);
            
            foreach (GameObject gameObject in allswitches)
            {
                Animator animator = gameObject.GetComponent<Animator>();
                bool isOn = animator.GetBool("IsOn");
                animator.Play("On");
                Transform child = gameObject.transform.GetChild(0);
                child.gameObject.SetActive(true);
                animator.SetBool("IsOn", true);
            }
        }
        else if (ison)
        {
               statustext.text="All Switches On";
            ison = false;
             liight3.SetActive(false);
            lights[0].SetActive(false);
            lights[1].SetActive(false);
            lights[2].SetActive(false);
            foreach (GameObject gameObject in allswitches)
            {
                Animator animator = gameObject.GetComponent<Animator>();
                bool isOn = animator.GetBool("IsOn");
                animator.Play("Off");
                Transform child = gameObject.transform.GetChild(0);
                child.gameObject.SetActive(false);
                animator.SetBool("IsOn", true);
            }
        }
   
    }
  
}
