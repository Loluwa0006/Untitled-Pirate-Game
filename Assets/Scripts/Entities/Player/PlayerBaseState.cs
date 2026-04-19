using UnityEngine;

public class PlayerBaseState : BaseState
{
    public const float MOVEMENT_DEADZONE = 0.1f;

    public const float GROUND_CHECK_SAFE_MARGIN = 0.15f;

    const float SHAPECAST_RATIO = 0.8f;

   protected static LayerMask groundMask;
    protected static LayerMask swingMask;

    public PlayerController Player { private set; get; }


    public virtual System.Type[] statesToAttemptToTransitionToEveryFrame { get; protected set; }

    protected static bool PlayerGrounded;


    public override void InitializeState(EntityStateMachine stateMachine, Transform owner)
    {
        base.InitializeState(stateMachine, owner);
        Player = owner.GetComponent<PlayerController>();
        groundMask = LayerMask.GetMask("Ground");
        swingMask = LayerMask.GetMask("Worm");
    }

    public bool IsGrounded()
    {

        var ray = new Ray(Player.Collider.bounds.center, Vector3.down);
        bool hit = Physics.SphereCast
            (
            ray, 
            Player.Collider.bounds.extents.y * SHAPECAST_RATIO,
            GROUND_CHECK_SAFE_MARGIN,
            groundMask
            );
        return hit;
    }

    public override void Process()
    {
        base.Process();
        CheckIfPerFrameStateTransitionRequired();
    }

    protected void CheckIfPerFrameStateTransitionRequired()
    {
        for (int i = 0; i < statesToAttemptToTransitionToEveryFrame.Length; i++)
        {
            var stateClass = statesToAttemptToTransitionToEveryFrame[i];
            if (StateMachine.IsStateAvailable(stateClass))
            {
                StateMachine.TransitionTo(stateClass, null);
                return;
            }
        }
    }

    
}
