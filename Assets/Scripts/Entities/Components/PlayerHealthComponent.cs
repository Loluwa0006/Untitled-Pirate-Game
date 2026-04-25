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
        wormManager.WormsRemaining--;
        if (wormManager.WormsRemaining <= 0)
        {
            Kill();
            return;
        }
        entityDamaged.Invoke(info);
    }
}
