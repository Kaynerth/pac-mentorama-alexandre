using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _currentScore;
    private int _highScore;

    public int CurrentScore { get => _currentScore; }
    public int HighScore { get => _highScore; }

    public event Action<int> OnScoreChange;
    public event Action<int> OnHighScoreChange;

    private void Awake()
    {
        _highScore = PlayerPrefs.GetInt("high-score", 0);
    }

    private void Start()
    {
        var allCollectables = FindObjectsOfType<Collectables>();
        foreach (var collectable in allCollectables)
        {
            collectable.OnCollected += Collectable_OnCollected;
        }
    }

    private void Collectable_OnCollected(int score, Collectables collectable)
    {
        _currentScore += score;
        OnScoreChange?.Invoke(_currentScore);

        if (_currentScore >= _highScore)
        {
            _highScore = _currentScore;
            OnHighScoreChange?.Invoke(_highScore);
        }

        collectable.OnCollected -= Collectable_OnCollected;
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("high-score", _highScore);
    }
}
