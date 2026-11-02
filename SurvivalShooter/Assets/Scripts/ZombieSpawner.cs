
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameManager gameManager;
    public UIManager uiManager;
    public Zombie[] zombiePrefabs;
    public ZombieData[] zombieDatas;
    public Transform[] spawnPoints;

    private readonly List<Zombie> zombies = new List<Zombie>();
    private int wave = 0;

    private void Start()
    {
        if (gameManager == null)
        {
            GameObject findGo = GameObject.FindWithTag("GameController");
            if (findGo != null) gameManager = findGo.GetComponent<GameManager>();
        }
        if (uiManager == null && gameManager != null)
        {
            uiManager = gameManager.uIManager;
        }
    }

    private void Update()
    {
        if (gameManager != null && gameManager.IsGameOver) return;

        if (zombies.Count == 0 && spawnPoints.Length > 0 && zombiePrefabs.Length > 0)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        wave++;
        int count = Mathf.RoundToInt(wave * 1.5f);
        for (int i = 0; i < count; ++i)
        {
            CreateZombie();
        }

        if (uiManager != null) uiManager.SetWaveText(wave, zombies.Count);
    }

    private void CreateZombie()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Zombie selectedPrefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];

        Zombie zombie = Instantiate(selectedPrefab, point.position, point.rotation);
        if (zombieDatas != null && zombieDatas.Length > 0)
        {
            ZombieData data = zombieDatas[Random.Range(0, zombieDatas.Length)];
            zombie.Setup(data);
        }

        zombies.Add(zombie);

        zombie.OnDead.AddListener(() => zombies.Remove(zombie));
        zombie.OnDead.AddListener(() => { if (uiManager != null) uiManager.SetWaveText(wave, zombies.Count); });
        zombie.OnDead.AddListener(() => { if (gameManager != null) gameManager.AddScore(50); });
        zombie.OnDead.AddListener(() => Destroy(zombie.gameObject, 2f));
    }

    public void OnEndGame()
    {
        for (int i = zombies.Count - 1; i >= 0; --i)
        {
            if (zombies[i] != null) zombies[i].Die();
        }
    }
}