using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MCQController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mcqPanel;
    public GameObject explanationPanel;

    [Header("Question UI")]
    public TMP_Text questionText;
    public Image referenceImage;

    [Header("Options")]
    public Button[] optionButtons;
    public TMP_Text[] optionTexts;

    [Header("Explanation UI")]
    public TMP_Text explanationText;
    public Button explanationActionButton;

    [Header("Explanation Entry Buttons")]
    public Button rightExplanationButton;
    public Button wrongExplanationButton;

    [Header("Sprites")]
    public Sprite defaultButtonSprite;
    public Sprite correctSprite;
    public Sprite wrongSprite;

    [Header("Data")]
    public MCQQuestionData questionData;
    public SlideManager slideManager;

    [Header("Scoring")]
    public int maxScore = 5;
    public int minScore = 2;

    private int attemptCount;

    // -------------------------------------------------

    private void OnEnable()
    {
        LoadQuestion();
        BindButtons();
        RestoreState();
        UpdateNavigationForReview();
    }

    // -------------------------------------------------
    // NAVIGATION (FIXED)

    void UpdateNavigationForReview()
    {
        int slideIndex = slideManager.CurrentSlideIndex;
        var state = EvaluationManager.Instance.questionStates[slideIndex];

        bool isReviewSlide =
            state.answered ||
            state.correctOption != -1 ||
            state.wrongOptions.Count > 0;

        slideManager.btnNext.interactable = isReviewSlide;
        slideManager.btnPrev.interactable = true;
    }

    // -------------------------------------------------
    // LOAD

    void LoadQuestion()
    {
        questionText.text = questionData.questionText;
        explanationText.text = questionData.explanationText;

        if (referenceImage != null)
        {
            if (questionData.referenceImage != null)
            {
                referenceImage.gameObject.SetActive(true);
                referenceImage.sprite = questionData.referenceImage;
                referenceImage.color = Color.white;
            }
            else
            {
                referenceImage.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionTexts[i].text = questionData.options[i];
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
            optionButtons[i].interactable = true;
            optionButtons[i].GetComponent<Image>().sprite = defaultButtonSprite;
        }

        mcqPanel.SetActive(true);
        explanationPanel.SetActive(false);
        HideExplanationButtons();
    }

    // -------------------------------------------------
    // RESTORE (FULL HISTORY)

    void RestoreState()
    {
        int slideIndex = slideManager.CurrentSlideIndex;
        var state = EvaluationManager.Instance.questionStates[slideIndex];

        // Restore ALL wrong attempts
        foreach (int wrongIndex in state.wrongOptions)
        {
            if (wrongIndex >= 0 && wrongIndex < optionButtons.Length)
            {
                optionButtons[wrongIndex]
                    .GetComponent<Image>().sprite = wrongSprite;

                optionButtons[wrongIndex].interactable = false;
            }
        }

        // Restore correct answer
        if (state.correctOption != -1)
        {
            optionButtons[state.correctOption]
                .GetComponent<Image>().sprite = correctSprite;

            DisableAllOptions();
            ShowRightExplanation();
        }
    }

    // -------------------------------------------------
    // ANSWER LOGIC (FIXED)

    void OnOptionSelected(int index)
    {
        int slideIndex = slideManager.CurrentSlideIndex;
        var state = EvaluationManager.Instance.questionStates[slideIndex];

        bool isCorrect = index == questionData.correctOptionIndex;

        if (isCorrect)
        {
            state.correctOption = index;
            state.answered = true;

            int score = Mathf.Max(maxScore - attemptCount, minScore);
            state.scoreAwarded = score;
            EvaluationManager.Instance.RegisterQuestionCompletion(score);

            optionButtons[index].GetComponent<Image>().sprite = correctSprite;
            DisableAllOptions();
            ShowRightExplanation();
            UnlockNavigation();
        }
        else
        {
            attemptCount++;

            // 🔥 STORE HISTORY (NOT OVERWRITE)
            if (!state.wrongOptions.Contains(index))
                state.wrongOptions.Add(index);

            optionButtons[index].GetComponent<Image>().sprite = wrongSprite;
            optionButtons[index].interactable = false;
            ShowWrongExplanation();
        }
    }

    // -------------------------------------------------
    // EXPLANATION FLOW

    void BindButtons()
    {
        rightExplanationButton.onClick.RemoveAllListeners();
        wrongExplanationButton.onClick.RemoveAllListeners();
        explanationActionButton.onClick.RemoveAllListeners();

        rightExplanationButton.onClick.AddListener(OpenExplanation);
        wrongExplanationButton.onClick.AddListener(OpenExplanation);
        explanationActionButton.onClick.AddListener(OnExplanationAction);
    }

    void OpenExplanation()
    {
        mcqPanel.SetActive(false);
        explanationPanel.SetActive(true);
    }

    void OnExplanationAction()
    {
        explanationPanel.SetActive(false);

        int slideIndex = slideManager.CurrentSlideIndex;
        var state = EvaluationManager.Instance.questionStates[slideIndex];

        if (state.answered)
            slideManager.ChangeSlideExternally(1);
        else
        {
            mcqPanel.SetActive(true);
            HideExplanationButtons();
        }
    }

    // -------------------------------------------------
    // UI HELPERS

    void DisableAllOptions()
    {
        foreach (var btn in optionButtons)
            btn.interactable = false;
    }

    void HideExplanationButtons()
    {
        rightExplanationButton.gameObject.SetActive(false);
        wrongExplanationButton.gameObject.SetActive(false);
    }

    void ShowRightExplanation()
    {
        rightExplanationButton.gameObject.SetActive(true);
        wrongExplanationButton.gameObject.SetActive(false);
    }

    void ShowWrongExplanation()
    {
        wrongExplanationButton.gameObject.SetActive(true);
        rightExplanationButton.gameObject.SetActive(false);
    }

    void UnlockNavigation()
    {
        slideManager.btnNext.interactable = true;
        slideManager.btnPrev.interactable = true;
    }
}
