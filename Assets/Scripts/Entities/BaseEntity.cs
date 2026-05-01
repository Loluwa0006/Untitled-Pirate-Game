using UnityEngine;
using UnityEngine.Events;

public class BaseEntity : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    public Rigidbody RigidBody { get => _rb; }

    [SerializeField] Collider _collider;

    public Collider Collider { get => _collider; }

    [SerializeField] protected EntityStateMachine stateMachine;

    [SerializeField] HealthComponent _healthComponent;

    public HealthComponent HealthComponent { get => _healthComponent; }



    public UnityEvent<Collision> entityCollision = new();
    public UnityEvent<Collider> entityTriggerEntry = new();

    private void OnCollisionEnter(Collision collision)
    {
        entityCollision.Invoke(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        entityTriggerEntry.Invoke(other);
    }
}
