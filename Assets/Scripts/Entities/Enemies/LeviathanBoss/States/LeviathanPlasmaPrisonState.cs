using System.Collections.Generic;
using UnityEngine;

public class LeviathanPlasmaPrisonState : LeviathanProjectileState
{
    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        Leviathan.RigidBody.linearVelocity = Vector3.zero;
    }
}
