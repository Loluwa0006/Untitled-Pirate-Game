using UnityEngine;

public class BoostShieldShipAbility : BaseShipAbility
{

    [SerializeField] GameObject shieldObject;
    [SerializeField] Collider shieldCollider;
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] BoostShieldAbilityData boostShieldAbilityData;
    [SerializeField] LayerMask shieldMask;

    Collider[] shieldContacts = new Collider[HitboxComponent.MAX_CONTACTS_PER_FRAME];

    Camera viewCamera;
    public override void InitializeShipAbility(AnarchyManager anarchyManager, PlayerController player)
    {
        base.InitializeShipAbility(anarchyManager, player);
        viewCamera = Camera.main;
        if (shieldCollider == null)
        {
            shieldCollider = shieldObject.GetComponent<Collider>();
        }
    }

    public override void ActivateAbility()
    {
        shieldObject.SetActive(true);
        healthComponent.Heal(healthComponent.MaxHealth); // reset health to max when activating the shield
        shieldObject.transform.position = GetShieldSpawnLocation();
        shieldObject.transform.LookAt(player.transform.position);

    }
    public override void DeactivateAbility()
    {
        base.DeactivateAbility();
        shieldObject.SetActive(false);
    }

    public override void PhysicsProcess()
    {
    if (!shieldObject.activeSelf) return;
        
        System.Array.Clear(shieldContacts, 0, shieldContacts.Length);
        var overlap = Physics.OverlapBoxNonAlloc(
            shieldCollider.bounds.center,
            shieldCollider.bounds.extents,
            shieldContacts, 
            shieldObject.transform.rotation, 
            shieldMask, 
            QueryTriggerInteraction.Collide);


        for (int i = 0; i < overlap; i++)
        {
            if (shieldContacts[i] == player.Collider)
            {
                float lateralSpeed = new Vector2(player.RigidBody.linearVelocity.x, player.RigidBody.linearVelocity.z).magnitude;
                var boostValue = lateralSpeed * boostShieldAbilityData.PlayerContactSpeedBoost;
                player.RigidBody.AddForce(viewCamera.transform.forward * boostValue, ForceMode.VelocityChange);
                DeactivateAbility();
            }
        }
    }
    Vector3 GetShieldSpawnLocation()
    {
        Vector3 spawnLocation = player.transform.position;

        float lateralSpeed = new Vector2(player.RigidBody.linearVelocity.x, player.RigidBody.linearVelocity.z).magnitude;

        var speedSampled = boostShieldAbilityData.SpeedToShieldDistanceCurve.Evaluate(lateralSpeed);

        var distanceToSpawn = Mathf.Lerp(boostShieldAbilityData.MinDistanceFromPlayer, boostShieldAbilityData.MaxDistanceFromPlayer, speedSampled);

        spawnLocation += viewCamera.transform.forward * distanceToSpawn;
        return spawnLocation;
    }
}
