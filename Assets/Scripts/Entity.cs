using UnityEngine;

public abstract class Entity :MonoBehaviour
{
    public float MaxHealth;

    float _health;
    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (value <= 0)
            {
                Die();
            }
            HealthChanged(value);
        }
    }
    
    public void Init()
    {
        Health = MaxHealth;
    }

    public abstract void HealthChanged(float health);
    public abstract void Damage(float damage);
    public abstract void Die();
    public abstract void Push(Vector2 force);
}