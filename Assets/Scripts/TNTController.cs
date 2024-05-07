using UnityEngine;

public class TNTController : MonoBehaviour
{
    public float radius = 5f;
    [SerializeField]
    private float radiusVariation = 0.1f;
    public float damage = 50;
    [SerializeField]
    private float damageVariation = 0.1f;
    public float knockback = 10;
    [SerializeField]
    private float knockbackVariation = 0.1f;

    [SerializeField]
    private float cameraShakeStrength = 1f;
    [SerializeField]
    private float cameraShakeDuration = 0.2f;
    [SerializeField]
    private float flashStrength = 1f;
    [SerializeField]
    private float flashDuration = 0.2f;
    [SerializeField]
    private float randomExplodeDelay = 0.5f;
    [SerializeField]
    private float randomExplodeDelayVariation = 0.5f;
    private ParticleGenerator particleGenerator;
    private bool canExplode = true;


    private void Start()
    {
        damage += damage * Random.Range(-damageVariation, damageVariation);
        radius += radius * Random.Range(-radiusVariation, radiusVariation);
        knockback += knockback * Random.Range(-knockbackVariation, knockbackVariation);
        randomExplodeDelay += randomExplodeDelay * Random.Range(-randomExplodeDelayVariation, randomExplodeDelayVariation);
        particleGenerator = GetComponentInChildren<ParticleGenerator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        GameManager.instance.PlaySound("metal");
        if (collision.collider.CompareTag("Player"))
        {
            Explode();
        }
    }

    public void Explode()
    {
        if (!canExplode)
            return;
        canExplode = false;
        Invoke("ExplodeActions", randomExplodeDelay);
    }
    private void ExplodeActions()
    {
        GameManager.instance.PlaySound("explosion");
        particleGenerator.Emit("smoke", transform.position, Quaternion.identity);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (collider.transform.position - transform.position).normalized;
                float distance = Vector2.Distance(collider.transform.position, transform.position);
                float dFactor = 1f - (distance / radius);
                rb.AddForce(direction * dFactor * knockback, ForceMode2D.Impulse);
            }
            if (collider.CompareTag("Player"))
            {
                HealthController healthController = collider.GetComponent<HealthController>();
                if (healthController != null)
                    healthController.health -= damage;

            }
            TNTController tntController = collider.GetComponent<TNTController>();
            if (tntController != null)
                tntController.Explode();

        }

        GetComponent<Collider2D>().enabled = false;
        GameManager.instance.cameraController.Shake(cameraShakeStrength, cameraShakeDuration);
        GameManager.instance.volumeExpoLerper.ChangeFromTo(flashStrength, 0f, flashDuration, () =>
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
