using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAiStateMachine
{
    public ZombieAiState[] states;
    public ZombieAiAgent agent;
    public ZombieAiStateId currentState;

    public ZombieAiStateMachine(ZombieAiAgent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(ZombieAiStateId)).Length;
        states = new ZombieAiState[numStates];
    }

    public void RegisterState(ZombieAiState state){
        int index = (int)state.GetId();
        states[index] = state;
    }

    public ZombieAiState GetState(ZombieAiStateId stateId){
        int index = (int)stateId;
        return states[index];
    }

    public void Update() {
        GetState(currentState)?.Update(agent);
    }

    public void ChangeStates(ZombieAiStateId newState){
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }
}
