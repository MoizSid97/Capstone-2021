using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    //Array of states from AIState script
    public AIState[] states;
    //Using AIAgent script
    public AIAgent accessAIAgent;

    public AIStateID currentState;

    public AIStateMachine(AIAgent agent)
    {
        this.accessAIAgent = agent;
        int numStates = System.Enum.GetNames(typeof(AIStateID)).Length;
        states = new AIState[numStates];
    }

    public void RegisterState(AIState state)
    {
        int index = (int)state.GetID();
        states[index] = state;
    }

    public AIState GetState(AIStateID stateID)
    {
        int index = (int)stateID;
        return states[index];
    }

    public void Update()
    {
        GetState(currentState)?.Update(accessAIAgent);
    }

    public void ChangeState(AIStateID newState)
    {
        GetState(currentState)?.Exit(accessAIAgent);
        currentState = newState;
        GetState(currentState)?.Enter(accessAIAgent);
    }
}
