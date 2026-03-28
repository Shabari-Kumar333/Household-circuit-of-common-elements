using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraTransform : MonoBehaviour
{
    public Transform[] cameraPoints;
    public float smoothSpeed = 3f;

    private int currentIndex = 0;

    void Start()
    {
        // Snap camera to first point at start
        transform.position = cameraPoints[0].position;
        transform.rotation = cameraPoints[0].rotation;
    }

    void Update()
    {
        Transform target = cameraPoints[currentIndex];

        transform.position = Vector3.Lerp(
            transform.position,
            target.position,
            Time.deltaTime * smoothSpeed
        );

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            target.rotation,
            Time.deltaTime * smoothSpeed
        );
    }

    // NEXT BUTTON
    public void Next()
    {
        if (currentIndex < cameraPoints.Length - 1)
            currentIndex++;
    }

    // BACK BUTTON
    public void Back()
    {
        if (currentIndex > 0)
            currentIndex--;
    }
}
