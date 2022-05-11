using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float Speed;
    public float Acceleration;

    public float DashCooldown;
    public float DashSpeed;
    public float DashAcceleration;
    public float DashLength;

    public float ShootingSpeed;
    public float BulletSpeedModifier = 1f;

    public float Deadzone = 0.3f;
    public OneShotSound OneShotFire;
    public float MaxHealth;

    public Pickle[] PickleSelection;
    int pickleIndex;

    public Rigidbody2D rb;
    public Vector2 LastStickPosition;

    public float BunDistance;

    bool blocking;
    bool dashing;
    Vector2 dashDirection;
    float timeSinceDash;
    float timeDashing;
    bool pressedDash;
    float timeSinceShot;
    AudioSource source;
    Collider2D col;
    Camera cam;
    SpriteRenderer sr;

    Vector2 bulletTrajectory;

    float health;
    Transform healthBarInner;
    SpriteRenderer healthBarHolderSr;
    SpriteRenderer healthBarInnerSr;

    Transform bun;

    ExplosionCreator explosion;
    Animator animator;

    const string HitTrigger = "Hit";
    const string BlockBool = "Blocking";

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        explosion = GetComponent<ExplosionCreator>();
        animator = GetComponent<Animator>();
        cam = Camera.main;
        timeSinceDash = DashCooldown;
        timeSinceShot = ShootingSpeed;
        source = GetComponent<AudioSource>();
        healthBarInner = transform.Find("HealthbarHolder/HealthbarInner");
        SetHealth(MaxHealth);
        healthBarInnerSr = transform.Find("HealthbarHolder/HealthbarInner").gameObject.GetComponent<SpriteRenderer>();
        healthBarHolderSr = transform.Find("HealthbarHolder").gameObject.GetComponent<SpriteRenderer>();
        bun = transform.Find("Bun");
    }

    void SetHealth(float amt) {
        health = amt;
        if (amt <= 0) {
            Die();
        }

        var scale = healthBarInner.localScale;
        scale.x = health / MaxHealth;
        healthBarInner.localScale = scale;
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

        blocking = Input.GetMouseButton(1);
        animator.SetBool(BlockBool, blocking);
        if (blocking) {
            var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;
            var diff = (mousePos - transform.position).normalized;
            float deg = Mathf.Atan2(diff.y, diff.x);
            bun.position = transform.position + diff * BunDistance;
            Debug.Log(transform.position + " " + diff + " " + BunDistance);
            bun.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * deg - 90);
        }

        timeSinceShot += Time.deltaTime;
        Vector2 rightStickPos = new Vector2(Input.GetAxisRaw("RightStickHorizontal"), Input.GetAxisRaw("RightStickVertical"));
        
        if (!blocking && timeSinceShot >= PickleSelection[pickleIndex].WaitTime) {
            Vector2 vel = Vector2.zero;
            bool fire = false;

            if (Input.GetMouseButton(0)) {
                bulletTrajectory = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                bulletTrajectory.Normalize();
                vel = bulletTrajectory;
                fire = true;
            }
            else if (Mathf.Abs(rightStickPos.x) >= Deadzone || Mathf.Abs(rightStickPos.y) >= Deadzone) {
                bulletTrajectory = rightStickPos.normalized;
                vel = bulletTrajectory;
                fire = true;
            }

            if (fire) {
                Instantiate(OneShotFire);
                timeSinceShot = 0;
                var bullet = Instantiate(PickleSelection[pickleIndex]);
                bullet.Fire(vel, 1.0f);
                bullet.transform.position = transform.position;
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

    public void Damage(float damage) {
        animator.SetTrigger(HitTrigger);
        SetHealth(health - damage);
    }

    public void Die() {
        dashing = false;
        rb.velocity = Vector2.zero;
        sr.enabled = false;
        healthBarInnerSr.enabled = false;
        healthBarHolderSr.enabled = false;
        col.enabled = false;
        enabled = false;
        animator.SetBool(BlockBool, false);
        explosion.Create();
        Game.Instance.PlayerDied();
    }

    public Vector2 GetVelocity() => rb.velocity;

    public void Respawn() {
        rb.velocity = Vector2.zero;
        sr.enabled = true;
        healthBarInnerSr.enabled = true;
        healthBarHolderSr.enabled = true;
        col.enabled = true;
        enabled = true;
        SetHealth(MaxHealth);
    }

    public void Push(Vector2 amount) {
        rb.AddForce(amount, ForceMode2D.Impulse);
    }
}
