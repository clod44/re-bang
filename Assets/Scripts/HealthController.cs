using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float maxHealth = 100f;
    [SerializeField]
    private float _health;
    public float health
    {
        get
        {
            return _health;
        }
        set
        {
            if (value < _health)
            {
                _health = Mathf.Clamp(value, 0f, maxHealth);
                TakeDamage();
            }
            else
            {
                _health = Mathf.Clamp(value, 0f, maxHealth);
            }
            if (OnHealthChange != null)
            {
                OnHealthChange(_health, maxHealth);
            }
        }
    }
    public System.Action OnTakeDamage;
    public System.Action OnDie;

    public delegate void HealthChangeDelegate(float newHealth, float maxHealth);
    public HealthChangeDelegate OnHealthChange;

    public bool isDead = false;

    void Start()
    {
        Reset();
    }

    void TakeDamage()
    {
        if (OnTakeDamage != null)
        {
            OnTakeDamage();
        }
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        if (OnDie != null)
        {
            OnDie();
        }
    }

    public void Reset()
    {
        health = maxHealth;
    }
}
