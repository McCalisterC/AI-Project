using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyAiStateMachine
{
    public FriendlyAiState[] states;
    public FriendlyAiAgent agent;
    public FriendlyAiStateId currentState;

    public FriendlyAiStateMachine(FriendlyAiAgent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(FriendlyAiStateId)).Length;
        states = new FriendlyAiState[numStates];
    }

    public void RegisterState(FriendlyAiState state){
        int index = (int)state.GetId();
        states[index] = state;
    }

    public FriendlyAiState GetState(FriendlyAiStateId stateId){
        int index = (int)stateId;
        return states[index];
    }

    public void Update() {
        GetState(currentState)?.Update(agent);
    }

    public void ChangeStates(FriendlyAiStateId newState){
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }
}
