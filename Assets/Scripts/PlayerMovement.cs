using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float jumpDelay = 0.05f;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private bool isGrounded = false;
    private float lastJumpTime = 0f;

    [SerializeField]
    private Transform groundCheck;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);

        isGrounded = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider != playerCollider && collider.enabled)
            {
                isGrounded = true;
                break;
            }
        }

        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.AddForce(moveInput * moveSpeed * Time.fixedDeltaTime * Vector2.right, ForceMode2D.Force);

        if (isGrounded && Time.time - lastJumpTime >= jumpDelay && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastJumpTime = Time.time; // Update last jump time
        }
    }
}
