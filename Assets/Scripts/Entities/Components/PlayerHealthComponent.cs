using UnityEngine;
/// <summary>
/// Special exception to health components, players use worms, not hitpoints
/// </summary>
public class PlayerHealthComponent : HealthComponent
{
    [SerializeField] WormManager wormManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Damage(HitboxContactInfo info)
    {
        foreach (var status in statusEffects)
        {
            info = status.Value.ProcessDamage(info);
        }
        if (info.DamageInfo.damage > 0)
        {
            if (wormManager.WormsRemaining <= 0)
            {
                Kill();
                return;
            }
            wormManager.WormsRemaining--;
            entityDamaged.Invoke(info);
        }
    }
}
