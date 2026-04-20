using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParryState : PlayerAirState
{

    [SerializeField] LayerMask parryMask;
    int durationTracker = 0;

    float startingSpeed = 0.0f;

    public override Type[] statesToAttemptToTransitionTo
    {
        get => new Type[]
        {
            
        };
    }
    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        startingSpeed = Player.RigidBody.linearVelocity.magnitude;
        Player.playerCollision.AddListener(OnPlayerCollision);
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
        durationTracker++;

        if (durationTracker == Player.PlayerStats.ProperParryDuration + Player.PlayerStats.PartialParryDuration)
        {
            StateMachine.TransitionTo<PlayerFallState>();
        }
    }

  
    void OnPlayerCollision(Collision collision)
    {
        PerformParry(Player.PlayerInput.GetMovementDirection(), collision.GetContact(0).normal);
    }
    void PerformParry(Vector3 movementDirection, Vector3 normal)
    {
        float bounceVelocity = startingSpeed + (startingSpeed * Player.PlayerStats.ParrySpeedIncrease);
        if (durationTracker > Player.PlayerStats.ProperParryDuration)
        {
            bounceVelocity *= Player.PlayerStats.PartialParrySpeedPenalty;
        }
        Vector3 velocityReflected = Vector3.Reflect(Player.RigidBody.linearVelocity.normalized, normal).normalized;
        Vector3 velocityRotated = Vector3.Lerp(velocityReflected, movementDirection.normalized, Player.PlayerStats.ParryBounceControl);

        Player.RigidBody.linearVelocity = velocityRotated * bounceVelocity;
    }
    public override void Exit()
    {
        base.Exit();
        Player.playerCollision.RemoveListener(OnPlayerCollision);
        Player.AnarchyManager.GenerateAnarchy(AnarchyManager.AnarchyGenerationMethod.Parry);
    }
    public override bool StateAvailable()
    {
        return Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Parry].Buffered && !Player.PlayerGrounded;
    }
}
