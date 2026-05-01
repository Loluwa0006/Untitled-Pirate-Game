using System;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
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
    private void Start()
    {
        projectileModifiers = modifierHolder.GetComponents<BaseProjectileModifier>();
        for (int i = 0; i < projectileModifiers.Length; i++)
        {
            projectileModifiers[i].InitializeModifier(this);
        }
    }

    private void FixedUpdate()
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
        ProjectileFired.Invoke();
        projectileCollider.enabled = true;
        Active = true;
    }
    public void DestroyProjectile()
    {
        meshObjects.SetActive(false);
        ProjectileDestroyed.Invoke();
        projectileCollider.enabled = false;
        Active = false;
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
    public float fireCooldown;
    public int poolSize;
}

