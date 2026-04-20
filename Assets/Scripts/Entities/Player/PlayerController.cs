using UnityEngine;
using UnityEngine.Events;
public class PlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] EntityStateMachine playerStateMachine;
    [SerializeField] Rigidbody _rb;
    [SerializeField] PlayerStats _playerStats;
    [SerializeField] Collider _collider;

    [Header("Managers")]
    [SerializeField] InputManager _playerInput;
    [SerializeField] WormManager _wormManager;
    [SerializeField] RodManager _rodManager;

    public InputManager PlayerInput { get => _playerInput; private set => _playerInput = value; }
    public Rigidbody RigidBody { get => _rb; private set => _rb = value; }


    public PlayerStats PlayerStats { get => _playerStats; private set => _playerStats = value; }

    public Collider Collider { get => _collider; private set => _collider = value; }

    public WormManager WormManager { get => _wormManager; private set => _wormManager = value; }

    public RodManager RodManager { get => _rodManager; private set => _rodManager = value; }

    public bool PlayerGrounded { get; set; }

    public UnityEvent<Collision> playerCollision = new();
    void Update()
    {
        playerStateMachine.Process();
    }

    private void FixedUpdate()
    {
        playerStateMachine.PhysicsProcess();
    }

    private void OnCollisionEnter(Collision collision)
    {
        playerCollision.Invoke(collision);
    }
}
