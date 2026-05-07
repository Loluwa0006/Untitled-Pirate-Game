using UnityEngine;

public class ShipManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] ShipAbilityData.ShipAbilityRegistry abilityOneID;
    [SerializeField] ShipAbilityData.ShipAbilityRegistry abilityTwoID;
    BaseShipAbility abilityOne;
    BaseShipAbility abilityTwo;
    

    [Header("Ability Prefabs")]
    [SerializeField] BaseShipAbility EMP_Prefab;
    [SerializeField] BaseShipAbility BoostShieldPrefab;
    [SerializeField] BaseShipAbility LivingRumPrefab;

    public bool AbilitiesAvailable { set; get; } = true;

    public void InitializeShipManager()
    {
       abilityOne = CreateShipAbilityPrefab(abilityOneID);
       abilityTwo = CreateShipAbilityPrefab(abilityTwoID);
       if (abilityOne != null) abilityOne.InitializeShipAbility(player.AnarchyManager, player);
       if (abilityTwo != null) abilityTwo.InitializeShipAbility(player.AnarchyManager, player);
    }

    BaseShipAbility CreateShipAbilityPrefab(ShipAbilityData.ShipAbilityRegistry ID)
    {
        switch (ID)
        {
            case ShipAbilityData.ShipAbilityRegistry.EMP:
                if (EMP_Prefab == null) return null;
                var EMPObject = Instantiate(EMP_Prefab);
                return EMPObject;
            case ShipAbilityData.ShipAbilityRegistry.BoostShield:
                if (BoostShieldPrefab == null) return null;
                var ShieldObject = Instantiate(BoostShieldPrefab);
                return ShieldObject;
            case ShipAbilityData.ShipAbilityRegistry.LivingRum:
                if (LivingRumPrefab == null) return null;
                var RumObject = Instantiate(LivingRumPrefab);
                return RumObject;
        }
        return null;
    }
    private void Update()
    {
        if (IsAbilityAvailable(abilityOne, InputManager.BufferableInputs.ShipAbilityOne))
        {
            abilityOne.ActivateAbility();
            player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.ShipAbilityOne].Consume();
        }
        if (IsAbilityAvailable(abilityTwo, InputManager.BufferableInputs.ShipAbilityTwo))
        {
            abilityTwo.ActivateAbility();
            player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.ShipAbilityTwo].Consume();
        }
    }

    protected bool IsAbilityAvailable(BaseShipAbility ability, InputManager.BufferableInputs input)
    {
        if (!AbilitiesAvailable) return false;
        if (ability == null) return false;
        if (!ability.AbilityAvailable()) return false;
        if (!player.PlayerInput.BufferRegistry[input].Buffered) return false;
        return true;
    }


}
