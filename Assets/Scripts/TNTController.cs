using UnityEngine;

public class TNTController : MonoBehaviour
{
    public float radius = 5f;
    public float damage = 50;
    [SerializeField]
    private float damageVariation = 0.1f;
    [SerializeField]
    private float radiusVariation = 0.1f;

    private void Start()
    {
        damage += damage * Random.Range(-damageVariation, damageVariation);
        radius += radius * Random.Range(-radiusVariation, radiusVariation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliders)
        {
            //if it has rigidbody, apply knockback
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (collider.transform.position - transform.position).normalized;
                float distance = Vector2.Distance(collider.transform.position, transform.position);
                float dFactor = 1f - (distance / radius);
                rb.AddForce(direction * dFactor * 10f, ForceMode2D.Impulse);
            }
            if (collider.CompareTag("Player"))
            {
                HealthController healthController = collider.GetComponent<HealthController>();
                if (healthController != null)
                {
                    healthController.health -= damage;
                }
            }
        }

        GetComponent<Collider2D>().enabled = false;
        GameManager.instance.cameraController.Shake(1f, 0.2f);
        GameManager.instance.volumeExpoLerper.ChangeFromTo(5f, 0f, 0.2f, () =>
        {
            Destroy(gameObject);
        });
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
