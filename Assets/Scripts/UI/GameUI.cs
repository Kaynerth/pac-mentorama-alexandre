using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject ReadyMessage;
    public GameObject GameOverMessage;
    public BlinkTilemapColor BlinkTilemap;
    public AudioSource AudioSource;
    public AudioClip BeginningMusic;
    public GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _gameManager.OnGameStarted += GameManager_OnGameStarted;
        _gameManager.OnGameOver += _gameManager_OnGameOver;
        _gameManager.OnVictory += GameManager_OnVictory;
        AudioSource.PlayOneShot(BeginningMusic);
    }

    private void GameManager_OnVictory()
    {
        BlinkTilemap.enabled = true;
    }

    private void GameManager_OnGameStarted()
    {
        ReadyMessage.SetActive(false);
    }

    private void _gameManager_OnGameOver()
    {
        GameOverMessage.SetActive(true);
    }
}
