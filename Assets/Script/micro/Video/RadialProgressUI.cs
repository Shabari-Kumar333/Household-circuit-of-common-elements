using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RadialProgressUI : MonoBehaviour
{
    public Image circleFill; // Assign the CircleFill Image
    public TextMeshProUGUI percentageText; // Assign the Text component
    [Range(0, 100)]
    public float targetPercentage = 85f;

    void Start()
    {
        UpdateProgress(targetPercentage);
    }

    public void UpdateProgress(float value)
    {
        float fillAmount = Mathf.Clamp01(value / 100f);
        circleFill.fillAmount = fillAmount;
        percentageText.text = Mathf.RoundToInt(value) + "%";
    }
}
