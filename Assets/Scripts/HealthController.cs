using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float maxHealth = 100f;
    //set get health
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
        }
    }
    public System.Action OnTakeDamage;
    public System.Action OnDie;

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
    }

    void Die()
    {
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
