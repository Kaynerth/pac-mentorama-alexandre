using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _currentScore;
    private int _highScore;
    private int _scoreMultiplier = 1;

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

        var allGhosts = FindObjectsOfType<GhostAI>();
        foreach (var ghost in allGhosts)
        {
            ghost.OnGhostCaptured += Ghost_OnGhostCaptured;
            ghost.OnVulnerabilityFade += GhostComponent_OnVulnerabilityFade;
        }
    }

    private void GhostComponent_OnVulnerabilityFade()
    {
        _scoreMultiplier = 1;
    }

    private void Ghost_OnGhostCaptured(int score, GhostAI ghost)
    {
        _currentScore += score * _scoreMultiplier;
        OnScoreChange?.Invoke(_currentScore);
        _scoreMultiplier++;

        if (_currentScore >= _highScore)
        {
            _highScore = _currentScore;
            OnHighScoreChange?.Invoke(_highScore);
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
