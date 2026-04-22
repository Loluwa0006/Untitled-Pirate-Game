using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwingState : PlayerAirState
{

    public override Type[] statesToAttemptToTransitionTo
    {
        get => new Type[]
        {
            typeof(PlayerShadowstepState),
            
            typeof(PlayerParryState),
            typeof(PlayerThrowWormState),
            typeof(PlayerDashState),
        };
    }
    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        Player.RodManager.StartSwing();
        Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Swing].Consume();
    }

    public override void Process()
    {
        if (Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Jump].Buffered)
        {
            var jumpVelocity = Player.RigidBody.linearVelocity.normalized * (Player.PlayerStats.SwingJumpInfo.JumpVelocity + (Player.PlayerStats.SwingSpeedToJumpPowerRatio * Player.RigidBody.linearVelocity.magnitude));
            if (jumpVelocity.y < Player.PlayerStats.MinSwingJumpHeight) jumpVelocity.y = Player.PlayerStats.MinSwingJumpHeight;
            Player.RigidBody.AddForce(jumpVelocity, ForceMode.VelocityChange);
            Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Jump].Consume();
            StateMachine.TransitionTo<PlayerFallState>();
            return;
        }
        if (StateMachine.IsStateAvailable<PlayerDashState>())
        {
            StateMachine.TransitionTo<PlayerDashState>();
            return;
        }
        if (!Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Swing].Buffered)
        {
            StateMachine.TransitionTo<PlayerFallState>();
            return;
        }
        
        AttemptStateTransition();
    }
    public override void PhysicsProcess()
    {
        base.PhysicsProcess();
        float gravity;
        if (Player.RigidBody.linearVelocity.y > 0) gravity = Player.PlayerStats.SwingJumpInfo.JumpGravity;
        else gravity = Player.PlayerStats.SwingJumpInfo.FallGravity;

        ApplyGravity(gravity);
        AirborneMovement(Player.PlayerInput.GetMovementDirection(), Player.PlayerStats.SwingAcceleration);
    }

  
    public override void Exit()
    {
        base.Exit();
        Player.RodManager.RetractRod();
        Player.AnarchyManager.GenerateAnarchy(ScaledGenerationMethod.Swing);
    }

    public override bool StateAvailable()
    {
        if (WormStateUtilities.AimingAtWorm(Player, Player.RodManager.GrappleMask) && Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Swing].Buffered)
        {
            return true;
        }
        return false;
    }
}

