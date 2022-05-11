using System.Collections;
using UnityEngine;

public class Pickle : MonoBehaviour {
    public float Damage;
    public float Weight;
    public float WaitTime;
    public float Speed;
    public float InaccuracyDeviation;
    public float InnaccuracyDegrees;
    public OneShotSound OnImpactSound;
    public OneShotSound OnShieldSound;
    public float FadeTime = 0.5f;

    bool inert;
    float t;
    SpriteRenderer sr;
    Rigidbody2D rb;

    public void Fire(Vector2 velocity, float speedModifier) {
        float d = Mathf.Atan2(velocity.y, velocity.x);
        float r = Mathf.Pow(Random.Range(0f, 1f), InaccuracyDeviation) * InnaccuracyDegrees;

        d += (Random.Range(0, 2) == 0 ? -1 : 1) * r;
        velocity = new Vector2(Mathf.Cos(d), Mathf.Sin(d));
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocity * Speed * speedModifier;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (inert) {
            t += Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f - t / FadeTime);
            if (t >= FadeTime) {
                Die();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (inert) {
            return;
        }

        var enemy = collision.collider.GetComponent<EnemyController>();
        if (enemy) {
            enemy.Damage(Damage);
            if (OnImpactSound != null) {
                Instantiate(OnImpactSound); 
            }
        }

        var entity = collision.collider.GetComponent<EntityController>();
        if (entity) {
            entity.Damage(Damage);
            if (OnImpactSound != null) {
                Instantiate(OnImpactSound);
            }
        }

        if (collision.collider.CompareTag("Shield")) {
            if (OnShieldSound) {
                Instantiate(OnShieldSound);
            }
            inert = true;
            t = 0;
            return;
        }

        var player = collision.collider.GetComponent<PlayerController>();
        if (player) {
            player.Damage(Damage);
            if (OnImpactSound != null) {
                Instantiate(OnImpactSound); 
            }
        }

        Die();
    }

    void Die() {
        Destroy(this.gameObject);
    }
}