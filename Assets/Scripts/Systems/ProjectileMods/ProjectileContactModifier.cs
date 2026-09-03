using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
public class ProjectileContactModifier : BaseProjectileModifier
{
    const int MAX_CONTACTS_PER_FRAME = 3;

    [SerializeField] float delayBeforeActivation = 0.0f;
    [SerializeField] LayerMask projectileMask;
    [SerializeField] DamageInfo hitboxInfo;

    //if false, will use a box cast instead
    [SerializeField] bool useSphereCollision = true;
    [SerializeField] PostContactLogic postContactLogic;

    Collider[] hitboxResults = new Collider[MAX_CONTACTS_PER_FRAME];

    List<HealthComponent> permanentBlacklistedTargets = new();
    HashSet<HealthComponent> blacklistedTargets = new();

    public UnityEvent hitboxEnabled = new();
    public enum PostContactLogic
    {
        DisableProjectile,
        DisableHitbox,
        DisableHitboxAndStop,

        ContinueProcessing,
    }

    bool checkForContacts = true;
    float delayTracker = 0.0f;

    bool delayAlreadyOver = false;
    public override void InitializeModifier(BaseProjectile owner)
    {
        base.InitializeModifier(owner);

        switch (Projectile.OwnerEntityType)
        {
            case BaseProjectile.OwnerType.Player:
                PlayerController player = (PlayerController)Projectile.ProjectileOwner;
                permanentBlacklistedTargets.Add(player.HealthComponent);
                break;
            case BaseProjectile.OwnerType.Enemy:
                BaseEnemy enemy = (BaseEnemy)Projectile.ProjectileOwner;
                permanentBlacklistedTargets.AddRange(enemy.HealthFragments);
                break;
            default:
                permanentBlacklistedTargets.AddRange(Projectile.ProjectileOwner.GetComponentsInChildren<HealthComponent>());
                break;
        }
        delayTracker = delayBeforeActivation;
    }
    public override void OnProjectileFired()
    {
        base.OnProjectileFired();
        blacklistedTargets.Clear();
        for (int i = 0; i < permanentBlacklistedTargets.Count; i++)
        {
            blacklistedTargets.Add(permanentBlacklistedTargets[i]);
        }
        checkForContacts = true;
        delayAlreadyOver = false;
    }

    void DelayLogic()
    {
        delayTracker = Mathf.MoveTowards(delayTracker, 0.0f, Time.fixedDeltaTime);
        if (delayTracker <= 0.001f && !delayAlreadyOver)
        {
            hitboxEnabled.Invoke();
            delayAlreadyOver = true;
        }
    }
    public override void UpdateModifier()
    {
        DelayLogic();
        if (!checkForContacts || delayTracker > 0.001f) return;
        bool validContact = false;
        for (int x = 0; x < Projectile.ProjectileColliders.Count; x++)
        {
            for (int i = 0; i < MAX_CONTACTS_PER_FRAME; i++)
            {
                hitboxResults[i] = null;
            }
            var hitbox = Projectile.ProjectileColliders[x];
            int overlap;
            if (useSphereCollision)
            {
                overlap = Physics.OverlapSphereNonAlloc(hitbox.bounds.center, hitbox.bounds.extents.magnitude, hitboxResults, projectileMask, QueryTriggerInteraction.Collide);
            }
            else
            {
                overlap = Physics.OverlapBoxNonAlloc(hitbox.bounds.center, hitbox.bounds.extents, hitboxResults, hitbox.transform.rotation, projectileMask, QueryTriggerInteraction.Collide);
            }
            for (int y = 0; y < overlap; y++)
            {
                if (DamageHealthComponent(hitboxResults[y], Projectile.ProjectileColliders[x]))
                {
                    validContact = true;
                }
            }
            PostHitboxCollisionCheckLogic(validContact);
        }
    }

    void PostHitboxCollisionCheckLogic(bool validContact)
    {
        if (validContact)
        {
            switch (postContactLogic)
            {
                case PostContactLogic.DisableHitbox:
                    checkForContacts = false;
                    break;
                case PostContactLogic.DisableProjectile:
                    Projectile.DisableProjectile();
                    break;
                case PostContactLogic.DisableHitboxAndStop:
                    checkForContacts = false;
                    Projectile.RigidBody.linearVelocity = Vector3.zero;
                    break;
                case PostContactLogic.ContinueProcessing:
                    //just keep doing stuff
                    break;
            }
        }
    }
    public bool DamageHealthComponent(Collider hurtbox, Collider hitbox)
    {
        if (!hurtbox.TryGetComponent(out HealthComponent healthComponent)) return false;      
        if (blacklistedTargets.Contains(healthComponent)) return false;
        
        HitboxContactInfo contactInfo = new()
        {
            DamageInfo = hitboxInfo,
            hurtbox = healthComponent.Hurtbox,
            collisionPoint = healthComponent.Hurtbox.ClosestPoint(hitbox.bounds.center)
        };
        healthComponent.Damage(contactInfo);
        Projectile.ProjectileLanded.Invoke(healthComponent);
        blacklistedTargets.Add(healthComponent);
        return true;
    }
}