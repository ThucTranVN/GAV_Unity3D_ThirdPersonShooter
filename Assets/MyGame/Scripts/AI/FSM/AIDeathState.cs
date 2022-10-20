using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    public Vector3 direction;

    public void Enter(AIAgent agent)
    {
        agent.ragdoll.ActiveRagdoll();
        direction.y = 1;
        agent.ragdoll.ApplyForce(direction * agent.config.dieForce);
        agent.healthBar.Deactive();
    }

    public void Exit(AIAgent agent)
    {
       
    }

    public AIStateID GetID()
    {
        return AIStateID.Death;
    }

    public void Update(AIAgent agent)
    {
       
    }
}
