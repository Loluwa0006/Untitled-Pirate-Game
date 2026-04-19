using System;

public class PlayerFallState : PlayerAirState
{

    public enum PlayerFallStateMessage
    {
        JumpInfo
    }
    public override Type[] statesToAttemptToTransitionToEveryFrame 
    {
        get => new Type[]
        {

            typeof(PlayerSwingState),
            typeof(PlayerThrowWormState),
            typeof(PlayerJumpState),
            typeof(PlayerRunState),
            typeof(PlayerIdleState),

        };
    }

    public override void PhysicsProcess()
    {
        PlayerGrounded = IsGrounded();
        ApplyGravity(Player.PlayerStats.GroundedJumpInfo.FallGravity);
        
        AirborneMovement(Player.PlayerInput.GetMovementDirection().normalized, Player.PlayerStats.AirAcceleration);
        if (PlayerGrounded)
        {
            if (Player.PlayerInput.GetMovementDirection().magnitude > MOVEMENT_DEADZONE)
            {
                StateMachine.TransitionTo<PlayerRunState>();
            }
            else
            {
                StateMachine.TransitionTo<PlayerIdleState>();
            }
        }
    }

    public override bool StateAvailable()
    {
        return !PlayerGrounded;
    }
}
