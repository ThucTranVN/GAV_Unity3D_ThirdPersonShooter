using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackPlayerState : AIState
{
    public void Enter(AIAgent agent)
    {
        agent.aiWeapon.ActivateWeapon();
        agent.aiWeapon.SetTarget(agent.playerTransform);
        agent.navMeshAgent.stoppingDistance = 5f;
        agent.aiWeapon.SetFiring(true);
    }

    public void Exit(AIAgent agent)
    {
        
    }

    public AIStateID GetID()
    {
        return AIStateID.AttackPlayer;
    }

    public void Update(AIAgent agent)
    {
        agent.navMeshAgent.destination = agent.playerTransform.position;
    }
}
