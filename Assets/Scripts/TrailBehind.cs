using UnityEngine;

public class TrailBehind : MonoBehaviour
{
    public Transform Target;
    public float Amount;

    Rigidbody2D rb;
    Vector3 previous;

    void Start() {
        previous = Target.position;
        rb = Target.GetComponentInParent<Rigidbody2D>();
    }

    void Update() {
        transform.position = Target.position - (Vector3)rb.velocity * Amount;
    }
}
