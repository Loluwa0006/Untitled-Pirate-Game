using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] PlayerStats _playerStats;
    [SerializeField] Collider _collider;

    [Header("Managers")]
    [SerializeField] InputManager _playerInput;
    [SerializeField] WormManager _wormManager;
    [SerializeField] RodManager _rodManager;
    [SerializeField] AnarchyManager _anarchyManager;
    [SerializeField] SquashbucklerManager _squashbucklerManager;
    [SerializeField] EntityStateMachine playerStateMachine;

    [Header("Components")]
    [SerializeField] PlayerHealthComponent _healthComponent;
    [SerializeField] Animator _animator;


    public InputManager PlayerInput { get => _playerInput; private set => _playerInput = value; }
    public Rigidbody RigidBody { get => _rb; private set => _rb = value; }


    public PlayerStats PlayerStats { get => _playerStats; private set => _playerStats = value; }

    public Collider Collider { get => _collider; private set => _collider = value; }

    public WormManager WormManager { get => _wormManager; private set => _wormManager = value; }

    public RodManager RodManager { get => _rodManager; private set => _rodManager = value; }

    public AnarchyManager AnarchyManager { get => _anarchyManager; private set => _anarchyManager = value; }

    public SquashbucklerManager SquashbucklerManager { get => _squashbucklerManager; }

    public HealthComponent HealthComponent { get => _healthComponent; }

    public Animator Animator { get => _animator; }

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

    public void OnPlayerDamaged(HitboxContactInfo info)
    {
        if (info.DamageInfo.damage <= 0) return;
        Dictionary<string, object> getHitStateMessage = new()
        {
            [PlayerGetHitState.PlayerGetHitMessage.ContactInfo.ToString()] = info
        };
        playerStateMachine.TransitionTo<PlayerGetHitState>(getHitStateMessage);
    }
}
