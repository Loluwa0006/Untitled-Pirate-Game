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

    [SerializeField] float decelerationDrag = 0.9f;
    public float DecelerationDrag { get => decelerationDrag; private set => decelerationDrag = value; }

    [SerializeField] JumpInfo groundedJumpInfo;
    public JumpInfo GroundedJumpInfo { get => groundedJumpInfo; private set => groundedJumpInfo = value; }

    [Header("Air Movement")]
    [SerializeField] float airAcceleration = 15 / 12.0f;
    public float AirAcceleration { get => airAcceleration; private set => airAcceleration = value; }

    [SerializeField] float angleToBeConsideredTurning = 5.0f;
    public float TurnAngle { get => angleToBeConsideredTurning; private set => angleToBeConsideredTurning = value; }

    [SerializeField] float maxFallSpeed;

    public float MaxFallSpeed { get => Mathf.Abs(maxFallSpeed) * -1; private set => maxFallSpeed = value; }

    [SerializeField] AnimationCurve turnAngleToSpeedLost;

    public AnimationCurve TurnAngleSpeedLostCurve { get => turnAngleToSpeedLost; private set => turnAngleToSpeedLost = value;}
    [Header("Worms")]
    [SerializeField] int maxWorms = 3;
    public int MaxWorms { get => maxWorms; private set => maxWorms = value; }

    [SerializeField] JumpInfo wormThrowInfo;

    public JumpInfo WormThrowJumpInfo { get => wormThrowInfo; private set => wormThrowInfo = value; }

    [Header("Rod")]
    [SerializeField] float maxRodRange = 120;
    public float MaxRodRange { get => maxRodRange; private set => maxRodRange = value; }

    #region SwingInfo
    [SerializeField] float swingAcceleration = 8.0f;
    public float SwingAcceleration { get => swingAcceleration; private set => swingAcceleration = value; }

    [SerializeField] JumpInfo swingJumpInfo;
    public JumpInfo SwingJumpInfo { get => swingJumpInfo; private set => swingJumpInfo = value; }

    [SerializeField] float minSwingJumpHeight = 5.0f;
    public float MinSwingJumpHeight { get => minSwingJumpHeight; private set => minSwingJumpHeight = value; }

    [SerializeField] float rodSpring = 10.0f;

    public float RodSpring { get => rodSpring; private set => rodSpring = value; }

    [SerializeField] float rodDamper = 0.2f;

    public float RodDamper { get => rodDamper; }

    [SerializeField, Range(0,1)] float rodMinDistanceWithNoSpring = 0.2f;

    public float RodMinDistanceWithNoSpring { get => rodMinDistanceWithNoSpring;}

    [SerializeField, Range(0, 1)] float rodMaxDistanceWIthNoSpring = 0.8f;

    public float RodMaxDistanceWithNoSpring { get => rodMaxDistanceWIthNoSpring;}

    [SerializeField] float rodSwingMassScale = 4.5f;

    public float RodSwingMassScale { get => rodSwingMassScale; }
    #endregion

    #region Dash

    [SerializeField] float dashGravity = 8.0f;

    public float DashGravity { get => dashGravity; }

    [SerializeField] float dashPower = 30.0f;

    public float DashPower { get => dashPower; }

    [SerializeField] float dashLateralAcceleration;

    public float DashLateralAcceleration { get => dashLateralAcceleration; }

    #endregion

}


