using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAiStateMachine
{
    public SniperAiState[] states;
    public SniperAiAgent agent;
    public SniperAiStateId currentState;

    public SniperAiStateMachine(SniperAiAgent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(AiStateId)).Length;
        states = new SniperAiState[numStates];
    }

    public void RegisterState(SniperAiState state){
        int index = (int)state.GetId();
        states[index] = state;
    }

    public SniperAiState GetState(SniperAiStateId stateId){
        int index = (int)stateId;
        return states[index];
    }

    public void Update() {
        GetState(currentState)?.Update(agent);
    }

    public void ChangeStates(SniperAiStateId newState){
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }
}
