using NaughtyAttributes;
using UnityEngine;

public class ProjectileTeleportModifier : BaseProjectileModifier
{
    [SerializeField] Vector3 teleportOffset;
    [SerializeField] TeleportStyle teleportStyle;

    [System.Serializable]

    enum TeleportStyle
    {
        OnFire
    }

    [SerializeField] bool predictTargetMovement;
    [SerializeField, ShowIf(nameof(NeedsTargetPrediction))] bool predictVerticalMovement = false;

    [SerializeField, ShowIf(nameof(NeedsTargetPrediction))] int numberOfFramesToPredict = 1;
    
    public bool NeedsTargetPrediction() => predictTargetMovement;

    Rigidbody targetRB;

    public override void InitializeModifier(BaseProjectile owner)
    {
        base.InitializeModifier(owner);
        Projectile.TargetChanged += OnTargetChanged;
    }

    void OnTargetChanged(Transform target)
    {
        targetRB = target.GetComponent<Rigidbody>();
    }
    public override void OnProjectileFired()
    {
        base.OnProjectileFired();
        Debug.Log("Teleport detected projectile fired");
        if (teleportStyle == TeleportStyle.OnFire)
        {
            var player = EntityManager.Instance.GetEntitiesOfType(IDComponent.IDType.Player, true)[0];
            Debug.Log("Distance from projectile to player before == " + Vector3.Distance(player.transform.position, Projectile.RigidBody.position));

            Vector3 targetPosition = Projectile.Target.position;
            if (predictTargetMovement && targetRB != null)
            {
                Vector3 movementToUse = targetRB.linearVelocity;

                if (!predictVerticalMovement) movementToUse.y = 0.0f;

                targetPosition += movementToUse * numberOfFramesToPredict;
            }
            Projectile.RigidBody.position = targetPosition + teleportOffset;

            Debug.Log("Distance from projectile to player now == " + Vector3.Distance(player.transform.position, Projectile.RigidBody.position));
        }
    }
}