using UnityEngine;

[CreateAssetMenu(fileName = "BoostShieldAbilityData", menuName = "Scriptable Objects/BoostShieldAbilityData")]
public class BoostShieldAbilityData : ShipAbilityData
{
    [SerializeField] float shieldDuration = 5.0f;

    public float ShieldDuration { get => shieldDuration; }

    [SerializeField] float minDistanceFromPlayer = 30.0f;

    public float MinDistanceFromPlayer { get => minDistanceFromPlayer; }

    [SerializeField] float maxDistanceFromPlayer = 120.0f;

    public float MaxDistanceFromPlayer { get => maxDistanceFromPlayer; }

    [SerializeField] float maxSpeedToConsiderForPrediction = 80.0f;

    public float MaxSpeedToConsiderForPrediction { get => maxSpeedToConsiderForPrediction; }

    [SerializeField] AnimationCurve speedToShieldDistanceCurve; 

    public AnimationCurve SpeedToShieldDistanceCurve { get => speedToShieldDistanceCurve; }

    [SerializeField, Range(0, 5)] float playerContactSpeedBoost = 0.15f;

    public float PlayerContactSpeedBoost { get => playerContactSpeedBoost; }
}
