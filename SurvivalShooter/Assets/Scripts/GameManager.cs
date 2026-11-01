// 파일명: GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UIManager uIManager;
    public ZombieSpawner zombieSpawner;
    
    [field: SerializeField] public string SceneName { get; private set; } = "SampleScene";

    private int score = 0;
    public bool IsGameOver { get; private set; }

    private void Start()
    {
        if (uIManager != null) uIManager.SetScore(score);
    }

    public void AddScore(int add)
    {
        if (IsGameOver) return;
        score += add;
        if (uIManager != null) uIManager.SetScore(score);
    }

    public void EndGame()
    {
        IsGameOver = true;
        if (uIManager != null) uIManager.SetActiveGameOverUI(true);
        if (zombieSpawner != null) zombieSpawner.OnEndGame();
    }

    public void RestartGame() => SceneManager.LoadScene(SceneName);
}