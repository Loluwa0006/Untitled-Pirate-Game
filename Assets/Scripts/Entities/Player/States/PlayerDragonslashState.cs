using System.Collections.Generic;
using UnityEngine;

public class PlayerDragonslashState : PlayerSlashState
{
    float initialSpeed;
    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        Player.Animator.SetBool(Player.GetAnimationParameterFormatted(PlayerController.AnimationParameter.Bool_InSquashbuckler), true);
        initialSpeed = new Vector2(Player.RigidBody.linearVelocity.x, Player.RigidBody.linearVelocity.z).magnitude;
    }

    public override void PhysicsProcess()
    {
        if (animationOver)
        {
            StateMachine.TransitionTo<PlayerFallState>();
            return;
        }
        Player.RigidBody.linearVelocity = initialSpeed * viewCamera.transform.forward;
        Player.RigidBody.MoveRotation(Quaternion.LookRotation(viewCamera.transform.forward));
        CalculateDamage(Player.PlayerStats.MinDragonslashDamage, Player.PlayerStats.MaxDragonslashDamage, Player.PlayerStats.SpeedToDragonslashDamageCurve);
    }

    public override void OnHitboxDeactivation(List<HealthComponent> victims)
    {
        for (int i = 0; i < victims.Count; i++)
        {
            Player.AnarchyManager.GenerateAnarchyUnscaled(UnscaledGenerationMethod.Dragonslash);
        }
    }

    public override bool StateAvailable()
    {
        return Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Slash].Buffered;
    }

    public override void Exit()
    {
        base.Exit();
        Player.Animator.SetBool(Player.GetAnimationParameterFormatted(PlayerController.AnimationParameter.Bool_InSquashbuckler), false);
    }

}

