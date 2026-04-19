using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwingState : PlayerAirState
{
    float maxGrappleDistance;

    public override Type[] statesToAttemptToTransitionToEveryFrame
    {
        get => new Type[]
        {

        };
        protected set => base.statesToAttemptToTransitionToEveryFrame = value;
    }
    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        Player.RodManager.FireRod();
        maxGrappleDistance = Vector3.Distance(Player.Collider.bounds.center, Player.RodManager.GrappleInfo.GrapplePosition);
        Player.RigidBody.AddForce(Player.RigidBody.linearVelocity.normalized * Player.PlayerStats.SwingJumpInfo.JumpVelocity, ForceMode.VelocityChange);
        Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Swing].Consume();
    }

    public override void Process()
    {
        if (StateMachine.IsStateAvailable<PlayerThrowWormState>())
        {
            StateMachine.TransitionTo<PlayerThrowWormState>();
            return;
        }
        if (!Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Swing].Buffered)
        {
            StateMachine.TransitionTo<PlayerFallState>();
            return;
        }
        if (Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Jump].Buffered)
        {
            var jumpVelocity = Player.RigidBody.linearVelocity.normalized * Player.PlayerStats.SwingJumpInfo.JumpVelocity;
            if (jumpVelocity.y < Player.PlayerStats.MinSwingJumpHeight) jumpVelocity.y = Player.PlayerStats.MinSwingJumpHeight;
            Player.RigidBody.AddForce(jumpVelocity, ForceMode.VelocityChange);
            Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Jump].Consume();
            StateMachine.TransitionTo<PlayerFallState>();
            return;
        }

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
        Player.RodManager.EndSwing();
    }

    public override bool StateAvailable()
    {
        if (AimingAtWorm() && Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Swing].Buffered)
        {
            return true;
        }
        return false;
    }

    public bool AimingAtWorm()
    {
        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        bool hit = Physics.Raycast(ray, Player.PlayerStats.MaxRodRange, swingMask, QueryTriggerInteraction.Collide);
        return hit;
    }
}

