using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class HitboxComponent : MonoBehaviour
{

    public const int MAX_CONTACTS_PER_FRAME = 4;
    [SerializeField] Collider hitboxCollider;
    [SerializeField] LayerMask hitboxMask;
    [SerializeField] List<HealthComponent> blacklistedTargets;
    [SerializeField] DamageInfo damageInfo;
    Collider[] struckTargets = new Collider[MAX_CONTACTS_PER_FRAME];
    List<HealthComponent> previousTargets = new();

    bool isBoxCollider;
    private void Start()
    {   
        if (hitboxCollider == null) hitboxCollider = GetComponent<Collider>();
        isBoxCollider = (hitboxCollider is BoxCollider);
    }
    public void Activate()
    {
        hitboxCollider.enabled = true;
        previousTargets.Clear();
    }

    public void Deactivate()
    {
        hitboxCollider.enabled = false;
    }

    private void FixedUpdate()
    {
        if (hitboxCollider.enabled)
        {
            CheckForCollisions();
        }
    }

    void CheckForCollisions()
    {
        System.Array.Clear(struckTargets, 0, struckTargets.Length);
        if (!isBoxCollider) Physics.OverlapSphereNonAlloc(hitboxCollider.bounds.center, hitboxCollider.bounds.extents.x, struckTargets, hitboxMask, QueryTriggerInteraction.Collide);
        else Physics.OverlapBoxNonAlloc(hitboxCollider.bounds.center, hitboxCollider.bounds.extents, struckTargets, hitboxCollider.transform.rotation, hitboxMask, QueryTriggerInteraction.Collide);
        for (int i = 0; i < struckTargets.Length; i++)
        {
            var collider = struckTargets[i];
            if (collider == null) continue;
            if (collider.TryGetComponent(out HealthComponent hp))
            {
                if (blacklistedTargets.Contains(hp) || previousTargets.Contains(hp)) continue;
                DamageEntity(hp);
            }
        }
    }

    void DamageEntity(HealthComponent healthComponent)
    {
        HitboxContactInfo collisionInfo = new()
        {
            DamageInfo = damageInfo,
            collisionPoint = hitboxCollider.bounds.ClosestPoint(healthComponent.Hurtbox.bounds.center),
            hurtbox = healthComponent.Hurtbox,
        };
        healthComponent.Damage(collisionInfo);
        previousTargets.Add(healthComponent);
    }
}
[System.Serializable]
public struct DamageInfo
{
    public int damage;
    public float horizontalKnockback;
    public float verticalKnockback;
    public int hitstunFrames;
    public DamageSource damageSource;
}

public struct HitboxContactInfo
{
    public DamageInfo DamageInfo;
    public Vector3 collisionPoint;
    public Collider hurtbox;
}
public enum DamageSource : short
{
    EnemyWall,
    AnySource
}

