using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI CurrentScoreText;
    public TextMeshProUGUI HighScoreText;

    private void Start()
    {
        var scoreManager = FindObjectOfType<ScoreManager>();
        scoreManager.OnScoreChange += ScoreManager_OnScoreChange;
        scoreManager.OnHighScoreChange += ScoreManager_OnHighScoreChange;

        HighScoreText.text = $"{scoreManager.HighScore:00}";
    }

    private void ScoreManager_OnHighScoreChange(int highScore)
    {
        HighScoreText.text = $"{highScore:00}";
    }

    private void ScoreManager_OnScoreChange(int score)
    {
        CurrentScoreText.text = $"{score:00}";
    }
}
