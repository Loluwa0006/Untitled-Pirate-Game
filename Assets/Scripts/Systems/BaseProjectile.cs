using System;
using UnityEngine;

public class BaseProjectile : BaseEntity
{
    [SerializeField] GameObject modifierHolder;
    [SerializeField] protected Rigidbody rigidBody;
    [SerializeField] protected Collider projectileCollider;
    [SerializeField] protected GameObject meshObjects;

    public Rigidbody RigidBody { get => rigidBody; }
    public Collider ProjectileCollider { get => projectileCollider; }
    BaseProjectileModifier[] projectileModifiers;

    public event Action ProjectileLanded;
    public event Action ProjectileFired;
    public event Action ProjectileDestroyed;

    public Transform Target { get; private set; }

    public bool Active { set; get; } = false;

    public BaseEntity ProjectileOwner { set; get; }
    public void InitializeProjectile(BaseEntity entity)
    {
        ProjectileOwner = entity;
        projectileModifiers = modifierHolder.GetComponents<BaseProjectileModifier>();
        for (int i = 0; i < projectileModifiers.Length; i++)
        {
            projectileModifiers[i].InitializeModifier(this);
        }
        EntityManager.Instance.RegisterEntity(this);
    }

    public override void PhysicsProcess()
    {
        if (Active)
        {
            for (int i = 0; i < projectileModifiers.Length; i++)
            {
                projectileModifiers[i].UpdateModifier();
            }
        }
        else
        {
            for (int i = 0; i < projectileModifiers.Length; i++)
            {
                projectileModifiers[i].InactiveUpdateModifier();
            }
        }
    }

    public void EnableProjectile(Transform target)
    {
        Target = target;
        meshObjects.SetActive(true);
        ProjectileFired?.Invoke();
        projectileCollider.enabled = true;
        Active = true;
        for (int i = 0; i < projectileModifiers.Length; i++)
        {
            projectileModifiers[i].OnProjectileFired();
        }
    }
    public void DestroyProjectile()
    {
        meshObjects.SetActive(false);
        ProjectileDestroyed?.Invoke();
        projectileCollider.enabled = false;
        Active = false;
        for (int i = 0; i < projectileModifiers.Length; i++)
        {
            projectileModifiers[i].OnProjectileDestroyed();
        }
    }

    public T GetModifier<T>() where T: BaseProjectileModifier
    {
        for (int i = 0; i < projectileModifiers.Length;i++)
        {
            if (projectileModifiers[i].GetType() == typeof(T))  
            {
                return projectileModifiers[i] as T;
            }
        }
    return null;
    }

}
[System.Serializable]
public struct ProjectileFireInformation
{
    public Transform spawnPoint;
    public int delayBeforeFiring;
    public BaseProjectile projectilePrefab;
    public int fireCooldown;
    public int poolSize;
}

