using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class DraggableOption : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private string targetTag = "DropTarget";

    [Header("Target Renderer (Mesh or SkinnedMesh)")]
    public Renderer targetRenderer;

    [Header("Materials")]
    public Material highlightMaterial;
    public Material originalMaterial;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Vector2 originalPosition;
    private Coroutine returnCoroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
    }

    private void Start()
    {
        originalPosition = rectTransform.anchoredPosition;

        if (mainCamera == null)
            mainCamera = Camera.main;

        // Save original material if not set manually
        if (targetRenderer != null && originalMaterial == null)
            originalMaterial = targetRenderer.sharedMaterial;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (returnCoroutine != null)
            StopCoroutine(returnCoroutine);

        // 🔥 Apply highlight material during drag
        if (targetRenderer != null)
        {
            targetRenderer.material = highlightMaterial;
            targetRenderer.enabled = true;
        }

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag(targetTag))
            {
                Debug.Log("Dropped correctly on: " + hit.collider.name);

                if (targetRenderer != null)
                {
                    // 🎯 Trigger the boom dissolve effect
                    SmoothDissolve dissolve = targetRenderer.GetComponent<SmoothDissolve>();
                    if (dissolve != null)
                        dissolve.StartBoomReveal();
                }

                // Disable the dragged UI icon
                gameObject.SetActive(false);
                return;
            }
        }
        else
        {
            if (targetRenderer != null)
                targetRenderer.enabled = false;
        }

        // ❌ Not dropped correctly → return back
        returnCoroutine = StartCoroutine(ReturnToOriginalPosition());
    }

    private System.Collections.IEnumerator ReturnToOriginalPosition()
    {
        float duration = 0.4f;
        float elapsed = 0f;
        Vector2 startPos = rectTransform.anchoredPosition;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            t = 1f - Mathf.Pow(1f - t, 3f);
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, originalPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPosition;
        returnCoroutine = null;
    }
}

