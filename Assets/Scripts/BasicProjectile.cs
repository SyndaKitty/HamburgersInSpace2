using UnityEngine;

public class BasicProjectile : MonoBehaviour, Projectile
{
    public float Damage;
    public Team Team;
    public bool Inert;
    public float SourceSpeedTransfer = 0.5f;
    public float FadeTime = 0.5f;
    
    SpriteRenderer sr;
    Rigidbody2D rb;
    
    float t;
    public OneShotSound OnImpactSound;
    public OneShotSound OnShieldSound;

    public float Weight { get; set; }

    public void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        Init();
    }

    public void Init() { }
    

    public void Fire(Vector2 velocity, float speedModifier, Vector2 sourceVelocity)
    {
        float intendedAngle = Mathf.Atan2(velocity.y, velocity.x);
        rb.velocity += sourceVelocity * SourceSpeedTransfer;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Inert)
        {
            t += Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f - t / FadeTime);
            if (t >= FadeTime)
            {
                Die();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (Inert)
        {
            return;
        }

        var enemy = collision.collider.GetComponent<EnemyController>();
        if (enemy)
        {
            enemy.Damage(Damage);
            if (OnImpactSound != null)
            {
                Instantiate(OnImpactSound);
            }
        }

        var entity = collision.collider.GetComponent<EntityController>();
        if (entity)
        {
            entity.Damage(Damage);
            if (OnImpactSound != null)
            {
                Instantiate(OnImpactSound);
            }
        }

        if (collision.collider.CompareTag("Shield"))
        {
            if (OnShieldSound)
            {
                Instantiate(OnShieldSound);
            }
            Inert = true;
            t = 0;
            return;
        }

        var player = collision.collider.GetComponent<PlayerController>();
        if (player)
        {
            player.Damage(Damage);
            if (OnImpactSound != null)
            {
                Instantiate(OnImpactSound);
            }
        }

        Die();
    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    void Projectile.Damage(EnemyController enemy)
    {
        throw new System.NotImplementedException();
    }

    void Projectile.Update()
    {
        throw new System.NotImplementedException();
    }

    public void Destroy()
    {
        throw new System.NotImplementedException();
    }
}