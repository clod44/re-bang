using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    void Start()
    {
    }

    void Update()
    {
        //flip sprite
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spriteRenderer.flipX = mousePosition.x < transform.position.x;
    }

    public void FreezePlayer(bool freeze)
    {
        GetComponent<Collider2D>().enabled = !freeze;
        GetComponent<Rigidbody2D>().isKinematic = freeze;
        if (freeze)
        {
            //reset rigidbody's velocity etc
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().angularVelocity = 0f;

        }
    }
}
