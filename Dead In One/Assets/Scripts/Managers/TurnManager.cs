using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public TurnStateEnum turnSate { get; private set; } = TurnStateEnum.PlayerTurn;

    // Events
    public event Action PlayerTurnStart;
    public event Action EnemyTurnStart;

    public void EndEnemyTurn()
    {
        turnSate = TurnStateEnum.PlayerTurn;
        PlayerTurnStart?.Invoke();
    }

    public void EndPlayerTurn()
    {
        turnSate = TurnStateEnum.EnemyTurn;
        EnemyTurnStart?.Invoke();
    }
}
