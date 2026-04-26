using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerYawnState : PlayerAirState
{

    int justYawnTracker = 0;

    int elaspedYawnTime;
    int minYawnTime;

    public override Type[] statesToAttemptToTransitionTo
    {
        get => new Type[]
        {
            typeof(PlayerShadowstepState),
        };
    }

    public override void InitializeState(EntityStateMachine stateMachine, Transform owner)
    {
        base.InitializeState(stateMachine, owner);
        Player.AnarchyManager.anarchyGainedThroughScaledMethod.AddListener((method, charges) => OnAnarchyGenerated());
    }
    void OnAnarchyGenerated()
    {
        justYawnTracker = Player.PlayerStats.JustYawnWindow;
    }

    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        if (justYawnTracker > 0)
        {
            OnJustYawn();
        }
        else
        {
            minYawnTime = Player.PlayerStats.MinYawnTime;
        }
        Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Yawn].Consume();
        elaspedYawnTime = 0;
    }
    void OnJustYawn()
    {
        minYawnTime = Player.PlayerStats.MinJustYawnTime;
        Player.AnarchyManager.GenerateAnarchyUnscaled(UnscaledGenerationMethod.JustYawn);
    }

    public override void PhysicsProcess()
    {
        elaspedYawnTime++;
        if (elaspedYawnTime >= minYawnTime  && !Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Yawn].ActionPressed) 
        {
            StateMachine.TransitionTo<PlayerFallState>();
            return;
        }
        Player.AnarchyManager.GenerateAnarchyUnscaled(UnscaledGenerationMethod.Yawn);
        float gravity;
        if (Player.RigidBody.linearVelocity.y > 0) gravity = Player.PlayerStats.GroundedJumpInfo.JumpGravity;
        else gravity = Player.PlayerStats.GroundedJumpInfo.FallGravity;
        ApplyGravity(gravity);
        AirborneMovement(Player.PlayerInput.GetMovementDirection(), Player.PlayerStats.YawnAirAcceleration);
    }
    public override void InactivePhysicsProcess()
    {
        justYawnTracker = (int) Mathf.MoveTowards(justYawnTracker, 0, 1);     
    }
    public override bool StateAvailable()
    {
        return !Player.PlayerGrounded && Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Yawn].Buffered;
    }
}
