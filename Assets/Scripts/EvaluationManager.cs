using System.Collections.Generic;
using UnityEngine;

public class EvaluationManager : MonoBehaviour
{
    public static EvaluationManager Instance;

    public int totalQuestions;
    public int totalScore;

    [System.Serializable]
    public class QuestionState
    {
        public bool answered;
        public List<int> wrongOptions;
        public int correctOption;
        public int scoreAwarded;

        public QuestionState()
        {
            answered = false;
            wrongOptions = new List<int>();
            correctOption = -1;
            scoreAwarded = 0;
        }
    }

    public QuestionState[] questionStates;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        questionStates = new QuestionState[totalQuestions];
        for (int i = 0; i < totalQuestions; i++)
            questionStates[i] = new QuestionState();
    }

    public void RegisterQuestionCompletion(int score)
    {
        totalScore += score;
    }
}