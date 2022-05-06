using System.Collections;
using UnityEngine;

public class Pickle : MonoBehaviour {
    public float Damage;
    public float Weight;
    public float WaitTime;
    public float AliveTime;
    public float Speed;
    public OneShotSound OnImpactSound;

    float t;

    public void Fire(Vector2 velocity, float speedModifier) {
        GetComponent<Rigidbody2D>().velocity = velocity * Speed * speedModifier;
    }

    void Update() {
        t += Time.deltaTime;
        if (t >= AliveTime) {
            Die();
        }
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