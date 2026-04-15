using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerBaseState
{
    [SerializeField] float decelRate;

    [SerializeField] InputActionReference moveReference;
    Vector3 movementDirection;

    public override void Process()
    {
        movementDirection = moveReference.action.ReadValue<Vector2>();
    }

    public override void PhysicsProcess()
    {
        if (movementDirection.magnitude > 0.1f)
        {
            StateMachine.TransitionTo<PlayerRunState>();
            return;
        }

        var previous = Player.VelocityComponent.GetInternalSpeed();
        previous.x *= decelRate;
        previous.z *= decelRate;
        Player.VelocityComponent.OverwriteInternalSpeed(previous);
    }
}