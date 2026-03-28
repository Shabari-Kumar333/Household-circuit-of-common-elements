using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SlideManager : MonoBehaviour
{
    [Header("Slide Anchors (Camera Positions)")]
    public Transform[] slideAnchors;

    [Header("Slide Panels (UI)")]
    public GameObject[] slideUIs;

    [Header("Camera")]
    public Camera mainCam;
    public float camMoveSpeed = 2f;

    [Header("Navigation")]
    public Button btnNext;
    public Button btnPrev;

    [Header("Progress UI")]
    public TMP_Text slideNumberText;

    [Header("Slide Config")]
    public int totalSlides = 36;

    // 🔒 Internal slide state (1-based)
    private int currentSlide = 1;

    // ✅ PUBLIC READ-ONLY INDEX (0-based for arrays & evaluation)
    public int CurrentSlideIndex
    {
        get { return currentSlide - 1; }
    }

    private void Start()
    {
        if (btnNext != null)
            btnNext.onClick.AddListener(() => ChangeSlide(1));

        if (btnPrev != null)
            btnPrev.onClick.AddListener(() => ChangeSlide(-1));

        RefreshSlide();
    }

    void ChangeSlide(int direction)
    {
        currentSlide += direction;

        // 🔁 Wrap-around logic
        if (currentSlide < 1)
            currentSlide = totalSlides;

        if (currentSlide > totalSlides)
            currentSlide = 1;

        RefreshSlide();
    }

    public void ChangeSlideExternally(int direction)
    {
        ChangeSlide(direction);
    }

    void RefreshSlide()
    {
        UpdateUI();
        UpdateSlideUI();
        MoveCameraToSlide();
    }

    void UpdateUI()
    {
        if (slideNumberText != null)
            slideNumberText.text = $"{currentSlide} / {totalSlides}";
    }

    void UpdateSlideUI()
    {
        for (int i = 0; i < slideUIs.Length; i++)
        {
            if (slideUIs[i] != null)
                slideUIs[i].SetActive(i == CurrentSlideIndex);
        }
    }

    void MoveCameraToSlide()
    {
        if (slideAnchors == null || slideAnchors.Length == 0) return;
        if (CurrentSlideIndex >= slideAnchors.Length) return;

        StopAllCoroutines();
        StartCoroutine(SmoothCamMove(slideAnchors[CurrentSlideIndex]));
    }

    IEnumerator SmoothCamMove(Transform target)
    {
        Vector3 startPos = mainCam.transform.position;
        Quaternion startRot = mainCam.transform.rotation;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * camMoveSpeed;
            mainCam.transform.position = Vector3.Lerp(startPos, target.position, t);
            mainCam.transform.rotation = Quaternion.Slerp(startRot, target.rotation, t);
            yield return null;
        }
    }
}
