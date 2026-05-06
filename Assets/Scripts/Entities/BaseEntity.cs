using System;
using UnityEngine;
using UnityEngine.Events;

public class BaseEntity : MonoBehaviour
{
    [SerializeField] IDComponent idComponent;

    public IDComponent IDComponent { get => idComponent; }

    public Action<BaseEntity> entityDestroyed;

    public virtual void Process()
    {

    }

    public virtual void PhysicsProcess()
    {

    }

    private void OnEnable()
    {
        EntityManager.Instance.RegisterEntity(this);
    }

    private void OnDestroy()
    {
        entityDestroyed?.Invoke(this);
    }
}
