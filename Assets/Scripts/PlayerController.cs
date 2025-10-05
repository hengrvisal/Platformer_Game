using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;

    [Header("Food System")]
    private FoodItem currentFood;
    public Transform foodHoldPosition;

    [Header("Components")]
    private Rigidbody2D rb;
    private Collider2D col;
    private Animator animator;

    private bool isGrounded;
    private float horizontalInput;

    // Add this to see if script is running
    private void Start()
    {
        Debug.Log("PlayerController Started!");
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInput();
        UpdateAnimations();
        CheckGrounded();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        Debug.Log($"Input: {horizontalInput}"); // Add this to check input

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void HandleMovement()
    {
        Vector2 velocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = velocity;

        // Flip sprite based on direction
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void CheckGrounded()
    {
        if (col == null) return;

        Vector2 bottom = new Vector2(col.bounds.center.x, col.bounds.min.y);
        isGrounded = Physics2D.OverlapCircle(bottom, 0.1f, groundLayer);
    }

    private void UpdateAnimations()
    {
        if (animator != null)
        {
            animator.SetBool("IsRunning", horizontalInput != 0);
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);
            animator.SetBool("HasFood", currentFood != null);
        }
    }

    public void CollectFood(FoodItem food)
    {
        if (currentFood != null) return;

        currentFood = food;
        if (foodHoldPosition != null && food != null)
        {
            food.transform.SetParent(foodHoldPosition);
            food.transform.localPosition = Vector3.zero;
        }

        GameManager.Instance.AddPoints(food.pointsValue);
    }

    public bool HasFood(string foodType)
    {
        return currentFood != null && currentFood.foodType == foodType;
    }

    public void DeliverFood(string foodType)
    {
        if (currentFood != null && currentFood.foodType == foodType)
        {
            Destroy(currentFood.gameObject);
            currentFood = null;
        }
    }
}