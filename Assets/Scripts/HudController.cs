using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI foodCarriedText;
    public Slider healthBar;
    public GameObject pausePanel;

    private PlayerController player;
    private HealthSystem playerHealth;

    private void Start()
    {
        // Find player if not assigned
        if (player == null)
            player = FindObjectOfType<PlayerController>();

        if (player != null)
        {
            playerHealth = player.GetComponent<HealthSystem>();
        }

        UpdateUI();

        // Make sure GameManager exists
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager instance not found!");
        }
    }

    private void Update()
    {
        UpdateUI();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void UpdateUI()
    {
        // Update score
        if (scoreText != null && GameManager.Instance != null)
        {
            scoreText.text = $"Points: {GameManager.Instance.totalPoints}";
        }

        // Update health
        if (playerHealth != null && healthBar != null)
        {
            healthBar.value = (float)playerHealth.currentHealth / playerHealth.maxHealth;
        }

        // Update food carried text
        if (foodCarriedText != null && player != null)
        {
            // We'll need to modify PlayerController to expose current food
            foodCarriedText.text = "Food: None"; // Placeholder
        }
    }

    public void TogglePause()
    {
        if (GameManager.Instance.CurrentState == GameState.Gameplay)
        {
            GameManager.Instance.ChangeState(GameState.Pause);
            if (pausePanel != null) pausePanel.SetActive(true);
        }
        else if (GameManager.Instance.CurrentState == GameState.Pause)
        {
            GameManager.Instance.ChangeState(GameState.Gameplay);
            if (pausePanel != null) pausePanel.SetActive(false);
        }
    }
}