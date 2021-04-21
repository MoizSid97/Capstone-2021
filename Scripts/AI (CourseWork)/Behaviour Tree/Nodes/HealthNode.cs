using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNode : NodeBT
{
    private EnemyAIVersion3 ai;
    private float threshold;

    public HealthNode(EnemyAIVersion3 ai, float threshold)
    {
        this.ai = ai;
        this.threshold = threshold;
    }

    public override NodeState Evaluate()
    {
        return ai.currentHealth <= threshold ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
