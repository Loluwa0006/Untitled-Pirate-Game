using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{

    Dictionary<string, object> fallStateTransitionDictionary = new();
    public override void InitializeState(EntityStateMachine stateMachine, Transform owner)
    {
        base.InitializeState(stateMachine, owner);
        fallStateTransitionDictionary[PlayerFallState.PlayerFallStateMessage.JumpInfo.ToString()] = Player.PlayerStats.GroundedJumpInfo;
    }

    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        var currentSpeed = Player.VelocityComponent.GetInternalSpeed();
        currentSpeed.y = Player.PlayerStats.GroundedJumpInfo.JumpVelocity;
        Player.VelocityComponent.OverwriteInternalSpeed(currentSpeed);
    }

    public override void PhysicsProcess()
    {
        if (Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.FireWorm].Buffered)
        {
            Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.FireWorm].Consume();
            StateMachine.TransitionTo<PlayerThrowWormState>();
            return;
        }
        ApplyGravity(Player.PlayerStats.GroundedJumpInfo.JumpGravity);
        AirborneMovement();
        var currentSpeed = Player.VelocityComponent.GetInternalSpeed();
        if (currentSpeed.y <= 0.0f)
        {
            StateMachine.TransitionTo<PlayerFallState>(fallStateTransitionDictionary);
            return;
        }
    }

    

}
