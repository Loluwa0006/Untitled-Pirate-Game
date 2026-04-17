using UnityEngine;
using static PlayerStats;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{


    [System.Serializable]
    public struct JumpInfo
    {
        public float jumpHeight;
        public float jumpTimeToPeak;
        public float jumpTimeToDecent;

        public float JumpGravity { get => 2.0f * jumpHeight / (jumpTimeToPeak * jumpTimeToPeak); }

        public float FallGravity { get => 2.0f * jumpHeight / (jumpTimeToDecent * jumpTimeToDecent); }

        public float JumpVelocity { get => 2.0f * jumpHeight; }

    }


    [Header("General Movement")]
    [SerializeField] float moveSpeed = 15;
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }
    [Header("Ground Movement")]
    [SerializeField] float groundAcceleration = 15 / 7.0f;
    public float GroundAcceleration { get => groundAcceleration; private set => groundAcceleration = value; }

    [SerializeField] float decelerationRate = 0.9f;
    public float DecelerationRate { get => decelerationRate; private set => decelerationRate = value; }

    [SerializeField] JumpInfo groundedJumpInfo;
    public JumpInfo GroundedJumpInfo { get => groundedJumpInfo; private set => groundedJumpInfo = value; }

    [Header("Air Movement")]
    [SerializeField] float airAcceleration = 15 / 12.0f;
    public float AirAcceleration { get => airAcceleration; private set => airAcceleration = value; }

    [SerializeField] float angleToBeConsideredTurning = 5.0f;
    public float TurnAngle { get => angleToBeConsideredTurning; private set => angleToBeConsideredTurning = value; }

    [SerializeField] float maxFallSpeed;

    public float MaxFallSpeed { get => Mathf.Abs(maxFallSpeed); private set => maxFallSpeed = value; }
    [Header("Worms")]
    [SerializeField] int maxWorms = 3;
    public int MaxWorms { get => maxWorms; private set => maxWorms = value; }

    [SerializeField] JumpInfo wormThrowInfo;

    public JumpInfo WormThrowJumpInfo { get => wormThrowInfo; private set => wormThrowInfo = value; }
}


