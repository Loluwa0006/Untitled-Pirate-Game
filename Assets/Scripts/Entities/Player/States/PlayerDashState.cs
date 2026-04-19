using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAirState
{
    [SerializeField] float distanceToExitStateAt = 5.0f;
    public override Type[] statesToAttemptToTransitionTo
    {
        get => new Type[]
        {
            typeof(PlayerSwingState),
            typeof(PlayerThrowWormState),
            typeof(PlayerFallState),
            typeof(PlayerRunState),
            typeof(PlayerIdleState),
        };
    }
    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        Player.RodManager.EnableRod();
        Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Dash].Consume();
    }

    public override void PhysicsProcess()
    {
        var dashDirection = Player.PlayerInput.GetMovementDirection().y;

        var dashDirectionCorrected = 1 -( (dashDirection + 1) / 2.0f);

        base.PhysicsProcess();
        Debug.Log("Dash direction corrected == " + dashDirectionCorrected);
        float gravity = Player.PlayerStats.DashGravity * dashDirectionCorrected;

        ApplyGravity(gravity);
        AirborneMovement(Player.PlayerInput.GetMovementDirection(), Player.PlayerStats.DashLateralAcceleration);

        var directionToGrapple = (Player.RodManager.GrappleInfo.GrapplePosition - Player.Collider.bounds.center).normalized;
        Player.RigidBody.AddForce(directionToGrapple * Player.PlayerStats.DashPower, ForceMode.VelocityChange);

        if (Vector3.Distance(Player.RodManager.GrappleInfo.GrapplePosition, Player.Collider.bounds.center) <= distanceToExitStateAt || !Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Dash].Buffered)
        {
            AttemptStateTransition();
        }
    }
    public override void Process()
    {

        if (StateMachine.IsStateAvailable<PlayerThrowWormState>())
        {
            StateMachine.TransitionTo<PlayerThrowWormState>();
            return;
        }
        if (StateMachine.IsStateAvailable<PlayerSwingState>())
        {
            StateMachine.TransitionTo<PlayerSwingState>();
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
        if (WormStateUtilities.AimingAtWorm(Player, swingMask) && Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Dash].Buffered)
        {
            return true;
        }
        return false;
    }
}
