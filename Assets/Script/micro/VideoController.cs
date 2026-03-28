using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    [Header("Play / Pause UI")]
    public TextMeshProUGUI buttonText;
    public Image playPauseButtonImage;
    public Sprite playSprite;
    public Sprite pauseSprite;

    [Header("Time UI")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI durationText;

    [Header("Controls")]
    public Slider volumeSlider;
    public TMP_Dropdown speedDropdown;

    [Header("Front Page Stats")]
    public TextMeshProUGUI totalDurationText;
    public TextMeshProUGUI videoCountText;

    public string videoFolderName = "Videos";
    private List<string> videoPaths = new List<string>();
    private int currentVideoIndex = 0;
    private bool isPrepared = false;

    void Start()
    {
        buttonText.text = "Play";
        playPauseButtonImage.sprite = playSprite;

        LoadVideoFiles();

        if (videoPaths.Count > 0)
        {
            StartCoroutine(ShowFrontPageVideoDuration(videoPaths[currentVideoIndex]));
            StartCoroutine(ShowVideoStats());
        }
        else
        {
            Debug.LogWarning("No videos found!");
        }

        volumeSlider.onValueChanged.AddListener(SetVolume);
        speedDropdown.onValueChanged.AddListener(SetPlaybackSpeed);

        videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    void Update()
    {
        if (isPrepared && videoPlayer.isPlaying)
            UpdateVideoInfo();
    }

    // ---------------------- PLAY / PAUSE -----------------------

    public void TogglePlayPause()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            buttonText.text = "Play";
            playPauseButtonImage.sprite = playSprite;   // 🔹 Switch to Play icon
        }
        else
        {
            videoPlayer.Play();
            buttonText.text = "Pause";
            playPauseButtonImage.sprite = pauseSprite;  // 🔹 Switch to Pause icon
        }
    }

    public void OnBackButtonPressed()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            buttonText.text = "Play";
            playPauseButtonImage.sprite = playSprite; // 🔹 Switch to Play icon
        }
    }

    // ---------------------- TIME UPDATES -----------------------

    void OnVideoPrepared(VideoPlayer vp)
    {
        isPrepared = true;
        UpdateVideoInfo();

        double totalTime = videoPlayer.length;
        durationText.text = "Duration: " + FormatMinutes(totalTime);
    }

    void UpdateVideoInfo()
    {
        double current = videoPlayer.time;
        double total = videoPlayer.length;
        timeText.text = $"{FormatMinutes(current)} / {FormatMinutes(total)}";
    }

    string FormatMinutes(double time)
    {
        int m = Mathf.FloorToInt((float)time / 60f);
        return $"{m} Mins";
    }

    // ---------------------- VIDEO LOADING -----------------------

    void LoadVideoFiles()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, videoFolderName);

        if (Directory.Exists(folderPath))
        {
            foreach (string file in Directory.GetFiles(folderPath))
            {
                string ext = Path.GetExtension(file).ToLower();
                if (ext == ".mp4" || ext == ".mov" || ext == ".avi" || ext == ".mkv")
                    videoPaths.Add(file);
            }
        }
    }

    IEnumerator ShowFrontPageVideoDuration(string videoPath)
    {
        videoPlayer.url = videoPath;
        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared);

        double totalTime = videoPlayer.length;
        durationText.text = "Duration: " + FormatMinutes(totalTime);
    }

    // ---------------------- CONTROLS -----------------------

    void SetVolume(float v)
    {
        AudioSource audio = videoPlayer.GetTargetAudioSource(0);
        if (audio != null) audio.volume = v;
    }

    void SetPlaybackSpeed(int i)
    {
        string label = speedDropdown.options[i].text.Replace("x", "");
        if (float.TryParse(label, out float spd)) videoPlayer.playbackSpeed = spd;
    }

    // ---------------------- STATS (TOTAL MINUTES + COUNT) -----------------------

    IEnumerator ShowVideoStats()
    {
        double totalDuration = 0;

        foreach (string path in videoPaths)
        {
            VideoPlayer tempPlayer = gameObject.AddComponent<VideoPlayer>();
            tempPlayer.source = VideoSource.Url;
            tempPlayer.url = path;
            tempPlayer.playOnAwake = false;
            tempPlayer.Prepare();

            yield return new WaitUntil(() => tempPlayer.isPrepared);
            totalDuration += tempPlayer.length;
            Destroy(tempPlayer);
        }

        int totalMinutes = Mathf.FloorToInt((float)totalDuration / 60f);
        totalDurationText.text = $"Duration: {totalMinutes} Mins";
        videoCountText.text = $"Videos: {videoPaths.Count:00}";
    }
}
