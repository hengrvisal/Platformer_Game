// GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { MainMenu, LevelSelect, Gameplay, Pause, GameOver, Win }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState CurrentState { get; private set; }

    [Header("Level Progression")]
    public int totalPoints = 0;
    public bool[] levelUnlocked = new bool[4] { true, false, false, false };
    public int[] levelUnlockThresholds = new int[3] { 0, 50, 100 };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        HandleStateChange();
    }

    private void HandleStateChange()
    {
        switch (CurrentState)
        {
            case GameState.Gameplay:
                Time.timeScale = 1f;
                break;
            case GameState.Pause:
                Time.timeScale = 0f;
                break;
            case GameState.Win:
                UnlockNextLevel();
                break;
        }
    }

    public void AddPoints(int points)
    {
        totalPoints += points;
        CheckLevelUnlocks();
    }

    private void CheckLevelUnlocks()
    {
        for (int i = 1; i < levelUnlocked.Length; i++)
        {
            if (totalPoints >= levelUnlockThresholds[i])
            {
                levelUnlocked[i] = true;
            }
        }
    }

    private void UnlockNextLevel()
    {
        int currentLevel = GetCurrentLevel();
        if (currentLevel < levelUnlocked.Length - 1)
        {
            levelUnlocked[currentLevel + 1] = true;
        }
    }

    private int GetCurrentLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.Contains("Level"))
        {
            return int.Parse(sceneName.Replace("Level", ""));
        }
        return 1;
    }
}