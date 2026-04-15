using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunState : PlayerBaseState
{
    float deadzone = 0.1f;
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody _rb;
    [SerializeField] InputActionReference moveReference;

    Vector3 movementDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    public override void Process()
    {
        movementDirection = moveReference.action.ReadValue<Vector2>();
    }

    public override void PhysicsProcess()
    {
        if (movementDirection.magnitude < deadzone)
        {
            StateMachine.TransitionTo<PlayerIdleState>();
            return;
        }
        Vector3 movement = movementDirection.x * transform.right + movementDirection.y * transform.forward;
        Player.VelocityComponent.OverwriteInternalSpeed(new Vector3(movement.x * moveSpeed, _rb.linearVelocity.y, movement.z * moveSpeed));
    }
}
