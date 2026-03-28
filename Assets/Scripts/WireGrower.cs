using UnityEngine;
using System.Collections;

public class WireGrowWithWaypoints : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public Transform startPoint;
    public Transform endPoint;
    public Transform[] waypoints;

    public float growSpeed = 3f;

    [Header("Glow Tip")]
    public Transform glowTip;   // assign glow object here

    private void Start()
    {
        StartCoroutine(GrowWire());
    }

    IEnumerator GrowWire()
    {
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, startPoint.position);

        Vector3 prev = startPoint.position;

        // Waypoints
        for (int i = 0; i < waypoints.Length; i++)
        {
            lineRenderer.positionCount++;
            yield return StartCoroutine(GrowSegment(prev, waypoints[i].position, lineRenderer.positionCount - 1));
            prev = waypoints[i].position;
        }

        // Final End
        lineRenderer.positionCount++;
        yield return StartCoroutine(GrowSegment(prev, endPoint.position, lineRenderer.positionCount - 1));
    }

    IEnumerator GrowSegment(Vector3 start, Vector3 end, int index)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * growSpeed;
            Vector3 pos = Vector3.Lerp(start, end, t);

            lineRenderer.SetPosition(index, pos);

            // ✅ Glow EXACTLY at tip
            if (glowTip != null)
                glowTip.position = pos;

            yield return null;
        }

        lineRenderer.SetPosition(index, end);

        if (glowTip != null)
            glowTip.position = end;
    }
}
