using UnityEngine;
using UnityEngine.Events;

public class KidNPC : MonoBehaviour
{
    [Header("Kid Settings")]
    public string requiredFoodType = "Apple";
    public bool isHungry = true;
    public int pointsReward = 25;

    [Header("Visual Feedback")]
    public Color hungryColor = Color.red;
    public Color fedColor = Color.green;
    public GameObject happyParticles;

    [Header("Events")]
    public UnityEvent OnFed;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisuals();

        // Remove any Rigidbody to prevent movement
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) Destroy(rb);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isHungry && other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && player.HasFood(requiredFoodType))
            {
                FeedKid(player);
            }
        }
    }

    private void FeedKid(PlayerController player)
    {
        player.DeliverFood(requiredFoodType);
        isHungry = false;

        GameManager.Instance.AddPoints(pointsReward);

        UpdateVisuals();
        if (happyParticles != null)
            Instantiate(happyParticles, transform.position, Quaternion.identity);

        OnFed?.Invoke();

        CheckLevelCompletion();
    }

    private void UpdateVisuals()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isHungry ? hungryColor : fedColor;
        }
    }

    private void CheckLevelCompletion()
    {
        KidNPC[] allKids = FindObjectsOfType<KidNPC>();
        bool allFed = true;

        foreach (KidNPC kid in allKids)
        {
            if (kid.isHungry)
            {
                allFed = false;
                break;
            }
        }

        if (allFed)
        {
            Debug.Log("LEVEL COMPLETE! All kids fed!");
            GameManager.Instance.ChangeState(GameState.Win);
        }
    }
}