using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebugNavmash : MonoBehaviour
{
    public bool showVelocity;
    public bool showDesiredVelocity;
    public bool showPath;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnDrawGizmos()
    {
        if (showVelocity)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
        }

        if (showDesiredVelocity)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + agent.desiredVelocity);
        }

        if (showPath)
        {
            Gizmos.color = Color.white;
            var agentPath = agent.path;
            Vector3 prevCornor = transform.position;
            foreach (var cornor in agentPath.corners)
            {
                Gizmos.DrawLine(prevCornor, cornor);
                Gizmos.DrawSphere(cornor, 0.1f);
                prevCornor = cornor;
            }
        }
    }
}
