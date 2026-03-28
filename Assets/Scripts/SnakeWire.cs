using UnityEngine;

public class SmoothWireGrower : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform[] controlPoints;
    public float growSpeed = 0.5f;

    private float t = 0f;   // overall progress (0-1)
    private int smoothness = 50; // higher = smoother line

    void Start()
    {
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, controlPoints[0].position);
    }

    void Update()
    {
        if (t >= 1f) return;

        t += growSpeed * Time.deltaTime;
        t = Mathf.Clamp01(t);

        UpdateWire();
    }

    void UpdateWire()
    {
        int totalPoints = Mathf.CeilToInt(smoothness * t);
        lineRenderer.positionCount = totalPoints;

        for (int i = 0; i < totalPoints; i++)
        {
            float segmentT = (float)i / (smoothness - 1);
            Vector3 pos = GetCatmullRomPoint(segmentT);
            lineRenderer.SetPosition(i, pos);
        }
    }

    Vector3 GetCatmullRomPoint(float t)
    {
        // Find which segment we are on
        int numSections = controlPoints.Length - 3;
        int current = Mathf.Min(Mathf.FloorToInt(t * numSections), numSections - 1);

        float u = t * numSections - current;

        Vector3 p0 = controlPoints[current].position;
        Vector3 p1 = controlPoints[current + 1].position;
        Vector3 p2 = controlPoints[current + 2].position;
        Vector3 p3 = controlPoints[current + 3].position;

        // Catmull-Rom Formula
        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * u +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * u * u +
            (-p0 + 3f * p1 - 3f * p2 + p3) * u * u * u
        );
    }
}
