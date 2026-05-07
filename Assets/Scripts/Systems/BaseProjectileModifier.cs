using UnityEngine;

public class BaseProjectileModifier : MonoBehaviour
{
    public readonly short priority;
    public BaseProjectile Projectile { get; set; } 
    public virtual void InitializeModifier(BaseProjectile owner)
    {
        Projectile = owner;
    }
    public virtual void UpdateModifier()
    {

    }

    public virtual void InactiveUpdateModifier()
    {

    }

    public virtual void OnProjectileFired()
    {

    }

    public virtual void OnProjectileDestroyed()
    {

    }

    public virtual void OnProjectileLanded()
    {

    }
}
