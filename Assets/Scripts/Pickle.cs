using UnityEngine;

public class Pickle : MonoBehaviour {
    public float Damage;
    public float Weight;
    public float WaitTime;
    public float AliveTime;

    float t;

    public void Fire(Vector2 velocity) {
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    void Update() {
        t += Time.deltaTime;
        if (t >= AliveTime) {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Die();
    }

    void Die() {
        Destroy(this.gameObject);
    }
}