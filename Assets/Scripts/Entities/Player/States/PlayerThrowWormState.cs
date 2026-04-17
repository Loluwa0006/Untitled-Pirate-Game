using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowWormState : PlayerAirState
{
    [SerializeField] float throwRange = 120;

    [SerializeField] int stateDuration = 19;
    [SerializeField] LayerMask terrainMask;
    
    Camera gameCamera;

    Vector3 middlePointOfViewport = new (0.5f, 0.5f);

    int durationTracker = 0;
    public override void InitializeState(EntityStateMachine stateMachine, Transform owner)
    {
        base.InitializeState(stateMachine, owner);
        gameCamera = Camera.main;
    }
    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        FireWorm();
        var currentSpeed = Player.VelocityComponent.GetInternalSpeed();
        currentSpeed.y = Player.PlayerStats.WormThrowJumpInfo.JumpVelocity;
        Player.VelocityComponent.OverwriteInternalSpeed(currentSpeed);
        durationTracker = stateDuration;
    }

    void FireWorm()
    {
        var cameraRay = gameCamera.ViewportPointToRay(middlePointOfViewport);
        var raycast = Physics.Raycast(cameraRay, out var hitInfo, throwRange, terrainMask, QueryTriggerInteraction.Collide);
        Vector3 wormTarget;
        if (raycast)
        {
            wormTarget = hitInfo.point;
        }
        else
        {
            wormTarget = cameraRay.GetPoint(throwRange);
        }
        WormEntity newWorm = Player.WormManager.GetNewWorm();
        newWorm.Fire(wormTarget, Player.transform.position);
    }

    public override void PhysicsProcess()
    {
        base.PhysicsProcess();
        var currentSpeed = Player.VelocityComponent.GetInternalSpeed().y;
        if (currentSpeed > 0)
        {
            ApplyGravity(Player.PlayerStats.WormThrowJumpInfo.JumpGravity);
        }
        else
        {
            ApplyGravity(Player.PlayerStats.WormThrowJumpInfo.FallGravity);
        }
        durationTracker--;
        if (durationTracker == 0)
        {
            if (!AttemptGroundTransition())
            {
                StateMachine.TransitionTo<PlayerFallState>();
                return;
            }
        }
    }
}
