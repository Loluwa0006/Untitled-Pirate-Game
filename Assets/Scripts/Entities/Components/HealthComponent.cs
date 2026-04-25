using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] int maxHealth;
    public int MaxHealth { get => maxHealth; protected set => maxHealth = value;}

    [SerializeField] Collider hurtbox;

    public Collider Hurtbox { get => hurtbox; }

    int health;

    public int Health {  get => health; protected set => health = Mathf.Clamp(value, 0, maxHealth); }

    public UnityEvent entityKilled = new();
    public UnityEvent<HitboxContactInfo> entityDamaged = new();

    public void Start()
    {
        health = MaxHealth;
    }

    public virtual void Damage(HitboxContactInfo info)
    {
        health -= info.DamageInfo.damage;
        if (health <= 0)
        {
            health = 0;
            Kill();
            return;
        }
        entityDamaged.Invoke(info);
    }

    public virtual void Kill()
    {
        entityKilled.Invoke();
    }
}

public abstract class StatusEffect
{
    public abstract void PhysicsProcess();

    public abstract int ProcessDamage(int damage);
}