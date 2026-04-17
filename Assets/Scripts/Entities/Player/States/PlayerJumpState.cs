using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{

    Dictionary<string, object> fallStateTransitionDictionary = new();
    public override void InitializeState(EntityStateMachine stateMachine, Transform owner)
    {
        base.InitializeState(stateMachine, owner);

        jumpInfo = Player.PlayerStats.GroundedJumpInfo;

        jumpInfo.JumpGravity =  2.0f * jumpInfo.jumpHeight / (jumpInfo.jumpTimeToPeak * jumpInfo.jumpTimeToPeak);
        jumpInfo.FallGravity =  2.0f * jumpInfo.jumpHeight / (jumpInfo.jumpTimeToDecent * jumpInfo.jumpTimeToDecent);
        jumpInfo.JumpVelocity = 2.0f * jumpInfo.jumpHeight;

        fallStateTransitionDictionary[PlayerFallState.PlayerFallStateMessage.JumpInfo.ToString()] = jumpInfo;

    }

    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        var currentSpeed = Player.VelocityComponent.GetInternalSpeed();
        currentSpeed.y = jumpInfo.JumpVelocity;
        Player.VelocityComponent.OverwriteInternalSpeed(currentSpeed);
    }

    public override void PhysicsProcess()
    {
        ApplyGravity(jumpInfo.JumpGravity);
        AirborneMovement();
        var currentSpeed = Player.VelocityComponent.GetInternalSpeed();
        if (currentSpeed.y <= 0.0f)
        {
            StateMachine.TransitionTo<PlayerFallState>(fallStateTransitionDictionary);
            return;
        }
    }

    

}
