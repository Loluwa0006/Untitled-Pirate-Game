using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAirState
{
    public override Type[] statesToAttemptToTransitionTo
    {
        get => new Type[]
        {
            typeof(PlayerFallState),
        };
    }

    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        Player.RodManager.StartDash();
        Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Dash].Consume();
    }

    public override void PhysicsProcess()
    {
        var dashDirection = Player.PlayerInput.GetMovementDirection().y;

        var dashDirectionCorrected = 1 -( (dashDirection + 1) / 2.0f); //converts range from (-1,1) to (0, 1)

        base.PhysicsProcess();
        float gravity = Player.PlayerStats.DashGravity * dashDirectionCorrected;

        ApplyGravity(gravity);
        AirborneMovement(Player.PlayerInput.GetMovementDirection(), Player.PlayerStats.DashLateralAcceleration);

        var directionToGrapple = (Player.RodManager.GrappleInfo.GrapplePosition - Player.Collider.bounds.center).normalized;

        if (Vector3.Distance(Player.RodManager.GrappleInfo.GrapplePosition, Player.Collider.bounds.center) <= Player.PlayerStats.MinDistanceBeforeDashCancelled || !Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Dash].ActionPressed)
        {
            StateMachine.TransitionTo<PlayerFallState>();
            return;
        }
        var speedToAdd = directionToGrapple * Player.PlayerStats.DashPower;
        var currentSpeed = Player.RigidBody.linearVelocity;
        if (currentSpeed.magnitude >= Player.PlayerStats.MaxDashSpeed)
        {
            var speedNormalized = currentSpeed.normalized;
            var extraSpeed = Vector2.Dot(speedToAdd, speedNormalized);
            if (extraSpeed > 0)
            {
                speedToAdd -= extraSpeed * speedNormalized;
            }
        }
        Player.RigidBody.AddForce(speedToAdd, ForceMode.VelocityChange);

    }
    public override void Process()
    {
        if (StateMachine.IsStateAvailable<PlayerThrowWormState>())
        {
            StateMachine.TransitionTo<PlayerThrowWormState>();
            return;
        }
    }
    public override void Exit()
    {
        base.Exit();
        Player.RodManager.RetractRod();
    }
    public override bool StateAvailable()
    {
        if (WormStateUtilities.AimingAtWorm(Player, Player.RodManager.GrappleMask) && Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Dash].Buffered)
        {
            return true;
        }
        return false;
    }
}
