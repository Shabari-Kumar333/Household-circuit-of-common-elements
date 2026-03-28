//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[RequireComponent(typeof(LineRenderer))]
//public class SimpleWireDrawer : MonoBehaviour
//{
//    [Header("Points (assign in Inspector)")]
//    public Transform startPoint;             // REQUIRED
//    public Transform[] midPoints;            // optional (0..n)
//    public Transform endPoint;               // REQUIRED

//    [Header("Line & Glow")]
//    public LineRenderer line;                // optional, will get component if null
//    public Transform glowPoint;              // small sprite/particle that follows tip

//    [Header("Drawing")]
//    [Tooltip("Total seconds to draw from start -> end.")]
//    public float drawDuration = 1.5f;

//    // internal
//    bool isDrawing = false;
//    bool hasDrawn = false;   // Option 2: if already drawn, do nothing on re-enable
//    float elapsed = 0f;

//    // cached path
//    Vector3[] pathPoints;    // start, mids..., end
//    float[] segmentLengths;
//    float totalLength;

//    void Awake()
//    {
//        // ensure line reference
//        if (line == null)
//            line = GetComponent<LineRenderer>();

//        // hide visuals initially (before script enabled/run)
//        if (line != null)
//            line.enabled = false;

//        if (glowPoint != null)
//            glowPoint.gameObject.SetActive(false);
//    }

//    void OnEnable()
//    {
//        // Only begin drawing the first time the component is enabled.
//        if (hasDrawn) return;

//        // Validate required points
//        if (startPoint == null || endPoint == null)
//        {
//            Debug.LogWarning($"[{name}] SimpleWireDrawer: StartPoint and EndPoint are required.");
//            return;
//        }

//        BuildPath();
//        PrepareLine();
//        StartDrawing();
//    }

//    void OnDisable()
//    {
//        // Do not reset hasDrawn — Option 2 requires we keep previous result
//        // But stop drawing if mid-animation and component gets disabled.
//        isDrawing = false;
//    }

//    void BuildPath()
//    {
//        int mids = (midPoints != null) ? midPoints.Length : 0;
//        pathPoints = new Vector3[2 + mids];
//        pathPoints[0] = startPoint.position;
//        for (int i = 0; i < mids; i++)
//            pathPoints[1 + i] = midPoints[i] != null ? midPoints[i].position : pathPoints[1 + i - 1];
//        pathPoints[pathPoints.Length - 1] = endPoint.position;

//        // compute lengths
//        segmentLengths = new float[pathPoints.Length - 1];
//        totalLength = 0f;
//        for (int i = 0; i < pathPoints.Length - 1; i++)
//        {
//            float seg = Vector3.Distance(pathPoints[i], pathPoints[i + 1]);
//            segmentLengths[i] = seg;
//            totalLength += seg;
//        }

//        // avoid zero division
//        if (totalLength <= Mathf.Epsilon)
//            totalLength = Mathf.Epsilon;
//    }

//    void PrepareLine()
//    {
//        if (line == null) return;

//        // set positions initially all to start (collapsed)
//        int count = pathPoints.Length;
//        line.positionCount = count;
//        for (int i = 0; i < count; i++)
//            line.SetPosition(i, pathPoints[0]);

//        line.enabled = true;
//    }

//    void StartDrawing()
//    {
//        elapsed = 0f;
//        isDrawing = true;
//        hasDrawn = false;

//        if (glowPoint != null)
//            glowPoint.gameObject.SetActive(true);
//    }

//    void Update()
//    {
//        if (!isDrawing) return;
//        if (pathPoints == null || pathPoints.Length < 2) return;

//        elapsed += Time.deltaTime;
//        float t = Mathf.Clamp01(elapsed / drawDuration); // 0..1 progress across whole path
//        float targetDistance = t * totalLength;

//        // Build the current line positions: for segments before current -> full, current -> partial,
//        // segments after -> duplicated tip (so line appears continuous to tip).
//        Vector3 tipPosition = pathPoints[pathPoints.Length - 1]; // default end

//        float acc = 0f;
//        int currentSeg = 0;
//        float segProgress = 0f;

//        // find which segment contains the targetDistance
//        for (int i = 0; i < segmentLengths.Length; i++)
//        {
//            if (acc + segmentLengths[i] >= targetDistance)
//            {
//                currentSeg = i;
//                float segTarget = targetDistance - acc;
//                segProgress = (segmentLengths[i] <= Mathf.Epsilon) ? 1f : (segTarget / segmentLengths[i]);
//                tipPosition = Vector3.Lerp(pathPoints[i], pathPoints[i + 1], segProgress);
//                break;
//            }
//            acc += segmentLengths[i];
//            // if loop completes without break, we are at end; handled below
//            if (i == segmentLengths.Length - 1)
//            {
//                currentSeg = segmentLengths.Length - 1;
//                segProgress = 1f;
//                tipPosition = pathPoints[pathPoints.Length - 1];
//            }
//        }

//        // fill positions for LineRenderer
//        int totalPts = pathPoints.Length;
//        Vector3[] linePts = new Vector3[totalPts];

//        for (int i = 0; i < totalPts; i++)
//        {
//            if (i < currentSeg + 1) // earlier points (including start of current)
//            {
//                linePts[i] = pathPoints[i];
//            }
//            else if (i == currentSeg + 1)
//            {
//                // this is the current segment's end position -> set to tip
//                linePts[i] = tipPosition;
//            }
//            else
//            {
//                // beyond tip: set to tip so line stops there
//                linePts[i] = tipPosition;
//            }
//        }

//        // apply to LineRenderer
//        if (line != null)
//        {
//            line.positionCount = linePts.Length;
//            line.SetPositions(linePts);
//        }

//        // move glow
//        if (glowPoint != null)
//            glowPoint.position = tipPosition;

//        // finish
//        if (t >= 1f)
//        {
//            isDrawing = false;
//            hasDrawn = true;

//            // turn off glow when done
//            if (glowPoint != null)
//                glowPoint.gameObject.SetActive(false);

//            // keep the final line visible (do nothing else)
//        }
//    }
//}

using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SimpleWireDrawer : MonoBehaviour
{
    [Header("Points (assign in Inspector)")]
    public Transform startPoint;
    public Transform[] midPoints;
    public Transform endPoint;

    [Header("Line & Glow")]
    public LineRenderer line;
    public ParticleSystem glowEffect;   // ✔ Particle System

    [Header("Drawing")]
    public float drawDuration = 1.5f;

    bool isDrawing = false;
    bool hasDrawn = false;
    float elapsed = 0f;

    Vector3[] pathPoints;
    float[] segmentLengths;
    float totalLength;

    void Awake()
    {
        if (line == null)
            line = GetComponent<LineRenderer>();

        // Hide wire before enabled
        line.enabled = false;

        // ✔ Make sure particle system is completely stopped
        if (glowEffect != null)
        {
            glowEffect.Stop();
            glowEffect.Clear();
            glowEffect.gameObject.SetActive(false); // disable object
        }
    }

    void OnEnable()
    {
        if (hasDrawn) return;  // Option 2 behaviour

        if (startPoint == null || endPoint == null)
        {
            Debug.LogWarning("Start/End point missing.");
            return;
        }

        BuildPath();
        PrepareLine();
        StartDrawing();
    }

    void OnDisable()
    {
        isDrawing = false;
    }

    void BuildPath()
    {
        int mids = midPoints != null ? midPoints.Length : 0;
        pathPoints = new Vector3[2 + mids];

        pathPoints[0] = startPoint.position;

        for (int i = 0; i < mids; i++)
        {
            pathPoints[i + 1] = midPoints[i] != null ? midPoints[i].position : pathPoints[i];
        }

        pathPoints[pathPoints.Length - 1] = endPoint.position;

        // calculate lengths
        segmentLengths = new float[pathPoints.Length - 1];
        totalLength = 0f;

        for (int i = 0; i < segmentLengths.Length; i++)
        {
            segmentLengths[i] = Vector3.Distance(pathPoints[i], pathPoints[i + 1]);
            totalLength += segmentLengths[i];
        }
    }

    void PrepareLine()
    {
        line.positionCount = pathPoints.Length;

        for (int i = 0; i < pathPoints.Length; i++)
            line.SetPosition(i, pathPoints[0]);

        line.enabled = true;
    }

    void StartDrawing()
    {
        elapsed = 0f;
        isDrawing = true;
        hasDrawn = false;

        // ✔ Enable + play glow particles only now
        if (glowEffect != null)
        {
            glowEffect.gameObject.SetActive(true);
            glowEffect.Play();
        }
    }

    void Update()
    {
        if (!isDrawing) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / drawDuration);
        float targetDistance = t * totalLength;

        Vector3 tipPosition = pathPoints[pathPoints.Length - 1];

        float acc = 0f;
        int currentSeg = 0;
        float segProgress = 0f;

        for (int i = 0; i < segmentLengths.Length; i++)
        {
            if (acc + segmentLengths[i] >= targetDistance)
            {
                currentSeg = i;
                float segTarget = targetDistance - acc;
                segProgress = segTarget / segmentLengths[i];
                tipPosition = Vector3.Lerp(pathPoints[i], pathPoints[i + 1], segProgress);
                break;
            }
            acc += segmentLengths[i];

            if (i == segmentLengths.Length - 1)
            {
                tipPosition = pathPoints[pathPoints.Length - 1];
            }
        }

        Vector3[] linePts = new Vector3[pathPoints.Length];

        for (int i = 0; i < linePts.Length; i++)
        {
            if (i <= currentSeg)
                linePts[i] = pathPoints[i];
            else if (i == currentSeg + 1)
                linePts[i] = tipPosition;
            else
                linePts[i] = tipPosition;
        }

        line.SetPositions(linePts);

        // ✔ Move glow particle to tip position
        if (glowEffect != null)
            glowEffect.transform.position = tipPosition;

        // Finish
        if (t >= 1f)
        {
            isDrawing = false;
            hasDrawn = true;

            // ✔ stop & hide particles at end
            if (glowEffect != null)
            {
                glowEffect.Stop();
                glowEffect.Clear();
                glowEffect.gameObject.SetActive(false);
            }
        }
    }
}


