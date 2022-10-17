using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private enum GameStates
    {
        Starting,
        Playing,
        LifeLost,
        GameOver,
        Victory
    }

    public float StartUpTime;

    public float LifeLostTimer;

    private GhostAI[] _allGhosts;
    private CharacterMotor _pacmanMotor;
    private GhostHouse _ghostHouse;

    private GameStates _gameState;
    private int _victoryCount;

    private float _lifeLostTimer;
    private bool _isGameOver;

    public event Action OnGameStarted;
    public event Action OnVictory;
    public event Action OnGameOver;

    private void Start()
    {
        var AllCollectables = FindObjectsOfType<Collectables>();

        _victoryCount = 0;
        foreach (var collectable in AllCollectables)
        {
            _victoryCount++;
            collectable.OnCollected += Collectable_OnCollected;
        }

        var pacman = GameObject.FindWithTag("Player");
        _pacmanMotor = pacman.GetComponent<CharacterMotor>();
        _allGhosts = FindObjectsOfType<GhostAI>();
        StopAllCharacters();

        _ghostHouse = FindObjectOfType<GhostHouse>();
        _ghostHouse.enabled = false;

        pacman.GetComponent<Life>().OnLifeRemoved += Pacman_OnLifeRemoved; ;

        _gameState = GameStates.Starting;
    }

    private void Pacman_OnLifeRemoved(int remainingLives)
    {
        StopAllCharacters();

        _lifeLostTimer = LifeLostTimer;
        _gameState = GameStates.LifeLost;

        _isGameOver = remainingLives <= 0;
    }

    private void Collectable_OnCollected(int _, Collectables collectable)
    {
        _victoryCount--;
        if (_victoryCount <= 0)
        {
            _gameState = GameStates.Victory;
            StopAllCharacters();
            OnVictory?.Invoke();
        }

        collectable.OnCollected -= Collectable_OnCollected;
    }

    void Update()
    {
        switch (_gameState)
        {
            case GameStates.Starting:
                StartUpTime -= Time.deltaTime;
                if (StartUpTime <= 0)
                {
                    _gameState = GameStates.Playing;
                    StartAllCharacters();
                    _ghostHouse.enabled = true;

                    OnGameStarted?.Invoke();
                }
                break;

            case GameStates.LifeLost:
                _lifeLostTimer -= Time.deltaTime;
                if (_lifeLostTimer <= 0)
                {
                    if (_isGameOver)
                    {
                        _gameState = GameStates.GameOver;
                        OnGameOver?.Invoke();
                    }
                    else
                    {
                        ResetAllCharacters();
                        _gameState = GameStates.Playing;
                    }
                }
                break;

            case GameStates.GameOver:
            case GameStates.Victory:
                if (Input.anyKey)
                {
                    SceneManager.LoadScene(0);
                }
                break;
        }
    }

    private void ResetAllCharacters()
    {
        _pacmanMotor.ResetPosition();

        foreach (var ghost in _allGhosts)
        {
            ghost.Reset();
        }

        StartAllCharacters();
    }

    private void StartAllCharacters()
    {
        _pacmanMotor.enabled = true;

        foreach (var ghost in _allGhosts)
        {
            ghost.StartMoving();
        }
    }

    private void StopAllCharacters()
    {
        _pacmanMotor.enabled = false;

        foreach (var ghost in _allGhosts)
        {
            ghost.StopMoving();
        }
    }
}
