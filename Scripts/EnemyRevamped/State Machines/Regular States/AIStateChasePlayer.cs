using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStateChasePlayer : AIState
{
    float timer = 0.0f;

    public AIStateID GetID()
    {
        return AIStateID.ChasePlayer;
    }

    public void Enter(AIAgent agent)
    {

    }

    public void Update(AIAgent agent)
    {
        if (!agent.enabled)
        {
            return;
        }

        timer -= Time.deltaTime;

        //Follow player
        if (!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = agent.playerTransform.position;
        }

        //Keep looking for player's position | Updating target/player position to follow
        if (timer < 0.0f)
        {
            Vector3 dir = (agent.playerTransform.position - agent.navMeshAgent.destination);
            dir.y = 0;
            if (dir.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }
            }

            timer = agent.config.maxTime;
        }
    }

    public void Exit(AIAgent agent)
    {

    }
}
