using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{

    public enum PlayerFallStateMessage
    {
        JumpInfo
    }
    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        if (message != null)
        {
            if (message.ContainsKey(PlayerFallStateMessage.JumpInfo.ToString()))
            {
                jumpInfo = (PlayerStats.JumpInfo)message[PlayerFallStateMessage.JumpInfo.ToString()];
            }
        }
    }
    public override void PhysicsProcess()
    {
        ApplyGravity(jumpInfo.FallGravity);
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
