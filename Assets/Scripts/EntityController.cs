using UnityEngine;

public class EntityController : MonoBehaviour {
    Animator anim;

    public float SpinTravelTime;
    public float SpinWaitTime;
    public float Acceleration;

    public float SpinDamage = 50;
    public float SpinKnockback = 100;
    public float BlinkChancePerSecond = 0.25f;

    float t;
    bool spinning;
    Vector2 spinTarget;
    Vector2 spinStart;
    Vector2 anchor;
    Rigidbody2D rb;

    Animator rightEye;
    Animator leftEye;
    Animator upEye;
    Animator downEye;

    const string SpinnyParameter = "Spinny";
    const string BlinkTrigger = "Blink";

    void Start() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        anchor = transform.position;

        rightEye = transform.Find("RightEye").GetComponent<Animator>();
        leftEye = transform.Find("LeftEye").GetComponent<Animator>();
        upEye = transform.Find("UpEye").GetComponent<Animator>();
        downEye = transform.Find("DownEye").GetComponent<Animator>();
    }

    void SpinTowards(Vector2 target) {
        anim.SetBool(SpinnyParameter, true);
        spinning = true;
        t = 0;
        spinTarget = target;
        spinStart = transform.position;
    }

    void Blink() {
        if (Random.Range(0, 1f) < BlinkChancePerSecond * Time.deltaTime) {
            rightEye.SetTrigger(BlinkTrigger);
        }
        if (Random.Range(0, 1f) < BlinkChancePerSecond * Time.deltaTime) {
            leftEye.SetTrigger(BlinkTrigger);
        }
        if (Random.Range(0, 1f) < BlinkChancePerSecond * Time.deltaTime) {
            downEye.SetTrigger(BlinkTrigger);
        }
        if (Random.Range(0, 1f) < BlinkChancePerSecond * Time.deltaTime) {
            upEye.SetTrigger(BlinkTrigger);
        }
    }

    void FixedUpdate() {
        Blink();
    }

    void Update() {
        if (!spinning) {
            t += Time.deltaTime;
            var diff = (Vector3)anchor - transform.position;
            rb.velocity = diff * Acceleration;
            
            if (t > SpinWaitTime) {
                float delta = Random.Range(0, Mathf.PI * 2);
                SpinTowards(new Vector3(Mathf.Cos(delta), Mathf.Sin(delta)) * 5);
            }
        }
        else {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(spinStart, spinTarget, t / SpinTravelTime);
            if (t > SpinTravelTime) {
                t = 0;
                anim.SetBool(SpinnyParameter, false);
                spinning = false; 
                anchor = transform.position;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (!spinning) return;
        if (collision.collider.CompareTag("Player")) {
            var player = collision.collider.GetComponent<PlayerController>();
            player.Damage(SpinDamage);
            player.Push((player.transform.position - gameObject.transform.position).normalized * SpinKnockback);
        }
    }
}