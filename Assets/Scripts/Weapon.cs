using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Entity Source {get; private set;}
    protected bool prevHeld;
    protected bool triggerHeld;
    protected Vector2 aimDirection;

    public void Init(Entity source) 
    {
        Source = source;
    }
    
    public abstract void Select();
    public abstract void Deselect();
    public void WeaponInput(bool triggerHeld, Vector2 aimDirection)
    {
        prevHeld = this.triggerHeld;
        this.triggerHeld = triggerHeld;
        this.aimDirection = aimDirection;
        WeaponUpdate();
    }
    protected abstract void WeaponUpdate();
}