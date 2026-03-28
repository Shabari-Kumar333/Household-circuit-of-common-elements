using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class VideoButtonPlingLoop : MonoBehaviour
{
    [Header("References")]
    public VideoPlayer videoPlayer;   // Reference to your VideoPlayer
    public Button uiButton;           // Reference to your UI Button

    [Header("Pling Animation Settings")]
    public float plingScale = 1.2f;       // How much the button scales up
    public float plingUpDuration = 0.2f;  // Speed of scaling up
    public float plingDownDuration = 0.2f; // Speed of scaling down
    public float waitBetweenPlings = 0.5f; // Pause between each pling

    private Vector3 originalScale;
    private bool buttonClicked = false;

    void Start()
    {
        // Store the original button scale
        originalScale = uiButton.transform.localScale;

        // Hide the button initially
        uiButton.gameObject.SetActive(false);

        // Listen for video finished event
        videoPlayer.loopPointReached += OnVideoFinished;

        // Listen for button click
        uiButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        // Show the button
        uiButton.gameObject.SetActive(true);

        // Start continuous pling animation
        StartCoroutine(PlingLoop());
    }

    private void OnButtonClicked()
    {
        // Stop pling loop when button is clicked
        buttonClicked = true;
    }

    IEnumerator PlingLoop()
    {
        while (!buttonClicked)
        {
            // Scale up
            float elapsed = 0f;
            while (elapsed < plingUpDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / plingUpDuration;
                uiButton.transform.localScale = Vector3.Lerp(originalScale, originalScale * plingScale, t);
                yield return null;
            }

            // Scale down
            elapsed = 0f;
            while (elapsed < plingDownDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / plingDownDuration;
                uiButton.transform.localScale = Vector3.Lerp(originalScale * plingScale, originalScale, t);
                yield return null;
            }

            // Pause between plings
            yield return new WaitForSeconds(waitBetweenPlings);
        }

        // Reset scale when loop stops
        uiButton.transform.localScale = originalScale;
    }
}
