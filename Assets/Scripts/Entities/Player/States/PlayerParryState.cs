using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParryState : PlayerAirState
{

    [SerializeField] LayerMask parryMask;
    int durationTracker = 0;

    public override Type[] statesToAttemptToTransitionTo
    {
        get => new Type[]
        {
            
        };
    }
    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        durationTracker = 0;
        Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Parry].Consume();
    }
    
    public override void PhysicsProcess()
    {
        float gravity;
        if (Player.RigidBody.linearVelocity.y > 0)
        {
            gravity = Player.PlayerStats.GroundedJumpInfo.JumpVelocity;
        }
        else
        {
            gravity = Player.PlayerStats.GroundedJumpInfo.FallGravity;
        }
        ApplyGravity(gravity);
        Vector3 movementDirection = Player.PlayerInput.GetMovementDirection();
        AirborneMovement(movementDirection, Player.PlayerStats.ParryStrafeSpeed);
        CheckForWall(movementDirection);
        durationTracker++;

        if (durationTracker == Player.PlayerStats.ProperParryDuration + Player.PlayerStats.PartialParryDuration)
        {
            StateMachine.TransitionTo<PlayerFallState>();
        }
    }

    void CheckForWall(Vector3 movementDirection)
    {
        var raycastLength = Player.PlayerStats.MinParryRange + (Player.PlayerStats.ParryRangeIncreaseWithSpeedRatio * Player.RigidBody.linearVelocity.magnitude);
        Ray ray = new (Player.Collider.bounds.center, Player.RigidBody.linearVelocity.normalized);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, raycastLength, parryMask, QueryTriggerInteraction.Collide))
        {
            Player.RigidBody.MovePosition(hitInfo.point);
            PerformParry(movementDirection, hitInfo.normal);
            StateMachine.TransitionTo<PlayerFallState>();
        }
    }

    void PerformParry(Vector3 movementDirection, Vector3 normal)
    {
        float bounceVelocity = Player.RigidBody.linearVelocity.magnitude + (Player.RigidBody.linearVelocity.magnitude * Player.PlayerStats.ParrySpeedIncrease);
        if (durationTracker > Player.PlayerStats.ProperParryDuration)
        {
            bounceVelocity *= Player.PlayerStats.PartialParrySpeedPenalty;
        }
        Vector3 velocityReflected = Vector3.Reflect(Player.RigidBody.linearVelocity, normal).normalized;
        Vector3 velocityRotated = Vector3.Lerp(velocityReflected, movementDirection.normalized, Player.PlayerStats.ParryBounceControl);

        Player.RigidBody.linearVelocity = velocityRotated * bounceVelocity;
    }

    public override bool StateAvailable()
    {
        return Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Parry].Buffered && !Player.PlayerGrounded;
    }
}
