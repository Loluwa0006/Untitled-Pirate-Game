using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{


    [System.Serializable]
    public struct JumpInfo
    {
        public float jumpHeight;
        public float jumpTimeToPeak;
        public float jumpTimeToDecent;

        public float JumpGravity { set; get; }
        public float FallGravity { set; get; }

        public float JumpVelocity { set; get; }
    }


    [Header("General Movement")]
    [SerializeField] float moveSpeed = 15;
    [Header("Ground Movement")]
    [SerializeField] float groundAcceleration = 15 / 7.0f;
    [SerializeField] float decelerationRate = 0.9f;
    [SerializeField] JumpInfo groundedJumpInfo;

    [Header("Air Movement")]
    [SerializeField] float airAcceleration = 15 / 12.0f;
    [SerializeField] float angleToBeConsideredTurning = 5.0f;

    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }
    public float GroundAcceleration { get => groundAcceleration; private set => groundAcceleration = value; }

    public float DecelerationRate { get => decelerationRate; private set => decelerationRate = value; }

    public float AirAcceleration { get => airAcceleration; private set => airAcceleration = value; }

    public JumpInfo GroundedJumpInfo { get => groundedJumpInfo; private set => groundedJumpInfo = value;}

    public float TurnAngle { get => angleToBeConsideredTurning; private set => angleToBeConsideredTurning = value; }
}


