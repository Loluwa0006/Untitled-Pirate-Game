using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerThrowWormState : PlayerAirState
{ 
    [SerializeField] LayerMask terrainMask;
    
    Vector3 middlePointOfViewport = new (0.5f, 0.5f);

    int durationTracker = 0;

    public override Type[] statesToAttemptToTransitionTo
    {
        get => new Type[]
        {
            typeof(PlayerSwingState),
            typeof (PlayerDashState),

        };
        protected set => base.statesToAttemptToTransitionTo = value;
    }
 
    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        FireWorm();
        Vector3 newSpeed = Player.RigidBody.linearVelocity;
        newSpeed.y = Mathf.Max(newSpeed.y + Player.PlayerStats.WormThrowJumpInfo.JumpVelocity, Player.PlayerStats.WormThrowJumpInfo.JumpVelocity);
        Player.RigidBody.linearVelocity = newSpeed;
        durationTracker = Player.PlayerStats.WormThrowDuration;
        Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.FireWorm].Consume();
        Player.WormManager.WormsRemaining--;
    }

    void FireWorm()
    {
        var cameraRay = viewCamera.ViewportPointToRay(middlePointOfViewport);
        var raycast = Physics.Raycast(cameraRay, out var hitInfo, Player.PlayerStats.WormThrowRange, terrainMask, QueryTriggerInteraction.Collide);
        Vector3 wormTarget;
        if (raycast)
        {
            wormTarget = hitInfo.point;
        }
        else
        {
            wormTarget = cameraRay.GetPoint(Player.PlayerStats.WormThrowRange);
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
        if (durationTracker == 0)
        {
            StateMachine.TransitionTo<PlayerFallState>();
            return;
        }
        Player.PlayerGrounded = IsGrounded();
        if (Player.PlayerGrounded)
        {
            if (StateMachine.IsStateAvailable<PlayerRunState>())
            {
                StateMachine.TransitionTo<PlayerRunState>();
            }
            else
            {
                StateMachine.TransitionTo<PlayerIdleState>();
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        Player.AnarchyManager.GenerateAnarchy(ScaledGenerationMethod.WormThrow);
    }

    public override bool StateAvailable()
    {
        if (Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.FireWorm].Buffered && Player.WormManager.WormsRemaining > 0)
        {
            return true;
        }
        return false;
    }
}
