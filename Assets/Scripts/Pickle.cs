using UnityEngine;

public class Pickle : MonoBehaviour {
    public float Damage;

    public void Fire(Vector2 velocity) {
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Hit!");
        Destroy(this.gameObject);
    }
}