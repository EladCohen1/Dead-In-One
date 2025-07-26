using UnityEngine;

public class PlayerTurnManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] PlayerController playerController;
    [SerializeField] MainBoardGrid mainBoardGrid;
    [SerializeField] TurnManager turnManager;

    void OnEnable()
    {
        playerController.EndPlayerTurnEvent += EndPlayerTurn;
        playerController.StartPlayerTurnEvent += StartPlayerTurn;
    }
    void OnDisable()
    {
        playerController.EndPlayerTurnEvent -= EndPlayerTurn;
        playerController.StartPlayerTurnEvent -= StartPlayerTurn;
    }

    void StartPlayerTurn()
    {
        mainBoardGrid.GenerateDistanceField(playerController.GetPlayerPos());
    }
    void EndPlayerTurn()
    {
        turnManager.EndPlayerTurn();
    }
}
