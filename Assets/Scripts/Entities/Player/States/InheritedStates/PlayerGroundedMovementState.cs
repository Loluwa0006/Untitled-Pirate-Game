using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedMovementState : PlayerBaseState
{

    Camera viewCamera;

    public override Type[] statesToAttemptToTransitionTo
    {
        get => new Type[]
        {

            typeof(PlayerSwingState),
            typeof(PlayerThrowWormState),
            typeof(PlayerJumpState),
            typeof(PlayerRunState),
            typeof(PlayerIdleState),
        };
    }
    public override void InitializeState(EntityStateMachine stateMachine, Transform owner)
    {
        base.InitializeState(stateMachine, owner);
        viewCamera = Camera.main;
    }

 
    protected virtual void GroundedMovement()
    {
        if (!PlayerGrounded)
        {
            StateMachine.TransitionTo<PlayerFallState>();
            return;
        }
        Vector2 movementDirection = Player.PlayerInput.GetMovementDirection();
        movementDirection = movementDirection.normalized;
        Vector2 currentSpeed =  new Vector2(Player.RigidBody.linearVelocity.x, Player.RigidBody.linearVelocity.z);
    
        Vector3 moveDirection = movementDirection.x * viewCamera.transform.right + movementDirection.y * viewCamera.transform.forward;
        Vector2 lateralAddition = new Vector2(moveDirection.x * Player.PlayerStats.GroundAcceleration, moveDirection.z * Player.PlayerStats.GroundAcceleration);
        

        if (currentSpeed.magnitude >= Player.PlayerStats.MoveSpeed)
        {
            var speedNormalized = currentSpeed.normalized;
            var extraSpeed = Vector2.Dot(lateralAddition, speedNormalized);
            if (extraSpeed > 0)
            {
                lateralAddition -= extraSpeed * speedNormalized;
            }
        }

        Player.RigidBody.AddForce(new Vector3(lateralAddition.x, 0, lateralAddition.y), ForceMode.VelocityChange);
    }
    public override void PhysicsProcess()
    {
        PlayerGrounded = IsGrounded();
        GroundedMovement();
    }


}
