using UnityEngine;
using static TestEnemy;

public class LivingRumShipAbility : BaseShipAbility
{
    Camera viewCamera;
    [SerializeField] BaseProjectile livingRumProjectile;
    [SerializeField] Transform rumTarget;
    [SerializeField] Transform firePoint;
    [SerializeField] LayerMask rumMask;
    [SerializeField] LivingRumAbilityData livingRumAbilityData;

    int durationTracker = 0;
    public override void InitializeShipAbility(AnarchyManager anarchyManager, PlayerController player)
    {
        livingRumProjectile.InitializeProjectile(player);
        base.InitializeShipAbility(anarchyManager, player);
        viewCamera = Camera.main;
        livingRumProjectile.transform.SetParent(null);
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
        if (livingRumProjectile == null)
        {
            livingRumProjectile = GetComponentInChildren<BaseProjectile>();
        }
    }
    override public void ActivateAbility()
    {
        base.ActivateAbility();
        SetRumTarget();
        durationTracker = livingRumAbilityData.RumDuration;
        livingRumProjectile.RigidBody.MovePosition(firePoint.position);
        livingRumProjectile.EnableProjectile(rumTarget);
    }

    public override void DeactivateAbility()
    {
        base.DeactivateAbility();
        livingRumProjectile.DisableProjectile();
    }

    void SetRumTarget()
    {
        Ray ray = viewCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        var raycast = Physics.Raycast(ray, out RaycastHit hit, livingRumAbilityData.RumRange, rumMask, QueryTriggerInteraction.Collide);
        if (raycast)
        {
            rumTarget.position = hit.point;
        }
        else
        {
            rumTarget.position = ray.GetPoint(livingRumAbilityData.RumRange);
        }
    }

    public override void PhysicsProcess()
    {
        base.PhysicsProcess();
        if (!abilityActive) return;
        SetRumTarget();
        durationTracker--;
        if (durationTracker <= 0 && abilityActive)
        {
            DeactivateAbility();
        }
    }
}
