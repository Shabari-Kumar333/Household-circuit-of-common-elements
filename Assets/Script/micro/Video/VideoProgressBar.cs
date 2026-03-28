using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System.IO;
using System.Linq;

public class VideoProgressBar : MonoBehaviour
{
    [Header("Video Player")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Slider progressSlider;

    [Header("Time Texts")]
    [SerializeField] private TMP_Text[] timeTexts;

    [Header("Skip Settings")]
    [SerializeField] private double skipSeconds = 10;

    [Header("Video Folder Settings")]
    [SerializeField] private string videoFolderPath;
    [SerializeField] private TMP_Text[] videoCountTexts;

    [Header("Final Page UI")]
    [SerializeField] private GameObject finalPage;
    [SerializeField] private TMP_Text completionText;

    private bool isDragging = false;
    private bool isVideoCompleted = false;

    void Start()
    {
        progressSlider.minValue = 0;
        progressSlider.maxValue = 1;

        UpdateVideoCount();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void Update()
    {
        if (videoPlayer.frameCount <= 0) return;

        if (!isDragging)
        {
            float progress = (float)(videoPlayer.time / videoPlayer.length);
            progressSlider.value = progress;
        }

        // ✨ Show current time and total time
        UpdateCurrentAndTotalTimeTexts();
    }

    public void OnSliderDragStart() => isDragging = true;

    public void OnSliderDragEnd()
    {
        videoPlayer.time = progressSlider.value * videoPlayer.length;
        isDragging = false;
    }

    /// <summary>
    /// ⏳ Shows "current time / total time"
    /// </summary>
    private void UpdateCurrentAndTotalTimeTexts()
    {
        double currentTime = videoPlayer.time;
        double totalTime = videoPlayer.length;
        string timeString = FormatTime(currentTime) + " / " + FormatTime(totalTime);

        foreach (TMP_Text t in timeTexts)
        {
            if (t != null) t.text = timeString;
        }
    }

    /// <summary>
    /// ⏱ Shows total video duration in minutes (e.g. "3 mins")
    /// </summary>
    private void UpdateTotalMinutesOnlyTexts()
    {
        double totalTime = videoPlayer.length;
        string timeString = $"{FormatMinutes(totalTime)} mins";

        foreach (TMP_Text t in timeTexts)
        {
            if (t != null) t.text = timeString;
        }
    }

    private string FormatTime(double time)
    {
        int minutes = Mathf.FloorToInt((float)time / 60F);
        int seconds = Mathf.FloorToInt((float)time - minutes * 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private int FormatMinutes(double time)
    {
        return Mathf.CeilToInt((float)time / 60f);
    }

    public void SkipBackward()
    {
        double newTime = videoPlayer.time - skipSeconds;
        if (newTime < 0) newTime = 0;
        videoPlayer.time = newTime;
    }

    public void SkipForward()
    {
        double newTime = videoPlayer.time + skipSeconds;
        if (newTime > videoPlayer.length) newTime = videoPlayer.length;
        videoPlayer.time = newTime;
    }

    public void UpdateVideoCount()
    {
        if (string.IsNullOrEmpty(videoFolderPath) || !Directory.Exists(videoFolderPath))
        {
            foreach (var t in videoCountTexts)
            {
                if (t != null) t.text = "0 Videos";
            }
            return;
        }

        string[] supportedExtensions = { ".mp4", ".mov", ".avi", ".mkv", ".webm" };
        string[] files = Directory.GetFiles(videoFolderPath);

        int videoCount = files.Count(file => supportedExtensions.Contains(Path.GetExtension(file).ToLower()));

        foreach (var t in videoCountTexts)
        {
            if (t != null) t.text = videoCount + " Videos";
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        if (isVideoCompleted) return;
        isVideoCompleted = true;

        double total = videoPlayer.length;
        double watched = videoPlayer.time;
        float percentage = (float)((watched / total) * 100f);

        if (completionText != null)
            completionText.text = $"Video Completed: {percentage:F1}%";

        if (finalPage != null)
            finalPage.SetActive(true);
    }
}
