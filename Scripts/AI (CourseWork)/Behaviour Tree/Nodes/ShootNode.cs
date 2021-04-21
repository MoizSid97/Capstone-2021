using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootNode : NodeBT
{
    private NavMeshAgent agent;
    private EnemyAIVersion3 ai;

    public ShootNode(NavMeshAgent agent, EnemyAIVersion3 ai)
    {
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        agent.isStopped = true;
        ai.SetColor(Color.green);
        return NodeState.RUNNING;
    }
}
