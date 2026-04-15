using UnityEngine;

public class PlayerBaseState : BaseState
{
    public PlayerController Player { private set; get; }

    public override void InitializeState(EntityStateMachine stateMachine, Transform owner)
    {
        base.InitializeState(stateMachine, owner);
        Player = owner.GetComponent<PlayerController>();
    }
}
