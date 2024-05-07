using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    [SerializeField]
    private float jumpDelay = 0.1f;
    private float jumpT = 0f;
    public float groundCheckDistance = 0.2f;

    private Rigidbody2D rb;
    private bool isGrounded = false;

    // Layer mask to exclude the player layer
    private LayerMask playerLayerMask;
    private ParticleGenerator particleGenerator;

    private float moveInput = 0f;
    private float footstepRate = 10f;
    private float footstepT = 1f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerLayerMask = ~LayerMask.GetMask("Player");
        particleGenerator = GetComponentInChildren<ParticleGenerator>();
        footstepT = 1f / footstepRate;
    }


    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        footstepT -= Time.deltaTime;
        if (isGrounded && Mathf.Abs(rb.velocity.x) > 0.1f && footstepT <= 0)
        {
            GameManager.instance.PlaySound("footstep");
            particleGenerator.Emit("footstep", transform.position - new Vector3(0f, 0.5f, 0f), Quaternion.identity);
            footstepT = 1f / footstepRate;
        }

    }
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, playerLayerMask);
        isGrounded = hit.collider != null;

        rb.AddForce(moveInput * moveSpeed * Time.fixedDeltaTime * Vector2.right, ForceMode2D.Force);

        jumpT += Time.deltaTime;
        if (isGrounded && jumpT >= jumpDelay && (Input.GetKey(KeyCode.W) || Input.GetButtonDown("Jump")))
        {
            jumpT = 0f;
            Debug.Log("Jump");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
