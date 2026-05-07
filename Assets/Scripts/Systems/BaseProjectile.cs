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

    public event Action ProjectileFired;
    public event Action ProjectileDestroyed;

    public Transform Target { get; private set; }

    public bool Active { set; get; } = false;

    public BaseEntity ProjectileOwner { set; get; }
    public void InitializeProjectile(BaseEntity entity)
    {
        ProjectileOwner = entity;
        InitializeModifiers();
        OrderModifiersByPriority();
        EntityManager.Instance.RegisterEntity(this);
    }

    void InitializeModifiers()
    {
        projectileModifiers = modifierHolder.GetComponents<BaseProjectileModifier>();
        for (int i = 0; i < projectileModifiers.Length; i++)
        {
            projectileModifiers[i].InitializeModifier(this);
        }
    }

    void OrderModifiersByPriority()
    {
        for (int i = 0; i < projectileModifiers.Length; i++)
        {
            for (int x = 0; x < projectileModifiers.Length - 1; x++)
            {
                if (projectileModifiers[x].priority > projectileModifiers[x + 1].priority)
                {
                    (projectileModifiers[x + 1], projectileModifiers[x]) = (projectileModifiers[x], projectileModifiers[x + 1]);
                }
            }
        }
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
    public void DisableProjectile()
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

