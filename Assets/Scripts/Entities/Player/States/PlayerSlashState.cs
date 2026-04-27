using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSlashState : PlayerAirState
{
    [SerializeField] HitboxComponent slashHitbox;

    [HideInInspector] public bool animationOver = false;

    public override Type[] statesToAttemptToTransitionTo
    {
        get => new Type[]
        {

        };
    }

    public override void InitializeState(EntityStateMachine stateMachine, Transform owner)
    {
        base.InitializeState(stateMachine, owner);
        if (slashHitbox == null) slashHitbox = GetComponent<HitboxComponent>();
        slashHitbox.targetsStruck += OnHitboxDeactivation;
    }
    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        Player.Animator.SetTrigger("IsAttacking");
        animationOver = false;
    }

    public override void PhysicsProcess()
    {
        base.PhysicsProcess();
        AirborneMovement(Player.PlayerInput.GetMovementDirection(), Player.PlayerStats.AirAcceleration);
        //use jump gravity to make attacks feel more floaty
        ApplyGravity(Player.PlayerStats.GroundedJumpInfo.JumpGravity);
        if (animationOver)
        {
            StateMachine.TransitionTo<PlayerFallState>();
            return;
        }
        if (StateMachine.IsStateAvailable<PlayerShadowstepState>())
        {
            StateMachine.TransitionTo<PlayerShadowstepState>();
            return;
        }
    }

    public void OnHitboxDeactivation(List<HealthComponent> victims)
    {
        for (int i = 0; i < victims.Count; i++)
        {
            Player.AnarchyManager.GenerateAnarchyUnscaled(UnscaledGenerationMethod.Slash);
        }
    }

    public override void Process()
    {
        //no state cancelling
    }
    public override void Exit()
    {
        base.Exit();
        slashHitbox.OnDeactivate();
    }

    public override bool StateAvailable()
    {
        return Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Slash].Buffered;
    }
}
