using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SniperAiStateId{
    Death,
    Idle,
    AttackPlayer,
    Pathfind
}

public interface SniperAiState
{
    SniperAiStateId GetId();
    void Enter(SniperAiAgent agent);
    void Update(SniperAiAgent agent);
    void Exit(SniperAiAgent agent);
}
