using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCinemachineControllerNew : MonoBehaviour
{
    [Header("Assign Virtual Camera")]
    public CinemachineVirtualCamera vcam;

    [Header("Camera Positions (Order)")]
    public List<Transform> cameraTargets = new List<Transform>(); // Add your list of transforms here

    [Header("Movement Settings")]
    public float transitionSpeed = 2f;

    private int currentIndex = 0; // Start from first transform
    private Transform target;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        if (cameraTargets.Count > 0)
        {
            // Start moving to the first transform
            target = cameraTargets[0];
        }
    }

    void Update()
    {
        // Press Space to move to the next transform
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveToNextTarget();
        }

        // Smoothly move and rotate the camera
        if (target != null && vcam != null)
        {
            vcam.transform.position = Vector3.SmoothDamp(
                vcam.transform.position,
                target.position,
                ref velocity,
                1f / transitionSpeed
            );

            vcam.transform.rotation = Quaternion.Slerp(
                vcam.transform.rotation,
                target.rotation,
                Time.deltaTime * transitionSpeed
            );

            // Snap exactly when close enough
            if (Vector3.Distance(vcam.transform.position, target.position) < 0.01f)
            {
                vcam.transform.position = target.position;
                vcam.transform.rotation = target.rotation;
            }
        }
    }

    public void MoveToNextTarget()
    {
        Debug.Log("MoveToNextTarget called"); // ✅ Add this line

        if (cameraTargets.Count == 0) return;

        currentIndex++;
        if (currentIndex >= cameraTargets.Count)
            currentIndex = 0; // Loop back to first

        target = cameraTargets[currentIndex];
    }
}
