using UnityEngine;

public class Rotate : MonoBehaviour {
    public float Torque;

    Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        rb.AddTorque(Torque * Time.deltaTime);
    }
}
