using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float Speed;
    public float Acceleration;

    Rigidbody2D rb;
    Vector2 LastStickPosition;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        LastStickPosition = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate() {
        var targetVelocity = LastStickPosition * Speed;
        var currentVelocity = rb.velocity;

        var diff = (targetVelocity - currentVelocity);
        rb.AddForce(Acceleration * diff);
    }
}
