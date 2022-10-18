using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{
    public Transform playerTransform;
    public float maxTime = 1.0f;
    public float minDistance = 1.0f;

    private NavMeshAgent agent;
    private Animator animator;
    private float timer = 0.0f;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0.0f)
        {
            float distance = (playerTransform.position - agent.destination).sqrMagnitude;
            if(distance > minDistance * minDistance)
            {
                agent.destination = playerTransform.position;
            }
            timer = maxTime;
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
