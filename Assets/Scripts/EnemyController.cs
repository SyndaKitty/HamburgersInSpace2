using UnityEngine;

public class EnemyController : MonoBehaviour {
    public float Speed;
    public float Acceleration;
    public Pickle bullet;
    
    public float Radius;
    public Transform[] Points;
    public float StartingHealth;
    float health;

    Vector2 targetDirection;
    Rigidbody2D rb;
    int targetPoint;
    Animator animator;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        health = StartingHealth;
        animator = GetComponent<Animator>();
    }

    float t;
    void Update() {
        t += Time.deltaTime;
        if (Points == null || Points.Length == 0) {
            targetDirection = Vector2.zero;
            return;
        }

        targetDirection = Points[targetPoint].position - transform.position;
        if (targetDirection.magnitude < Radius) {
            targetPoint++;
            targetPoint %= Points.Length;
        }
    }

    void FixedUpdate() {
        var targetVelocity = targetDirection.normalized * Speed;
        var currentVelocity = rb.velocity;

        var diff = (targetVelocity - currentVelocity);
        rb.AddForce(Acceleration * diff);
    }

    public void Damage(float amount) {
        Debug.Log("Hit!");
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