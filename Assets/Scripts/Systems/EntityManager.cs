using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance { get; private set; }
    Dictionary<IDComponent, BaseEntity> entityRegistry = new ();
    List<BaseEntity> entityList = new();
    public int PlayerID { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Multiple instances of EntityManager detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        var entities = FindObjectsByType<BaseEntity>(sortMode: FindObjectsSortMode.InstanceID);
        foreach(var entity in entities)
        {
            if (entity.IDComponent == null)
            {
                Debug.LogError($"Entity {entity.name} does not have an IDComponent attached. This entity will not be registered with the EntityManager.");
                continue;
            }
            if (!entityRegistry.ContainsKey(entity.IDComponent))
            {
                entityRegistry.Add(entity.IDComponent, entity);
                entity.entityDestroyed += OnEntityDestroyed;
                entityList.Add(entity);
            }
            else
            {
                Debug.LogWarning($"Duplicate IDComponent found on {entity.name}. This entity will not be registered.");
            }
        }
    }
    private void FixedUpdate()
    {
        foreach (var entity in entityList)
        {
            if (entity.enabled)
            {
                entity.PhysicsProcess();
            }
        }
    }

    private void Update()
    {
        foreach (var entity in entityList)
        {
            if (entity.enabled)
            {
                entity.Process();
            }
        }
    }

    void OnEntityDestroyed(BaseEntity entity)
    {
        if (entityRegistry.ContainsValue(entity))
        {
            entityRegistry.Remove(entity.IDComponent);
            entityList.Remove(entity);
        }
    }

    public void RegisterEntity(BaseEntity entity)
    {
        if (entity == null) return;
        if (!entityRegistry.ContainsKey(entity.IDComponent))
        {
            entityRegistry.Add(entity.IDComponent, entity);
            entityList.Add(entity);
        }
    }
}
