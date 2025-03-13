using System.Collections;
using UnityEngine;

public class EntityController : MonoBehaviour {
    Animator anim;

    public float StartingHealth;
    
    public float SpinDamage = 50;
    public float SpinKnockback = 100;
    public float BlinkChancePerSecond = 0.25f;
    
    public float SpinTravelTime;
    public float SpinWaitTime;
    public float Acceleration;

    public OneShotSound Growl;
    public OneShotSound Break;
    public OneShotSound MeleePlayer;

    float t;
    bool spinning;
    bool wait;
    Vector2 spinTarget;
    Vector2 spinStart;
    Vector2 anchor;
    Rigidbody2D rb;
    ExplosionCreator explosion;
    float health;

    Animator rightEye;
    Animator leftEye;
    Animator upEye;
    Animator downEye;
    Animator body;

    bool left = true;
    bool right = true;
    bool up = true;
    bool down = true;

    const string SpinnyParameter = "Spinny";
    const string BlinkTrigger = "Blink";
    const string HitTrigger = "Hit";

    void Start() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        anchor = transform.position;

        body = transform.Find("Body").GetComponent<Animator>();
        rightEye = transform.Find("RightEye").GetComponent<Animator>();
        leftEye = transform.Find("LeftEye").GetComponent<Animator>();
        upEye = transform.Find("UpEye").GetComponent<Animator>();
        downEye = transform.Find("DownEye").GetComponent<Animator>();
        explosion = GetComponent<ExplosionCreator>();

        health = StartingHealth;
    }

    void SpinTowards(Vector2 target) {
        // TODO: Windup animation, show trajectory
        anim.SetBool(SpinnyParameter, true);
        spinning = true;
        t = 0;
        spinTarget = target;
        spinStart = transform.position;
    }

    void Blink() {
        var flags = new bool[] { right, left, down, up };
        var eyes = new Animator[] { rightEye, leftEye, downEye, upEye };

        for (int i = 0; i < 4; i++) {
            if (flags[i] && Random.Range(0, 1f) < BlinkChancePerSecond * Time.deltaTime) {
                eyes[i].SetTrigger(BlinkTrigger);
            }
        }
    }

    void FixedUpdate() {
        Blink();
    }

    void Update() {
        if (spinning) {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(spinStart, spinTarget, t / SpinTravelTime);
            if (t > SpinTravelTime) {
                StopSpinning();
            }
        }
        else {
            var diff = (Vector3)anchor - transform.position;
            rb.velocity = diff * Acceleration;
            
            if (!wait) {
                t += Time.deltaTime;
                if (t > SpinWaitTime) {
                    float delta = Random.Range(0, Mathf.PI * 2);
                    SpinTowards(transform.position + new Vector3(Mathf.Cos(delta), Mathf.Sin(delta)) * 5);
                }
            }
        }
    }

    void StopSpinning() {
        spinning = false;
        t = 0;
        anim.SetBool(SpinnyParameter, false);
        anchor = transform.position;
    }

    public void Damage(float amt) {
        body.SetTrigger(HitTrigger);
        var healthBefore = health;
        health -= amt;

        if (healthBefore > 0.75f * StartingHealth && health <= 0.75f * StartingHealth) {
            StartCoroutine(Phase2());
        }
        else if (healthBefore > 0.5f * StartingHealth && health <= 0.5f * StartingHealth) {
            StartCoroutine(Phase3());
        }
        else if (healthBefore > 0.25f * StartingHealth && health <= 0.25f * StartingHealth) {
            StartCoroutine(Phase4());
        }

        if (health <= 0) {
            Die();
        }
    }

    IEnumerator Phase2() {
        Instantiate(Break);
        left = false;
        wait = true;
        StopSpinning();
        yield return null;
        leftEye.GetComponent<ExplosionCreator>().Create();
        Destroy(leftEye.gameObject);
        yield return new WaitForSeconds(1f);
        Instantiate(Growl);
        yield return new WaitForSeconds(2f);
        wait = false;
    }

    IEnumerator Phase3() {
        Instantiate(Break);
        right = false;
        wait = true;
        StopSpinning();
        rightEye.GetComponent<ExplosionCreator>().Create();
        Destroy(rightEye.gameObject);
        yield return null;
        yield return new WaitForSeconds(1f);
        Instantiate(Growl);
        yield return new WaitForSeconds(2f);
        wait = false;
    }

    IEnumerator Phase4() {
        Instantiate(Break);
        up = false;
        wait = true;
        StopSpinning();
        yield return null;
        upEye.GetComponent<ExplosionCreator>().Create();
        Destroy(upEye.gameObject);
        yield return new WaitForSeconds(1f);
        Instantiate(Growl);
        yield return new WaitForSeconds(2f);
        wait = false;
    }

    void Die() {
        downEye.GetComponent<ExplosionCreator>().Create();
        explosion.Create();
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (!spinning) return;
        if (collision.collider.CompareTag("Player")) {
            var player = collision.collider.GetComponent<PlayerController>();
            player.Damage(SpinDamage);
            player.Push((player.transform.position - gameObject.transform.position).normalized * SpinKnockback);
            Instantiate(MeleePlayer);
        }
    }
}