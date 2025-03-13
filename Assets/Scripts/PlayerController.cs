using System;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class PlayerController : Entity
{
    public float Speed;
    public float Acceleration;

    public float BunDistance;
    
    public float Deadzone = 0.3f;
    public OneShotSound OneShotFire;

    public Vector2 AimDirection;
    public Vector2 MoveDirection;
    public List<Weapon> Weapons = new();

    Rigidbody2D rb;
    Collider2D col;
    Camera cam;
    SpriteRenderer sr;

    Transform healthBarInner;
    SpriteRenderer healthBarHolderSr;
    SpriteRenderer healthBarInnerSr;

    Transform bun;
    bool blocking;
    int weaponSelectIndex = -1;

    ExplosionCreator explosion;
    Animator animator;

    const string HitTrigger = "Hit";
    const string BlockBool = "Blocking";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        explosion = GetComponent<ExplosionCreator>();
        animator = GetComponent<Animator>();
        cam = Camera.main;
        healthBarInner = transform.Find("HealthbarHolder/HealthbarInner");
        healthBarInnerSr = transform.Find("HealthbarHolder/HealthbarInner").gameObject.GetComponent<SpriteRenderer>();
        healthBarHolderSr = transform.Find("HealthbarHolder").gameObject.GetComponent<SpriteRenderer>();
        bun = transform.Find("Bun");
        Init();

        SelectWeapon(Weapons.Count - 1);
    }

    void Update()
    {
        MoveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        AimDirection = new Vector2(Input.GetAxisRaw("RightStickHorizontal"), Input.GetAxisRaw("RightStickVertical"));

        blocking = Input.GetMouseButton(1);

        animator.SetBool(BlockBool, blocking);
        if (blocking)
        {
            var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;
            var diff = (mousePos - transform.position).normalized;
            float deg = Mathf.Atan2(diff.y, diff.x);
            bun.position = transform.position + diff * BunDistance;
            bun.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * deg - 90);
        }
        
        bool triggerHeld = !blocking && Input.GetMouseButton(0);
        if (weaponSelectIndex >= 0)
        {
            Weapons[weaponSelectIndex].WeaponInput(triggerHeld, Vector2.zero);
        }
    }

    void FixedUpdate()
    {
        var targetVelocity = MoveDirection * Speed;
        var currentVelocity = rb.velocity;

        var diff = targetVelocity - currentVelocity;
        rb.AddForce(Acceleration * diff);
    }

    void SelectWeapon(int index)
    {
        if (weaponSelectIndex == index) return;
        if (weaponSelectIndex >= Weapons.Count) return;
        
        if (weaponSelectIndex >= 0)
        {
            Weapons[weaponSelectIndex].Deselect();
        }

        weaponSelectIndex = index;
        if (index >= 0)
        {
            Weapons[index].Select();
        }
    }

    public Vector2 GetVelocity() => rb.velocity;
    public void Respawn()
    {
        rb.velocity = Vector2.zero;
        sr.enabled = true;
        healthBarInnerSr.enabled = true;
        healthBarHolderSr.enabled = true;
        col.enabled = true;
        enabled = true;
        Health = MaxHealth;
    }

    public override void Push(Vector2 amount)
    {
        rb.AddForce(amount, ForceMode2D.Impulse);
    }

    public override void Damage(float damage)
    {
        animator.SetTrigger(HitTrigger);
        Health -= damage;
    }

    public override void HealthChanged(float health)
    {
        var h = Math.Max(health, 0);

        var scale = healthBarInner.localScale;
        scale.x = h / MaxHealth;
        healthBarInner.localScale = scale;
    }
        
    public override void Die()
    {
        rb.velocity = Vector2.zero;
        sr.enabled = false;
        healthBarInnerSr.enabled = false;
        healthBarHolderSr.enabled = false;
        col.enabled = false;
        enabled = false;
        blocking = false;
        animator.SetBool(BlockBool, false);

        explosion.Create();
        EventBus.PlayerDied();

        Destroy(gameObject);
    }
}
