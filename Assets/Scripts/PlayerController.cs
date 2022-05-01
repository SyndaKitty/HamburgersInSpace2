using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float Speed;
    public float Acceleration;

    public float DashCooldown;
    public float DashSpeed;
    public float DashAcceleration;
    public float DashLength;

    public float ShootingSpeed;

    public float Deadzone = 0.3f;
    
    Rigidbody2D rb;
    Vector2 LastStickPosition;

    bool dashing;
    Vector2 dashDirection;
    float timeSinceDash;
    float timeDashing;
    bool pressedDash;
    float timeSinceShot;
    AudioSource source;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        timeSinceDash = DashCooldown;
        timeSinceShot = ShootingSpeed;
        source = GetComponent<AudioSource>();
    }

    void Update() {
        LastStickPosition = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Dash")) {
            pressedDash = true;
        }

        timeSinceShot += Time.deltaTime;
        Vector2 rightStickPos = new Vector2(Input.GetAxisRaw("RightStickHorizontal"), Input.GetAxisRaw("RightStickVertical"));
        if (Input.GetMouseButton(0) || rightStickPos.x >= Deadzone || rightStickPos.y >= Deadzone) {
            if (timeSinceShot >= ShootingSpeed) {
                source.Play();
                timeSinceShot = 0;
            }
        }
    }

    void FixedUpdate() {
        if (!dashing && pressedDash && timeSinceDash > DashCooldown) {
            timeSinceDash = 0;
            dashing = true;
            dashDirection = rb.velocity.normalized;
        }
        
        if (dashing) {
            timeDashing += Time.deltaTime;
            
            rb.velocity = dashDirection * DashSpeed;

            if (timeDashing > DashLength) {
                dashing = false;
                timeSinceDash = 0;
                timeDashing = 0;
                rb.velocity = dashDirection * Speed * 0.5f;
            }
        } 
        else {
            timeSinceDash += Time.deltaTime;
            var targetVelocity = LastStickPosition * Speed;
            var currentVelocity = rb.velocity;

            var diff = (targetVelocity - currentVelocity);
            rb.AddForce(Acceleration * diff);
        }

        pressedDash = false;
    }
}
