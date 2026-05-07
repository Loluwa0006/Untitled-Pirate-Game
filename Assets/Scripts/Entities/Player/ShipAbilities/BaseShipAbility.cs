using UnityEngine;

public class BaseShipAbility : BaseEntity
{
    protected AnarchyManager anarchyManager;
    protected PlayerController player;
    public int AnarchyCost { get; protected set; }

    protected bool abilityActive = false;

    public virtual void InitializeShipAbility(AnarchyManager anarchyManager, PlayerController player)
    {
        this.player = player;
        this.anarchyManager = anarchyManager;
        DeactivateAbility();
    }

    public virtual void ActivateAbility()
    {
        abilityActive = true;
        anarchyManager.CurrentAnarchy -= AnarchyCost;
    }

    public virtual void DeactivateAbility()
    {
        abilityActive = false;
    }
    public virtual bool AbilityAvailable()
    {
        return AnarchyCost <= anarchyManager.CurrentAnarchy;
    }
}
