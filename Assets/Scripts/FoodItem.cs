// FoodItem.cs
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    [Header("Food Settings")]
    public string foodType;
    public int pointsValue = 10;

    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            CollectFood(other.GetComponent<PlayerController>());
        }
    }

    private void CollectFood(PlayerController player)
    {
        isCollected = true;
        player.CollectFood(this);
        gameObject.SetActive(false);
    }
}