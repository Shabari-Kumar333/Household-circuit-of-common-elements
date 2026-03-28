using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragOverlay : MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Vector2 originalAnchoredPos;
    private Transform originalParent;
    private Coroutine returnCoroutine;

    [Header("Assign in Inspector")]
    public RectTransform dragLayer;   // ← DragLayer (REQUIRED)

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    private void Start()
    {
        originalAnchoredPos = rectTransform.anchoredPosition;
        originalParent = rectTransform.parent;
    }

    // ---------------- DRAG START ----------------
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (returnCoroutine != null)
            StopCoroutine(returnCoroutine);

        canvasGroup.blocksRaycasts = false;

        // 🚀 MOVE TO TOP LAYER (THIS IS THE REAL FIX)
        rectTransform.SetParent(dragLayer, true);
        rectTransform.SetAsLastSibling();
    }

    // ---------------- DRAGGING (YOUR METHOD UNCHANGED) ----------------
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition +=
            eventData.delta / canvas.scaleFactor;
    }

    // ---------------- DRAG END ----------------
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Return to original parent
        rectTransform.SetParent(originalParent, true);

        returnCoroutine = StartCoroutine(ReturnToOriginalPosition());
    }

    // ---------------- RETURN ----------------
    private IEnumerator ReturnToOriginalPosition()
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Vector2 startPos = rectTransform.anchoredPosition;

        while (elapsed < duration)
        {
            rectTransform.anchoredPosition =
                Vector2.Lerp(startPos, originalAnchoredPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalAnchoredPos;
        returnCoroutine = null;
    }
}
