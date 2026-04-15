using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] EntityStateMachine playerStateMachine;
    [SerializeField] Rigidbody _rb;
    [SerializeField] VelocityComponent _velocityComponent;

    public PlayerInput PlayerInput { get => _playerInput; private set => _playerInput = value; }
    public Rigidbody RigidBody { get => _rb; private set => _rb = value; }

    public VelocityComponent VelocityComponent { get => _velocityComponent; private set => _velocityComponent = value; }

    void Update()
    {
        playerStateMachine.Process();
    }

    private void FixedUpdate()
    {
        playerStateMachine.PhysicsProcess();
    }
}
