using UnityEngine;

public class ProjectileVelocityModifier : BaseProjectileModifier
{
    [SerializeField] float moveAcceleration = 8.0f;
    [SerializeField] float moveSpeed = 120.0f;

    Vector3 directionTowardsTarget;

    public override void OnProjectileFired()
    {
        directionTowardsTarget = (Projectile.Target.position - Projectile.RigidBody.position).normalized;
    }

    public override void UpdateModifier()
    {
        var forceToAdd = directionTowardsTarget * moveAcceleration;
        var newVelocity = forceToAdd + Projectile.RigidBody.linearVelocity;
        if ( newVelocity.magnitude >= moveSpeed)
        {
            var speedNormalized = Projectile.RigidBody.linearVelocity.normalized;
            var extraSpeed = Vector3.Dot(forceToAdd, speedNormalized);
            if (extraSpeed > 0)
            {
                forceToAdd -= extraSpeed * speedNormalized;
            }
        }

        Projectile.RigidBody.AddForce(forceToAdd, ForceMode.VelocityChange);
    }
}
