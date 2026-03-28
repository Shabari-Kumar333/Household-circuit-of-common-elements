using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent3D : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnCollisionEnterEvent;
    public UnityEvent OnCollisionExitEvent;

    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerExitEvent;

    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionEnterEvent?.Invoke();
    }

    private void OnCollisionExit(Collision collision)
    {
        OnCollisionExitEvent?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent?.Invoke();
    }
}
