using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class NewDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [Header("References")]
    [SerializeField] private Camera mainCamera;    // Assign Main Camera
    [SerializeField] private string targetTag = "DropTarget"; // Tag for drop object
    public GameObject targetobject;
    public Material higlightmaterial;
    public Material Orginalmaterial;
    public UnityEvent object1;

    //[Header("Events")]
    //public UnityEvent onDroppedCorrectly;  // Trigger custom actions in Inspector

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
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (returnCoroutine != null)
            StopCoroutine(returnCoroutine);
        targetobject.GetComponent<MeshRenderer>().material = higlightmaterial;
        targetobject.GetComponent<MeshRenderer>().enabled = true;


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
                object1.Invoke();
                // Enable the target GameObject
                //hit.collider.gameObject.SetActive(true);

                // Trigger the UnityEvent
                //onDroppedCorrectly?.Invoke();
                targetobject.GetComponent<MeshRenderer>().material = Orginalmaterial;
                //targetobject.GetComponent<MeshRenderer>().enabled = true;

                // Disable the dragged UI
                gameObject.SetActive(false);

                return; // Stop here, no need to return to original position
            }
        }
        else
        {

            targetobject.GetComponent<MeshRenderer>().enabled = false;

        }

        // If not dropped correctly
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
