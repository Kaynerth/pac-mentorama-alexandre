using System;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    public bool IsVictoryCondition;
    public int Score;

    public event Action<int, Collectables> OnCollected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnCollected?.Invoke(Score, this);
        Destroy(gameObject);
    }
}
