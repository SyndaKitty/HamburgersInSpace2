using Assets.Scripts;
using UnityEngine;

public class EnemyController : Entity
{
    public float Speed;
    public float Acceleration;

    public Transform[] Points;
    public float PointWaitTime;
    public float Radius;

    public int BurstShots;
    public float TimeBetweenBursts;
    public float TimeBetweenShots;
    public Projectile Bullet;
    public bool TrackPlayerDuringBurst;
    public bool ShootAheadOfPlayer;
    public float BulletSpeedModifier = 1f;

    public OneShotSound OneShotFire;

    public float StartingHealth;

    bool waiting;
    float health;
    Vector2 targetDirection;
    Rigidbody2D rb;
    int targetPoint;
    Animator animator;
    float timeWaitingAtPoint;

    bool bursting;
    float burstShotsTaken;
    float timeSinceBurst;
    float timeSinceShot;
    Vector2 burstDirection;

    Transform healthBarInner;
    ExplosionCreator explosion;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = StartingHealth;
        animator = GetComponent<Animator>();
        explosion = GetComponent<ExplosionCreator>();
        healthBarInner = transform.Find("HealthbarHolder/HealthbarInner");

        timeSinceBurst = TimeBetweenBursts * 0.5f;
        EventBus.EnemySpawned(this);
    }

    void Update()
    {
        if (waiting)
        {
            timeWaitingAtPoint += Time.deltaTime;
            if (timeWaitingAtPoint > PointWaitTime)
            {
                waiting = false;
                timeWaitingAtPoint = 0;
            }
        }

        if (!waiting)
        {
            if (Points == null || Points.Length == 0)
            {
                targetDirection = Vector2.zero;
            }
            else
            {
                targetDirection = Points[targetPoint].position - transform.position;
                if (targetDirection.magnitude < Radius)
                {
                    waiting = true;
                    targetPoint++;
                    targetPoint %= Points.Length;
                }
            }

        }
        else
        {
            targetDirection = Vector2.zero;
        }

        if (bursting)
        {
            if (burstShotsTaken >= BurstShots)
            {
                bursting = false;
                timeSinceBurst = 0;
                burstShotsTaken = 0;
            }
            else
            {
                timeSinceShot += Time.deltaTime;
                if (timeSinceShot >= TimeBetweenShots)
                {
                    Shoot();
                }
            }
        }
        else
        {
            timeSinceBurst += Time.deltaTime;
            var aimInfo = Aim();
            if (timeSinceBurst >= TimeBetweenBursts && aimInfo.canSee)
            {
                bursting = true;
                burstDirection = (aimInfo.targetPos - transform.position).normalized;
            }
        }
    }

    void Shoot()
    {
        if (Bullet == null) return;

        Instantiate(OneShotFire);

        Vector2 bulletDir;
        if (TrackPlayerDuringBurst && Game.Instance.Player != null)
        {
            var aimInfo = Aim();
            bulletDir = (aimInfo.targetPos - transform.position).normalized;
        }
        else
        {
            bulletDir = burstDirection;
        }
        timeSinceShot = 0;
        burstShotsTaken++;
    }

    (bool canSee, Vector3 targetPos) Aim()
    {
        var def = (false, Vector2.zero);
        if (Game.Instance.Player == null) return def;
        var directionToPlayer = Game.Instance.Player.transform.position - transform.position;
        int mask = LayerMask.GetMask("Friendly", "Default");
        var hit = Physics2D.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, mask);

        bool canSee = hit.collider != null && hit.collider.gameObject.CompareTag("Player");

        Vector2 playerVelocity = ShootAheadOfPlayer ? Game.Instance.Player.GetVelocity() : Vector2.zero;
        var playerPos = Game.Instance.Player.transform.position;
        var diff = playerPos - transform.position;

        return (canSee, playerPos);
    }

    void FixedUpdate()
    {
        var targetVelocity = targetDirection.normalized * Speed;
        var currentVelocity = rb.velocity;

        var diff = (targetVelocity - currentVelocity);
        rb.AddForce(Acceleration * diff);
    }

    public override void Damage(float amount)
    {
        Health -= amount;
    }

    public override void HealthChanged(float health)
    {
        var scale = healthBarInner.localScale;
        scale.x = Mathf.Max(0, health) / StartingHealth;
        healthBarInner.localScale = scale;

        animator.SetTrigger("hit");
        if (health <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        explosion.Create();
        EventBus.EnemyKilled(this);
        Destroy(gameObject);
    }

    public override void Push(Vector2 force)
    {
        throw new System.NotImplementedException();
    }
}