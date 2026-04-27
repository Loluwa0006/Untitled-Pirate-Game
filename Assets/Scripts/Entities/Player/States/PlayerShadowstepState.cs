using System.Collections.Generic;
using UnityEngine;

public class PlayerShadowstepState : PlayerBaseState
{
    float initialSpeed;

    int durationTracker = 0;

    bool startedAtMaxCharge = false;
    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        startedAtMaxCharge = Player.SquashbucklerManager.SquashbucklerCharge == Player.SquashbucklerManager.MaxCharge;
        initialSpeed = new Vector2(Player.RigidBody.linearVelocity.x, Player.RigidBody.linearVelocity.z).magnitude;
        if (initialSpeed < Player.PlayerStats.MinimumShadowstepSpeed) initialSpeed = Player.PlayerStats.MinimumShadowstepSpeed;
        durationTracker = Player.PlayerStats.DurationPerSquashbucklerCharge * Player.PlayerStats.ChargesToEnterSquashbucklerMode;
        Player.SquashbucklerManager.SquashbucklerCharge -= Player.PlayerStats.ChargesToEnterSquashbucklerMode;
        Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Squashbuckler].Consume();
        Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Slash].Consume(); //prevent accidental dragonslashes
    }
    public override void PhysicsProcess()
    {
        base.PhysicsProcess();
        Player.RigidBody.linearVelocity = initialSpeed * viewCamera.transform.forward;
        durationTracker--;
        if (durationTracker == 0)
        {
            Player.SquashbucklerManager.SquashbucklerCharge--;
            durationTracker = Player.PlayerStats.DurationPerSquashbucklerCharge;
            if (!Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Squashbuckler].ActionPressed)
            {
                FallFromShadowstep();
                return;
            }
        }
        if (Player.SquashbucklerManager.SquashbucklerCharge == 0)
        {
            FallFromShadowstep();
            return;
        }
        if (startedAtMaxCharge && Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Slash].Buffered)
        {
            StateMachine.TransitionTo<PlayerDragonslashState>();
            return;
        }
    }

    void FallFromShadowstep()
    {
        Player.Animator.SetBool(Player.GetAnimationParameterFormatted(PlayerController.AnimationParameter.Bool_InSquashbuckler), false);
        StateMachine.TransitionTo<PlayerFallState>();
    }

    public override void Process()
    {
  
    }

    public override void Exit()
    {
        base.Exit();
        Player.AnarchyManager.GenerateAnarchy(ScaledGenerationMethod.Shadowstep);
    }
    public override bool StateAvailable()
    {
        return Player.SquashbucklerManager.SquashbucklerCharge > Player.PlayerStats.ChargesToEnterSquashbucklerMode 
               && Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Squashbuckler].Buffered;
    }
}
