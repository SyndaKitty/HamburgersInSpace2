using Assets.Scripts;
using UnityEngine;

public class PlayerController : Entity
{
    public float Speed;
    public float Acceleration;

    public float DashCooldown;
    public float DashSpeed;
    public float DashAcceleration;
    public float DashLength;

    public float ShootingSpeed;
    public float BulletSpeedModifier = 1f;
    public float BlockingPenalty = 0.5f;

    public float Deadzone = 0.3f;
    public OneShotSound OneShotFire;

    public Vector2 AimDirection;
    public Vector2 MoveDirection;

    Rigidbody2D rb;
    AudioSource source;
    Collider2D col;
    Camera cam;
    SpriteRenderer sr;

    Transform healthBarInner;
    SpriteRenderer healthBarHolderSr;
    SpriteRenderer healthBarInnerSr;

    Transform bun;

    ExplosionCreator explosion;
    Animator animator;

    const string HitTrigger = "Hit";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        explosion = GetComponent<ExplosionCreator>();
        animator = GetComponent<Animator>();
        cam = Camera.main;
        source = GetComponent<AudioSource>();
        healthBarInner = transform.Find("HealthbarHolder/HealthbarInner");
        healthBarInnerSr = transform.Find("HealthbarHolder/HealthbarInner").gameObject.GetComponent<SpriteRenderer>();
        healthBarHolderSr = transform.Find("HealthbarHolder").gameObject.GetComponent<SpriteRenderer>();

        Init();
    }

    void Update()
    {
        MoveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        AimDirection = new Vector2(Input.GetAxisRaw("RightStickHorizontal"), Input.GetAxisRaw("RightStickVertical"));
    }

    void FixedUpdate()
    {
        var targetVelocity = MoveDirection * Speed;
        var currentVelocity = rb.velocity;

        var diff = (targetVelocity - currentVelocity);
        rb.AddForce(Acceleration * diff);
    }

    public Vector2 GetVelocity() => rb.velocity;
    public void Respawn()
    {
        rb.velocity = Vector2.zero;
        sr.enabled = true;
        healthBarInnerSr.enabled = true;
        healthBarHolderSr.enabled = true;
        col.enabled = true;
        enabled = true;
        Health = MaxHealth;
    }

    public void Push(Vector2 amount)
    {
        rb.AddForce(amount, ForceMode2D.Impulse);
    }

    public override void Damage(float damage)
    {
        animator.SetTrigger(HitTrigger);
        Health -= damage;
    }

    public override void HealthChanged(float health)
    {
        if (health <= 0)
        {
            Die();
        }

        var scale = healthBarInner.localScale;
        scale.x = Health / MaxHealth;
        healthBarInner.localScale = scale;
    }
        
    public override void Die()
    {
        rb.velocity = Vector2.zero;
        sr.enabled = false;
        healthBarInnerSr.enabled = false;
        healthBarHolderSr.enabled = false;
        col.enabled = false;
        enabled = false;
        explosion.Create();
        EventBus.PlayerDied();

        Destroy(gameObject);
    }
}
