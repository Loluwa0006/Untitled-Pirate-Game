using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class ProjectileContactModifier : BaseProjectileModifier
{
    const int MAX_CONTACTS_PER_FRAME = 3;

    [SerializeField] LayerMask projectileMask;
    [SerializeField] DamageInfo hitboxInfo;
    [SerializeField] List<HealthComponent> blacklistedTargets = new();

    Collider[] hitboxResults = new Collider[MAX_CONTACTS_PER_FRAME];

    public override void InitializeModifier(BaseProjectile owner)
    {
        base.InitializeModifier(owner);
        blacklistedTargets.Add(Projectile.ProjectileOwner.HealthComponent);
    }
    public override void UpdateModifier()
    {
        for (int i = 0; i < MAX_CONTACTS_PER_FRAME; i++)
        {
            hitboxResults[i] = null;
        }
        var overlap = Physics.OverlapSphereNonAlloc(Projectile.RigidBody.position, Projectile.ProjectileCollider.bounds.extents.z, hitboxResults, projectileMask, QueryTriggerInteraction.Collide);
        if (overlap > 0)
        {
            for (int i = 0; i < overlap; i++)
            {
                var currentCollider = hitboxResults[i];
                if (currentCollider.TryGetComponent(out HealthComponent healthComponent))
                {
                    if (blacklistedTargets.Contains(healthComponent)) continue;
                    HitboxContactInfo contactInfo = new()
                    {
                        DamageInfo = hitboxInfo,
                        hurtbox = healthComponent.Hurtbox,
                        collisionPoint = healthComponent.Hurtbox.ClosestPoint(Projectile.ProjectileCollider.bounds.center)
                    };
                    healthComponent.Damage(contactInfo);
                }
            }
        }
    }
}
