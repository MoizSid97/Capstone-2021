using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateIdle : AIState
{
    public AIStateID GetID()
    {
        return AIStateID.Idle;
    }

    public void Enter(AIAgent agent)
    {
  
    }

    public void Exit(AIAgent agent)
    {
        
    }

    public void Update(AIAgent agent)
    {
        Vector3 playerDir = agent.playerTransform.position - agent.transform.position;

        if(playerDir.magnitude > agent.config.maxSightDisance)
        {
            return;
        }

        //Check if agent is facing the player
        Vector3 agentDir = agent.transform.forward;
        playerDir.Normalize();
        float dotProduct = Vector3.Dot(playerDir, agentDir);

        if(dotProduct > 0.0f)
        {
            agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
        }
    }
}
