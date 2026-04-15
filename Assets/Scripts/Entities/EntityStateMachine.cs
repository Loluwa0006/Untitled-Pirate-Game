using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityStateMachine : MonoBehaviour
{
    [SerializeField] Transform owner;
    [SerializeField] BaseState currentState;

    Dictionary<System.Type, BaseState> stateLookup = new();
    List<BaseState> statesWithInactiveProcess = new();
    List<BaseState> statesWithInactivePhysicsProcess = new();


    private void Start()
    {
        foreach (var state in transform.GetComponentsInChildren<BaseState>())
        {
            stateLookup[state.GetType()] = state;
            if (state.hasInactivePhysicsProcess) statesWithInactivePhysicsProcess.Add(state);
            if (state.hasInactiveProcess) statesWithInactiveProcess.Add(state);
            state.InitializeState(this, owner);
        }

        if (currentState == null) currentState = stateLookup.ElementAt(0).Value;
    }

    public void Process()
    {
        if (currentState != null) currentState.Process();
        foreach (var state in statesWithInactiveProcess) state.InactiveProcess();
    }

    public void PhysicsProcess()
    {
        if (currentState != null) currentState.PhysicsProcess();
        foreach (var state in statesWithInactivePhysicsProcess) state.InactivePhysicsProcess();
    }

    public void TransitionTo<T>(Dictionary<string, object> message = null) where T : BaseState
    {
        if (!stateLookup.ContainsKey(typeof(T)))
        {
            Debug.LogWarning("Could not find object of type " + typeof(T));
        }
        
        currentState.Exit();
        currentState = stateLookup[typeof(T)];
        currentState.Enter(message);
    }
}
