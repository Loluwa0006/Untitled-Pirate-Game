

using UnityEngine;

public class PlayerIdleState : PlayerGroundedMovementState
{

    protected override void GroundedMovement()
    {
        if (!IsGrounded())
        {
            StateMachine.TransitionTo<PlayerFallState>();
            return;
        }
        if (Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Jump].Buffered)
        {
            Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Jump].Consume();
            StateMachine.TransitionTo<PlayerJumpState>();
            return;
        }
        Vector2 movementDirection = Player.PlayerInput.GetMovementDirection();
        if (movementDirection.magnitude > 0.1f)
        {
            StateMachine.TransitionTo<PlayerRunState>();
            return;
        }

        var previous = Player.VelocityComponent.GetInternalSpeed();
        previous.x *= Player.PlayerStats.DecelerationRate;
        previous.z *= Player.PlayerStats.DecelerationRate;
        Player.VelocityComponent.OverwriteInternalSpeed(previous);
    }
}