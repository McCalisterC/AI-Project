using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyElevatorEngineerAiStateMachine
{
    public FriendlyElevatorEngineerAiState[] states;
    public FriendlyElevatorEngineerAiAgent agent;
    public FriendlyElevatorEngineerAiStateId currentState;

    public FriendlyElevatorEngineerAiStateMachine(FriendlyElevatorEngineerAiAgent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(FriendlyElevatorEngineerAiStateId)).Length;
        states = new FriendlyElevatorEngineerAiState[numStates];
    }

    public void RegisterState(FriendlyElevatorEngineerAiState state){
        int index = (int)state.GetId();
        states[index] = state;
    }

    public FriendlyElevatorEngineerAiState GetState(FriendlyElevatorEngineerAiStateId stateId){
        int index = (int)stateId;
        return states[index];
    }

    public void Update() {
        GetState(currentState)?.Update(agent);
    }

    public void ChangeStates(FriendlyElevatorEngineerAiStateId newState){
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }
}
