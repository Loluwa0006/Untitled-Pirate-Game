using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class TestEnemy : BaseEnemy
{
    public enum FireState
    {
        Preparing,
        Reloading,
    }
    public enum ProjectileInfoID
    {
        SniperShot,
        RapidFire
    }

    [System.Serializable]
    public struct TestEnemyProjectileInformation
    {
        public ProjectileInfoID ID;
        public ProjectileFireInformation fireInformation;
        public Queue<BaseProjectile> projectilePool;
    }

    [SerializeField] PlayerController player;
    [SerializeField] List<TestEnemyProjectileInformation> testEnemyProjectileInfos = new();
    [SerializeField] float distanceToUseSniperFireAt = 100.0f;

    Dictionary<ProjectileInfoID, TestEnemyProjectileInformation> projectileRegistry = new();

    ProjectileInfoID queuedProjectileFire;
    FireState fireState = FireState.Reloading;
    int delayRemaining = 0;
    int cooldownRemaining = 0;
    
    private void Start()
    {
        for (int i = 0; i < testEnemyProjectileInfos.Count; i++)
        {
            Queue<BaseProjectile> projectilePool = new();
            var nextInfo = testEnemyProjectileInfos[i];
            for (int x = 0; x < nextInfo.fireInformation.poolSize; x++)
            {
                var newProjectile = Instantiate(nextInfo.fireInformation.projectilePrefab);
                newProjectile.InitializeProjectile(this);
                newProjectile.DestroyProjectile();
                projectilePool.Enqueue(newProjectile);
            }
            nextInfo.projectilePool = projectilePool;
            testEnemyProjectileInfos[i] = nextInfo;
            projectileRegistry[nextInfo.ID] = nextInfo;
            Debug.Log("Setting up projectile " + nextInfo.ID);
        }
    }
    public void PrepareToFire(ProjectileInfoID projectileType)
    {
        SetFireType();
        fireState = FireState.Preparing;
        queuedProjectileFire = projectileType;
        delayRemaining = projectileRegistry[projectileType].fireInformation.delayBeforeFiring;
    }

    void FireProjectile()
    {
        fireState = FireState.Reloading;
        var nextProjectile = projectileRegistry[queuedProjectileFire].projectilePool.Dequeue();
        nextProjectile.EnableProjectile(player.transform);
        projectileRegistry[queuedProjectileFire].projectilePool.Enqueue(nextProjectile);
        cooldownRemaining = projectileRegistry[queuedProjectileFire].fireInformation.fireCooldown;
    }

    void SetFireType()
    {
        var distance = Vector3.Distance(player.RigidBody.position, RigidBody.position);
        if (distance < distanceToUseSniperFireAt)
        {
            queuedProjectileFire = ProjectileInfoID.RapidFire;
        }
        else
        {
            queuedProjectileFire = ProjectileInfoID.SniperShot;
        }
    }
    private void FixedUpdate()
    {
       
        if (fireState == FireState.Preparing)
        {
            delayRemaining = (int)Mathf.MoveTowards(delayRemaining, 0, 1);
            if (delayRemaining < 0.001f)
            {
                FireProjectile();
            }
        }
        else if (fireState == FireState.Reloading)
        {
            cooldownRemaining = (int)Mathf.MoveTowards(cooldownRemaining, 0, 1);
            if (cooldownRemaining < 0.001f)
            {
                PrepareToFire(queuedProjectileFire);
            }
        }
       
    }



}
