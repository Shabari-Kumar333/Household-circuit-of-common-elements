using UnityEngine;

public class SwitchRaycast : MonoBehaviour
{
    public string switchTag = "Switch";

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // ✅ Check if it is a switch
                if (hit.collider.CompareTag(switchTag))
                {
                    Animator animator = hit.collider.GetComponent<Animator>();

                    // If Animator is on child, use GetComponentInChildren
                    if (animator == null)
                        animator = hit.collider.GetComponentInChildren<Animator>();

                    if (animator != null)
                    {
                        animator.Play("On");
                    }
                    else
                    {
                        Debug.LogWarning("No Animator found on Switch!");
                        print("print");
                        print("Print");
                    }
                }
            }
        }
    }
}
