using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public TurnStateEnum turnSate { get; private set; }

    public void SetPlayerTurn()
    {
        turnSate = TurnStateEnum.PlayerTurn;
    }

    public void SetEnemyTurn()
    {
        turnSate = TurnStateEnum.EnemyTurn;
    }
}
