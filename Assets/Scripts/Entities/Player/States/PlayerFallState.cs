using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{

    public enum PlayerFallStateMessage
    {
        JumpInfo
    }
   
    public override void PhysicsProcess()
    {
        if (Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.FireWorm].Buffered)
        {
            Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.FireWorm].Consume();
            StateMachine.TransitionTo<PlayerThrowWormState>();
            return;
        }
        ApplyGravity(Player.PlayerStats.GroundedJumpInfo.FallGravity);
        var speed = Player.VelocityComponent.GetInternalSpeed();
        if (speed.y < -Player.PlayerStats.MaxFallSpeed)
        {
            speed.y = -Player.PlayerStats.MaxFallSpeed;
            Player.VelocityComponent.OverwriteInternalSpeed(speed);
        }
        AirborneMovement();
        if (IsGrounded())
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
}
