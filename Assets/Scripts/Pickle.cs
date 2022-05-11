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

    float t;

    public void Fire(Vector2 velocity, float speedModifier) {
        float d = Mathf.Atan2(velocity.y, velocity.x);
        float r = Mathf.Pow(Random.Range(0f, 1f), InaccuracyDeviation) * InnaccuracyDegrees;

        d += (Random.Range(0, 2) == 0 ? -1 : 1) * r;
        velocity = new Vector2(Mathf.Cos(d), Mathf.Sin(d));
        GetComponent<Rigidbody2D>().velocity = velocity * Speed * speedModifier;
    }

    void Update() {
        t += Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision) {
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