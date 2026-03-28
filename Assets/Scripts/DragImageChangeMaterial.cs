using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropImage : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject  lineRenderer;
    public GameObject[] Glowwires;
    public Material material;
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 startPos;
    private Transform startParent;

    // ✅ ADD
    public Camera Camera;
    public Color dropColor = Color.green;
     Vector3 previousscale;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        startPos = rectTransform.anchoredPosition;
        previousscale= rectTransform.localScale;

        // ✅ ADD (safe fallback)
       
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;
        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition +=
            eventData.delta / canvas.scaleFactor;
            rectTransform.localScale=new Vector3(5f,5f,5f);

    }
    

    public void OnEndDrag(PointerEventData eventData)
    {
    
           
        if (this.gameObject.CompareTag("Correct"))
        {
                    Wirefunctionality();
                    rectTransform.localScale=previousscale;
                    
                    

        }
        else
        {
                      transform.SetParent(startParent);
            rectTransform.anchoredPosition = startPos;
            rectTransform.localScale=previousscale;
        }
     
    }

    private void  Wirefunctionality()
    {
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Wire"))
            {
                lineRenderer.SetActive(true);
                foreach (GameObject gameObject in Glowwires)
                {
                    gameObject.GetComponent<Renderer>().material = material;
                    gameObject.SetActive(false);
                    lineRenderer.SetActive(true);
                    transform.SetParent(startParent);
                    rectTransform.anchoredPosition = startPos;
                }
                Debug.Log("hit");
                
            }
            else
            {
                 transform.SetParent(startParent);
            rectTransform.anchoredPosition = startPos;
            }

            // snap back to original position
        }
            transform.SetParent(startParent);
            rectTransform.anchoredPosition = startPos;
    }
}