using UnityEngine;

public class PlayerMovementL4 : MonoBehaviour
{
    public Rigidbody2D body;
    float horizontalMovement;

    [SerializeField] float speed = 6f;
    [SerializeField] float jumpForce = 12f;

    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.4f, 0.05f);
    public LayerMask groundLayer;

    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    Animator anim;

    void Awake()
    {
        if (!body) body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        body.freezeRotation = true;
        if (groundLayer.value == 0) groundLayer = Physics2D.AllLayers;
    }

    void Update()
    {
        float axis = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(axis) > 0.01f) horizontalMovement = axis;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            body.velocity = new Vector2(body.velocity.x, jumpForce);

        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0f)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.5f);

        if (horizontalMovement > 0.01f) transform.localScale = new Vector3(3, 3, 3);
        else if (horizontalMovement < -0.01f) transform.localScale = new Vector3(-3, 3, 3);

        if (anim)
        {
            TrySetFloat(anim, "yVelocity", body.velocity.y);
            TrySetFloat(anim, "magnitude", body.velocity.magnitude);
        }
    }

    void FixedUpdate()
    {
        body.velocity = new Vector2(horizontalMovement * speed, body.velocity.y);

        // custom gravity / terminal velocity
        if (body.velocity.y < 0f)
        {
            body.gravityScale = baseGravity * fallSpeedMultiplier;
            body.velocity = new Vector2(body.velocity.x, Mathf.Max(body.velocity.y, -maxFallSpeed));
        }
        else
        {
            body.gravityScale = baseGravity;
        }

        horizontalMovement = 0f;
    }

    bool IsGrounded()
    {
        if (!groundCheckPos) return false;
        return Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0f, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        if (!groundCheckPos) return;
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    }

    static void TrySetFloat(Animator a, string param, float value)
    {
        foreach (var p in a.parameters)
            if (p.name == param && p.type == AnimatorControllerParameterType.Float)
            { a.SetFloat(param, value); return; }
    }
}
