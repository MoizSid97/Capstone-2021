﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverterBT : NodeBT
{
    protected NodeBT node;

    public InverterBT(NodeBT nodes)
    {
        this.node = nodes;
    }

    public override NodeState Evaluate()
    {
        switch (node.Evaluate())
        {
            case NodeState.RUNNING:
                _nodeState = NodeState.RUNNING;
                break;
            case NodeState.SUCCESS:
                _nodeState = NodeState.FAILURE;
                break;
            case NodeState.FAILURE:
                _nodeState = NodeState.SUCCESS;
                break;
            default:
                break;
        }

        return _nodeState;
    }
}
