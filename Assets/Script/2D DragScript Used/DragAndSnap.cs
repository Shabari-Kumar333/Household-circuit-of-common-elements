using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DragAndSnap : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private Vector3 startPos;

    public Transform targetDropZone;
    public UnityEvent onDropSuccess;
    public UnityEvent onDropFail;

    public DragGroupController groupController;   // ⭐ NEW

    private bool isSnapped = false;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSnapped) return;
        startPos = rectTransform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isSnapped) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isSnapped) return;

        float distance = Vector2.Distance(rectTransform.position, targetDropZone.position);

        if (distance < 100f)
        {
            rectTransform.position = targetDropZone.position;
            isSnapped = true;

            onDropSuccess?.Invoke();

            // ⭐ REPORT SUCCESS TO GROUP MANAGER
            if (groupController != null)
                groupController.ReportSuccess();
        }
        else
        {
            rectTransform.position = startPos;
            onDropFail?.Invoke();
        }
    }
}
