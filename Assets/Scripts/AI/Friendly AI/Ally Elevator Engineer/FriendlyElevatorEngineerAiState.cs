using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FriendlyElevatorEngineerAiStateId{
    Idle,
    StartRepair,
    EndRepair
}

public interface FriendlyElevatorEngineerAiState
{
    FriendlyElevatorEngineerAiStateId GetId();
    void Enter(FriendlyElevatorEngineerAiAgent agent);
    void Update(FriendlyElevatorEngineerAiAgent agent);
    void Exit(FriendlyElevatorEngineerAiAgent agent);
}
