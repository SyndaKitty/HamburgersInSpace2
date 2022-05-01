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
    
    public Pickle Bullet;
    public float BulletSpeed;

    Rigidbody2D rb;
    Vector2 LastStickPosition;

    bool dashing;
    Vector2 dashDirection;
    float timeSinceDash;
    float timeDashing;
    bool pressedDash;
    float timeSinceShot;
    AudioSource source;
    Vector2 shotInDirection;
    Collider2D col;
    Camera cam;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        cam = Camera.main;
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
        
        if (timeSinceShot >= ShootingSpeed) {
            Vector2 vel = Vector2.zero;
            bool fire = false;

            if (Input.GetMouseButton(0)) {
                vel = (cam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * BulletSpeed;
                fire = true;
            }
            else if (Mathf.Abs(rightStickPos.x) >= Deadzone || Mathf.Abs(rightStickPos.y) >= Deadzone) {
                vel = rightStickPos.normalized * BulletSpeed;
                fire = true;
            }

            if (fire) {
                source.PlayOneShot(source.clip);
                timeSinceShot = 0;
                var bullet = Instantiate(Bullet);
                bullet.Fire(vel);
                bullet.transform.position = transform.position;
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
