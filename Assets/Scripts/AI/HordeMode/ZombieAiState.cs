using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZombieAiStateId
{
    ChasePlayer,
    Death,
    Idle,
    AttackPlayer
}

public interface ZombieAiState
{
    ZombieAiStateId GetId();
    void Enter(ZombieAiAgent agent);
    void Update(ZombieAiAgent agent);
    void Exit(ZombieAiAgent agent);
}
