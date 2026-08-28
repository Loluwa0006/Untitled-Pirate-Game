
using System.Collections.Generic;

public class LeviathanEntity : BaseEnemy
{

    public enum AnimationParameter
    {
        Trigger_IsAttacking,
        Bool_InDreamphase
    }
    public override void Initialize()
    {
        base.Initialize();
        for (int i = 0; i < HealthFragments.Count; i++)
        {
            HealthFragments[i].entityKilled.AddListener(OnFragmentDestroyed);
        }
    }
    public override void Process()
    {
        base.Process();
        stateMachine.Process();
    }

    public override void PhysicsProcess()
    {
        base.PhysicsProcess();
        stateMachine.PhysicsProcess();
    }

    public string GetAnimationParameterFormatted(AnimationParameter parameter)
    {
        var parameterString = parameter.ToString();
        parameterString = parameterString.Substring(parameterString.IndexOf("_") + 1);
        return parameterString;
    }

    public override void OnEntityDamaged(HitboxContactInfo info)
    {
        if (info.DamageInfo.damage < 1) return;
        stateMachine.TransitionTo<LeviathanMoveState>();
    }

    public void OnFragmentDestroyed()
    {
        if (InDreamphase) return;
        for (int i = 0; i < HealthFragments.Count; i++)
        {
           if (HealthFragments[i].Health > 0)
            {
                return;
            }
        }
        for (int i = 0;i < HealthFragments.Count; i++)
        {
            HealthFragments[i].Heal(HealthFragments[i].MaxHealth);
        }

        InDreamphase = true;

    }



}
