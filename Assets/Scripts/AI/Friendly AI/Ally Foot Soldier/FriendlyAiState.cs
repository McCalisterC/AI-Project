using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FriendlyAiStateId{
    ChasePlayer,
    Death,
    Idle,
    AttackPlayer,
    Pathfind,
    FollowPlayer
}

public interface FriendlyAiState
{
    FriendlyAiStateId GetId();
    void Enter(FriendlyAiAgent agent);
    void Update(FriendlyAiAgent agent);
    void Exit(FriendlyAiAgent agent);
}
