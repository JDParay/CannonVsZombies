using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text endText;

    private int score = 0;
    public int CurrentScore => score;

    void Awake()
    {
        instance = this;
    }

    public void AddScore(int value)
    {
        score += value;
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        // Clamp score so it doesn't go below 0
        if (score < 0)
            score = 0;
    }

    public void ShowEndMessage(string message)
    {
        if (endText != null)
        {
            endText.gameObject.SetActive(true);
            endText.text = message;
        }

        Time.timeScale = 0f;
    }
}
