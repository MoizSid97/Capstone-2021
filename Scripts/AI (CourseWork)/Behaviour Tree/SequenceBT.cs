using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceBT : NodeBT
{
    protected List<NodeBT> nodes = new List<NodeBT>();

    public SequenceBT(List<NodeBT> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        bool isAnyNodeRunning = false;

        foreach(var node in nodes)
        {
            switch(node.Evaluate())
            {
                case NodeState.RUNNING:
                    isAnyNodeRunning = true;
                    break;
                case NodeState.SUCCESS:
                    break;
                case NodeState.FAILURE:
                    _nodeState = NodeState.FAILURE;
                    return _nodeState;
                default:
                    break;
            }
        }

        _nodeState = isAnyNodeRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        return _nodeState;
    }
}
