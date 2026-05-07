using UnityEngine;

[CreateAssetMenu(fileName = "LivingRumAbilityData", menuName = "Scriptable Objects/ShipAbilityData/LivingRumAbilityData")]
public class LivingRumAbilityData : ShipAbilityData
{
    [SerializeField] float rumRange = 100.0f;
    public float RumRange { get => rumRange; }

    [SerializeField] int rumDuration = 240; // In frames, 240 frames is 4 seconds at 60 fps

    public int RumDuration { get => rumDuration; }
}
