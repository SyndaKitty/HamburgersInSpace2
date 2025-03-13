using UnityEngine;

public class SesameGun : Weapon
{
    public OneShotSound ShootSound;
    public float ShotsPerSecond;
    public float MinSpreadDegrees;
    public float MaxSpreadDegrees;
    public float SpreadPercentPerShot;
    public float SpreadReducePerSecond;

    float timeSinceShot;
    float currentSpread;
    float secondsPerShot => 1f / ShotsPerSecond;

    void Awake()
    {
        currentSpread = MinSpreadDegrees;
    }

    public override void Deselect()
    {
        
    }

    public override void Select()
    {
        
    }

    protected override void WeaponUpdate()
    {
        timeSinceShot = Mathf.Min(timeSinceShot + Time.deltaTime, secondsPerShot);
        currentSpread = Mathf.Max(currentSpread - SpreadReducePerSecond * Time.deltaTime, MinSpreadDegrees);

        if (triggerHeld && timeSinceShot >= secondsPerShot)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (ShootSound)
        {
            Instantiate(ShootSound);
        }
        print("Shoot! " + currentSpread);
        timeSinceShot -= secondsPerShot;

        var spreadIncrease = (MaxSpreadDegrees - MinSpreadDegrees) * SpreadPercentPerShot / 100f;
        currentSpread = Mathf.Min(currentSpread + spreadIncrease, MaxSpreadDegrees);
    }
}