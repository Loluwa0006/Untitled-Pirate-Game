using UnityEngine;
public class PlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] EntityStateMachine playerStateMachine;
    [SerializeField] Rigidbody _rb;
    [SerializeField] PlayerStats _playerStats;
    [SerializeField] Collider _collider;

    [Header("Managers")]
    [SerializeField] VelocityComponent _velocityComponent;
    [SerializeField] InputManager _playerInput;
    [SerializeField] WormManager _wormManager;

    public InputManager PlayerInput { get => _playerInput; private set => _playerInput = value; }
    public Rigidbody RigidBody { get => _rb; private set => _rb = value; }

    public VelocityComponent VelocityComponent { get => _velocityComponent; private set => _velocityComponent = value; }

    public PlayerStats PlayerStats { get => _playerStats; private set => _playerStats = value; }

    public Collider Collider { get => _collider; private set => _collider = value; }

    public WormManager WormManager { get => _wormManager; private set => _wormManager = value; }

    void Update()
    {
        playerStateMachine.Process();
    }

    private void FixedUpdate()
    {
        playerStateMachine.PhysicsProcess();
    }
}
