using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAirState
{
    public override Type[] statesToAttemptToTransitionTo
    {
        get => new Type[]
        {
            typeof(PlayerShadowstepState),
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

        var dashDirectionCorrected = (dashDirection + 1) / 2.0f; //converts range from (-1,1) to (0,1)

        base.PhysicsProcess();
        float gravity = Player.PlayerStats.DashGravity * (1.0f - dashDirectionCorrected);

        ApplyGravity(gravity);
        var movementCorrected = new Vector2(Player.PlayerInput.GetMovementDirection().x, 0); //force 0 because forward/backward movement is completely handled by dash functionality
        AirborneMovement(movementCorrected, Player.PlayerStats.DashLateralAcceleration);

        var directionToGrapple = (Player.RodManager.GrappleInfo.GrapplePosition - Player.Collider.bounds.center).normalized;

        if (Vector3.Distance(Player.RodManager.GrappleInfo.GrapplePosition, Player.Collider.bounds.center) <= Player.PlayerStats.MinDistanceBeforeDashCancelled || !Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Dash].ActionPressed)
        {
            StateMachine.TransitionTo<PlayerFallState>();
            return;
        }
        var speedToAdd = GetSpeedToAdd(directionToGrapple);
        Player.RigidBody.AddForce(speedToAdd, ForceMode.VelocityChange);
        var strippedLateral = StripLateralMovementBasedOnInput(directionToGrapple, dashDirectionCorrected);
        Player.RigidBody.AddForce(strippedLateral, ForceMode.VelocityChange);
    }

    Vector3 GetSpeedToAdd(Vector3 directionToGrapple)
    {
        Vector3 speedToAdd = directionToGrapple * Player.PlayerStats.DashPower;
        var currentSpeed = new Vector3(Player.RigidBody.linearVelocity.x, 0, Player.RigidBody.linearVelocity.z); //don't use y when clamping lateral movement
        if (currentSpeed.magnitude >= Player.PlayerStats.MaxDashSpeed)
        {
            var speedNormalized = currentSpeed.normalized;
            var extraSpeed = Vector2.Dot(speedToAdd, speedNormalized);
            if (extraSpeed > 0)
            {
                speedToAdd -= extraSpeed * speedNormalized;
            }
        }
        return speedToAdd;
    }

    Vector3 StripLateralMovementBasedOnInput(Vector3 directionToGrapple, float yAxis)
    {
        var velocityProjected = Vector3.Dot(Player.RigidBody.linearVelocity, directionToGrapple) * directionToGrapple;
        var lateralMovement = Player.RigidBody.linearVelocity - velocityProjected; //subtract aligned velocity
        return -lateralMovement * yAxis;
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
        Player.AnarchyManager.GenerateAnarchy(ScaledGenerationMethod.Dash);
    }
    public override bool StateAvailable()
    {
        if (GrappleUtilities.AimingAtGrappable(Player, Player.RodManager.GrappleMask) && Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Dash].Buffered)
        {
            if (Vector3.Distance(GrappleUtilities.RaycastResult.point, Player.Collider.bounds.center) >= Player.PlayerStats.MinDistanceBeforeDashCancelled)
            {
                return true;
            }
        }
        return false;
    }
}
