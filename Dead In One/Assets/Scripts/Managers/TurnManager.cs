using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public TurnStateEnum turnSate { get; private set; } = TurnStateEnum.PlayerTurn;

    // Events
    public event Action PlayerTurnStart;
    public event Action EnemyTurnStart;

    void Awake()
    {
        ServiceLocator.Register(this);
    }

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
