using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerThrowWormState : PlayerAirState
{
    [SerializeField] float throwRange = 120;

    [SerializeField] int stateDuration = 19;
    [SerializeField] LayerMask terrainMask;
    
    Camera gameCamera;

    Vector3 middlePointOfViewport = new (0.5f, 0.5f);

    int durationTracker = 0;

    public override Type[] statesToAttemptToTransitionToEveryFrame
    {
        get => new Type[]
        {
            typeof(PlayerSwingState),
            typeof(PlayerFallState),
            typeof(PlayerRunState),
            typeof(PlayerIdleState)

        };
        protected set => base.statesToAttemptToTransitionToEveryFrame = value;
    }
    public override void InitializeState(EntityStateMachine stateMachine, Transform owner)
    {
        base.InitializeState(stateMachine, owner);
        gameCamera = Camera.main;
    }
    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        FireWorm();
        Vector3 newSpeed = Player.RigidBody.linearVelocity;
        newSpeed.y = Player.PlayerStats.WormThrowJumpInfo.JumpVelocity;
        Player.RigidBody.linearVelocity = newSpeed;
        durationTracker = stateDuration;
        Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.FireWorm].Consume();
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
        newWorm.Fire(wormTarget, Player.transform.position, Player.RigidBody.linearVelocity);
    }

    public override void PhysicsProcess()
    {

        base.PhysicsProcess();
        if (Player.RigidBody.linearVelocity.y > 0)
        {
            ApplyGravity(Player.PlayerStats.WormThrowJumpInfo.JumpGravity);
        }
        else
        {
            ApplyGravity(Player.PlayerStats.WormThrowJumpInfo.FallGravity);
        }
        AirborneMovement(Player.PlayerInput.GetMovementDirection(), Player.PlayerStats.AirAcceleration);
        durationTracker--;
    }

    public override void Process()
    {
        if (durationTracker == 0)
        {
            PlayerGrounded = IsGrounded();
            CheckIfPerFrameStateTransitionRequired();
        }
    }

    public override bool StateAvailable()
    {
        if (Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.FireWorm].Buffered)
        {
            return true;
        }
        return false;
    }
}
