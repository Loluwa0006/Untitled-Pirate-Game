using UnityEngine;

public class PlayerGroundedMovementState : PlayerBaseState
{

    Camera viewCamera;

    public override void InitializeState(EntityStateMachine stateMachine, Transform owner)
    {
        base.InitializeState(stateMachine, owner);
        viewCamera = Camera.main;
    }
    protected virtual void GroundedMovement()
    {
        if (!IsGrounded())
        {
            StateMachine.TransitionTo<PlayerFallState>();
            return;
        }
        if (Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Jump].Buffered)
        {
            Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Jump].Consume();
            StateMachine.TransitionTo<PlayerJumpState>();
            return;
        }
        Vector2 movementDirection = Player.PlayerInput.GetMovementDirection();
        if (movementDirection.magnitude < MOVEMENT_DEADZONE)
        {
            StateMachine.TransitionTo<PlayerIdleState>();
            return;
        }
        movementDirection = movementDirection.normalized;
        Vector3 newSpeed = Player.VelocityComponent.GetInternalSpeed();
    
        Vector3 moveDirection = movementDirection.x * viewCamera.transform.right + movementDirection.y * viewCamera.transform.forward;
        newSpeed += new Vector3(moveDirection.x * Player.PlayerStats.GroundAcceleration, 0, moveDirection.z * Player.PlayerStats.GroundAcceleration);
        newSpeed = Vector3.ClampMagnitude(newSpeed, Player.PlayerStats.MoveSpeed);
        
        Player.VelocityComponent.OverwriteInternalSpeed(newSpeed);
    }
    public override void PhysicsProcess()
    {
        GroundedMovement();
    }
}
