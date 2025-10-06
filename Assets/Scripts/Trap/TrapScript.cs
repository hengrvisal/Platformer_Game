using System.Runtime.InteropServices;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    public float bounceForce = 10f;
    public int damage = 1;
    public Vector2 knockback = new Vector2(6f, 4f); // push & pop

    void Reset()
    {
        // Ensure this collider is a trigger so OnTriggerEnter2D fires
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void HandlePlayerBounce(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Animator anim = player.GetComponent<Animator>();
        Debug.Log("Got player Object");

        if (rb)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            float dir = Mathf.Sign(other.transform.position.x - transform.position.x);
            if (dir == 0) dir = 1f;
            rb.AddForce(new Vector2(knockback.x * dir, knockback.y), ForceMode2D.Impulse);
        }
    }
}
