//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class UIDragWire : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
//{
//    [Header("UI Settings")]
//    public RectTransform dragObject;     // The UI element you drag
//    public Canvas canvas;                // Main Canvas (for scaling)

//    [Header("Drop Settings (3D Target)")]
//    public Camera raycastCamera;         // Assign your custom camera
//    public LayerMask dropLayer;          // Layer for the 3D target
//    public string dropTag = "WireTarget"; // Tag of your 3D drop object

//    [Header("Wire Drawer")]
//    public WireDrawer wireDrawer;    // Reference to your wire script

//    private Vector2 originalPos;
//    private bool dropped = false;

//    void Start()
//    {
//        if (dragObject == null)
//            dragObject = GetComponent<RectTransform>();

//        originalPos = dragObject.anchoredPosition;
//    }

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        if (dropped) return;
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        if (dropped) return;

//        // UI movement
//        Vector2 pos;
//        RectTransformUtility.ScreenPointToLocalPointInRectangle(
//            canvas.transform as RectTransform,
//            eventData.position,
//            eventData.pressEventCamera,
//            out pos);

//        dragObject.anchoredPosition = pos;

//        // Check 3D drop while dragging
//        TryDropOn3DTarget(eventData);
//    }

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        if (dropped) return;

//        // If not dropped correctly → reset position
//        dragObject.anchoredPosition = originalPos;
//    }

//    private void TryDropOn3DTarget(PointerEventData eventData)
//    {
//        if (raycastCamera == null) return;

//        Ray ray = raycastCamera.ScreenPointToRay(eventData.position);

//        if (Physics.Raycast(ray, out RaycastHit hit, 150f, dropLayer))
//        {
//            if (hit.collider.CompareTag(dropTag))
//            {
//                dropped = true;

//                // Snap UI to the target visually (optional)
//                dragObject.anchoredPosition = originalPos; // or hide UI if you want

//                // Start wire drawing
//                if (wireDrawer != null)
//                {
//                    wireDrawer.StartWireDrawing();
//                }
//            }
//        }
//    }
//}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class UIDragWire : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
//{
//    public WireDrawer wireDrawer;
//    public int linesToDraw = 1;        // 1 = normal, 2 or 3 = experiment 1

//    public GameObject nextWireUI;
//    public Transform wireSnapPoint;
//    public Transform targetSnapPoint;

//    private Vector3 startPos;
//    private bool dropped = false;
//    public float snapDistance = 0.25f;

//    void Start()
//    {
//        startPos = transform.position;
//    }

//    public void OnBeginDrag(PointerEventData e)
//    {
//        if (dropped) return;
//    }

//    public void OnDrag(PointerEventData e)
//    {
//        if (dropped) return;

//        Vector3 mouse = e.position;
//        mouse.z = Camera.main.WorldToScreenPoint(transform.position).z;
//        transform.position = Camera.main.ScreenToWorldPoint(mouse);
//    }

//    public void OnEndDrag(PointerEventData e)
//    {
//        if (dropped) return;

//        float dist = Vector3.Distance(wireSnapPoint.position, targetSnapPoint.position);

//        if (dist < snapDistance)
//        {
//            dropped = true;

//            transform.position = targetSnapPoint.position;

//            // 🎯 Draw single or multiple lines
//            if (wireDrawer != null)
//                wireDrawer.EnableLines(linesToDraw);

//            if (nextWireUI != null)
//                nextWireUI.SetActive(true);

//            gameObject.SetActive(false); // Hide current UI
//        }
//        else
//        {
//            transform.position = startPos;
//        }
//    }
//}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragWire : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI Drag Settings")]
    public RectTransform dragObject;
    public Canvas canvas;
    private Vector2 originalPos;

    [Header("3D Drop Settings")]
    public Camera raycastCamera;
    public float raycastDistance = 200f;
    public string targetTag;   // ✔ Tag assigned from INSPECTOR ONLY

    [Header("Enable On Successful Drop")]
    public List<GameObject> enableObjects = new List<GameObject>();

    public List<GameObject> disableObjects = new List<GameObject>();

    [Header("Next UI Wire (Optional)")]
    public List<GameObject> nextUIWire = new List<GameObject>();

    private bool dropped = false;

    private Vector3 originalScale;
    public float dragScaleMultiplier = 1.2f;  // how much bigger it becomes

    void Start()
    {
        if (dragObject == null)
            dragObject = GetComponent<RectTransform>();

        originalPos = dragObject.anchoredPosition;

        if (raycastCamera == null)
            raycastCamera = Camera.main;

        originalScale = dragObject.localScale;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dropped) return;

        // Scale up on pick
        dragObject.localScale = originalScale * dragScaleMultiplier;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dropped) return;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pos);

        dragObject.anchoredPosition = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dropped) return;

        if (TryDrop(eventData))
            SuccessfulDrop();
        else
            ResetPosition();
    }

    bool TryDrop(PointerEventData eventData)
    {
        Ray ray = raycastCamera.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            // ✔ Compare USING INSPECTOR TAG, NOT HARD CODED
            if (hit.collider.CompareTag(targetTag))
            {
                return true;
            }
        }

        return false;
    }

    void SuccessfulDrop()
    {
        dropped = true;

        // ✔ Hide UI wire
        dragObject.gameObject.SetActive(false);

        // ✔ Enable all objects assigned in Inspector
        foreach (var obj in enableObjects)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        foreach (var obj in disableObjects)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // ✔ Enable next UI wire
        foreach (var obj in nextUIWire)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }

    void ResetPosition()
    {
        dragObject.anchoredPosition = originalPos;

        dragObject.anchoredPosition = originalPos;
        dragObject.localScale = originalScale;  // reset size
    }
}




