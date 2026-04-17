using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerBaseState : BaseState
{
    public const float MOVEMENT_DEADZONE = 0.1f;

    public const float GROUND_CHECK_SAFE_MARGIN = 0.15f;

    const float BOXCAST_RATIO = 0.8f;

   protected static LayerMask groundMask;

    public PlayerController Player { private set; get; }

    public override void InitializeState(EntityStateMachine stateMachine, Transform owner)
    {
        base.InitializeState(stateMachine, owner);
        Player = owner.GetComponent<PlayerController>();
        groundMask = LayerMask.GetMask("Ground");
    }

    public bool IsGrounded()
    {
        float checkLength = GROUND_CHECK_SAFE_MARGIN;
        Debug.Log("Ground check length " + checkLength);
        //return Physics.BoxCast(
        //    Player.Collider.bounds.center, 
        //    Player.Collider.bounds.extents, 
        //    Vector3.down, 
        //    Quaternion.identity,
        //    checkLength,
        //    groundMask          
        //    );
        float castDistance = (Player.Collider.bounds.size.y / 2.0f) + GROUND_CHECK_SAFE_MARGIN;
        bool hit = Physics.BoxCast
            (
            Player.Collider.bounds.center,
            Player.Collider.bounds.size / 2.0f * BOXCAST_RATIO,
            Vector3.down,
            Player.transform.rotation,
            castDistance,
            groundMask
            );
        return hit;
    }

    public bool AttemptGroundTransition()
    {
        if (!IsGrounded()) return false;

        if (Player.PlayerInput.GetMovementDirection().magnitude < MOVEMENT_DEADZONE)
        {
            StateMachine.TransitionTo<PlayerIdleState>();
        }
        else
        {
            StateMachine.TransitionTo<PlayerRunState>();
        }

            return true;
    }
}
