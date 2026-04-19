

using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedMovementState
{
    public override System.Type[] statesToAttemptToTransitionToEveryFrame
    {
        get => new System.Type[]
        {

            typeof(PlayerSwingState),
            typeof(PlayerThrowWormState),
            typeof(PlayerJumpState),
            typeof(PlayerRunState),
        };
    }
    protected override void GroundedMovement()
    {
        PlayerGrounded = IsGrounded();
        if (!PlayerGrounded)
        {
            StateMachine.TransitionTo<PlayerFallState>();
            return;
        }
        Player.RigidBody.linearVelocity = Vector3.MoveTowards(Player.RigidBody.linearVelocity, Vector3.zero, Player.PlayerStats.DecelerationDrag);
    }
    public override bool StateAvailable()
    {
        Vector2 movementDirection = Player.PlayerInput.GetMovementDirection();
        return PlayerGrounded && movementDirection.magnitude <= MOVEMENT_DEADZONE;
    }

}