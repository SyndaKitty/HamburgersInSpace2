using UnityEngine;

public class EnemyController : MonoBehaviour {
    public float Speed;
    public float Acceleration;

    public Transform[] Points;
    public float PointWaitTime;
    public float Radius;
    
    public int BurstShots;
    public float TimeBetweenBursts;
    public float TimeBetweenShots;
    public Pickle Bullet;
    public bool TrackPlayerDuringBurst;
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

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        health = StartingHealth;
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (waiting) {
            timeWaitingAtPoint += Time.deltaTime;
            if (timeWaitingAtPoint > PointWaitTime) {
                waiting = false;
                timeWaitingAtPoint = 0;
            }
        }

        if (!waiting) {
            if (Points == null || Points.Length == 0) {
                targetDirection = Vector2.zero;
                return;
            }

            targetDirection = Points[targetPoint].position - transform.position;
            if (targetDirection.magnitude < Radius) {
                waiting = true;
                targetPoint++;
                targetPoint %= Points.Length;
            }
        }
        else {
            targetDirection = Vector2.zero;
        }

        if (bursting) {
            if (burstShotsTaken >= BurstShots) {
                bursting = false;
                timeSinceBurst = 0;
                burstShotsTaken = 0;
            }
            else {
                timeSinceShot += Time.deltaTime;
                if (timeSinceShot >= TimeBetweenShots) {
                    Shoot();
                }
            }
        }
        else {
            timeSinceBurst += Time.deltaTime;
            var canSee = CanSeeTarget();
            if (timeSinceBurst >= TimeBetweenBursts && canSee.b) {
                bursting = true;
                burstDirection = canSee.vel;
            }
        }
    }

    void Shoot() {
        if (Bullet == null) return;
        
        Instantiate(OneShotFire);

        Vector2 velocity;
        if (TrackPlayerDuringBurst && Game.Instance.Player != null) {
            velocity = (Game.Instance.Player.transform.position - transform.position).normalized;
        }
        else {
            velocity = burstDirection;
        }
        var bullet = Instantiate(Bullet);
        bullet.Fire(velocity, BulletSpeedModifier);
        bullet.transform.position = transform.position;
        rb.AddForce(velocity * -bullet.Weight, ForceMode2D.Impulse);
        timeSinceShot = 0;
        burstShotsTaken++;
    }

    (bool b, Vector2 vel) CanSeeTarget() {
        // Debug.Log(Game.Instance.Player);
        if (Game.Instance.Player == null) return (false, Vector2.zero);
        var directionToPlayer = Game.Instance.Player.transform.position - transform.position;
        int mask = LayerMask.GetMask("Friendly", "Default");
        var hit = Physics2D.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, mask);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Player")) {
            return (true, (hit.collider.gameObject.transform.position - transform.position).normalized);
        }
        return (false, Vector2.zero);
    }

    void FixedUpdate() {
        var targetVelocity = targetDirection.normalized * Speed;
        var currentVelocity = rb.velocity;

        var diff = (targetVelocity - currentVelocity);
        rb.AddForce(Acceleration * diff);
    }

    public void Damage(float amount) {
        health -= amount;
        animator.SetTrigger("hit");
        if (health <= 0) {
            Die();
        }
    }

    public void Die() {
        Destroy(gameObject);
    }
}