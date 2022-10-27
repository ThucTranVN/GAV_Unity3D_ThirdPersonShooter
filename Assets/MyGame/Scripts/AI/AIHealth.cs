using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : Health
{
    private AIAgent agent;

    protected override void OnStart()
    {
        agent = GetComponent<AIAgent>();
    }

    protected override void OnDeath(Vector3 direction)
    {
        AIDeathState deathState = agent.stateMachine.GetState(AIStateID.Death) as AIDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChangeState(AIStateID.Death);
    }

    protected override void OnDamage(Vector3 direction)
    {

    }
}
