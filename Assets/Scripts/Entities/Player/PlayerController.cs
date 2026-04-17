using UnityEngine;
public class PlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] InputManager _playerInput;
    [SerializeField] EntityStateMachine playerStateMachine;
    [SerializeField] Rigidbody _rb;
    [SerializeField] VelocityComponent _velocityComponent;
    [SerializeField] PlayerStats _playerStats;
    [SerializeField] Collider _collider;

    public InputManager PlayerInput { get => _playerInput; private set => _playerInput = value; }
    public Rigidbody RigidBody { get => _rb; private set => _rb = value; }

    public VelocityComponent VelocityComponent { get => _velocityComponent; private set => _velocityComponent = value; }

    public PlayerStats PlayerStats { get => _playerStats; private set => _playerStats = value; }

    public Collider Collider { get => _collider; private set => _collider = value; }

    void Update()
    {
        playerStateMachine.Process();
    }

    private void FixedUpdate()
    {
        playerStateMachine.PhysicsProcess();
    }
}
