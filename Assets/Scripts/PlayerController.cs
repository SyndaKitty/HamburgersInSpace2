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
    
    public Pickle[] PickleSelection;
    int pickleIndex;

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
    Collider2D col;
    Camera cam;

    Vector2 bulletTrajectory;

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

        pickleIndex += (int)Input.mouseScrollDelta.y;
        pickleIndex %= PickleSelection.Length;
        if (pickleIndex < 0) {
            pickleIndex += PickleSelection.Length;
        }

        if (Input.GetButtonDown("Dash")) {
            pressedDash = true;
        }

        timeSinceShot += Time.deltaTime;
        Vector2 rightStickPos = new Vector2(Input.GetAxisRaw("RightStickHorizontal"), Input.GetAxisRaw("RightStickVertical"));
        
        if (timeSinceShot >= PickleSelection[pickleIndex].WaitTime) {
            Vector2 vel = Vector2.zero;
            bool fire = false;

            if (Input.GetMouseButton(0)) {
                bulletTrajectory = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                bulletTrajectory.Normalize();
                vel = bulletTrajectory * BulletSpeed;
                fire = true;
            }
            else if (Mathf.Abs(rightStickPos.x) >= Deadzone || Mathf.Abs(rightStickPos.y) >= Deadzone) {
                bulletTrajectory = rightStickPos.normalized;
                vel = bulletTrajectory * BulletSpeed;
                fire = true;
            }

            if (fire) {
                source.PlayOneShot(source.clip);
                timeSinceShot = 0;
                var bullet = Instantiate(PickleSelection[pickleIndex]);
                bullet.Fire(vel);
                bullet.transform.position = transform.position;
                // rb.AddForce(bulletTrajectory * -bullet.Weight * 100);
                rb.AddForce(bulletTrajectory * -bullet.Weight, ForceMode2D.Impulse);
            }
        }
    }

    void FixedUpdate() {
        if (!dashing && pressedDash && timeSinceDash > DashCooldown && LastStickPosition.magnitude >= Deadzone) {
            timeSinceDash = 0;
            dashing = true;
            dashDirection = LastStickPosition.normalized;
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
