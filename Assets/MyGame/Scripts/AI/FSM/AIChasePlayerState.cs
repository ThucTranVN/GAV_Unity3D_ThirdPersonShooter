using UnityEngine;
using UnityEngine.AI;

public class AIChasePlayerState : AIState
{
    private float timer = 0.0f;

    public void Enter(AIAgent agent)
    {

    }

    public void Exit(AIAgent agent)
    {
        
    }

    public AIStateID GetID()
    {
        return AIStateID.ChasePlayer;
    }

    public void Update(AIAgent agent)
    {
        if (!agent.enabled)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = agent.playerTransform.position;
        }

        if (timer < 0.0f)
        {
            Vector3 direction = agent.playerTransform.position - agent.navMeshAgent.destination;
            direction.y = 0;
            if (direction.sqrMagnitude > agent.config.minDistance * agent.config.minDistance)
            {
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }
            }
            timer = agent.config.maxTime;
        }
    }
}
