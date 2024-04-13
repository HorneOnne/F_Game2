using UnityEngine;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<LevelDataSO> Levels;
    public LevelDataSO CurrentLevel;

    private void Awake()
    {
        // Check if an instance already exists, and destroy the duplicate
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // FPS
        Application.targetFrameRate = 60;

        CurrentLevel = Levels[0];
    }

    private void Start()
    {
        // Make the GameObject persist across scenes
        DontDestroyOnLoad(this.gameObject);
    }

    public void RandomLevel()
    {
        CurrentLevel = Levels[Random.Range(0, Levels.Count)];
    }

 
}

