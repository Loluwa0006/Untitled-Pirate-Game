using System.Collections.Generic;
using UnityEngine;

public class PlayerShadowstepState : PlayerBaseState
{
    float initialSpeed;

    int durationTracker = 0;
    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        initialSpeed = new Vector2(Player.RigidBody.linearVelocity.x, Player.RigidBody.linearVelocity.z).magnitude;
        if (initialSpeed < Player.PlayerStats.MinimumShadowstepSpeed) initialSpeed = Player.PlayerStats.MinimumShadowstepSpeed;
        durationTracker = Player.PlayerStats.DurationPerSquashbucklerCharge * Player.PlayerStats.ChargesToEnterSquashbucklerMode;
        Player.SquashbucklerManager.SquashbucklerCharge -= Player.PlayerStats.ChargesToEnterSquashbucklerMode;
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
        }
        if (Player.SquashbucklerManager.SquashbucklerCharge == 0)
        {
            StateMachine.TransitionTo<PlayerFallState>();
            return;
        }
    }

    public override void Process()
    {
        if (!Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Squashbuckler].ActionPressed)
        {
            StateMachine.TransitionTo<PlayerFallState>();
            return;
        }
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
