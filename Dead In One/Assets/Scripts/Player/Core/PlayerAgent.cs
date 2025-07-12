using DG.Tweening;
using UnityEngine;

public class PlayerAgent : MonoBehaviour
{
    [Header("Dependencies")]
    MainBoardGrid mainBoardGrid;
    TurnManager turnManager;



    // Runtime Data
    Vector2Int currentPos = new Vector2Int(-1, -1);

    void Awake()
    {
        mainBoardGrid = FindAnyObjectByType<MainBoardGrid>();
        turnManager = FindAnyObjectByType<TurnManager>();
    }

    void OnEnable()
    {
        mainBoardGrid.OnGridGeneratedEvent += InitPosition;
        turnManager.PlayerTurnStart += StartPlayerTurn;
    }

    void OnDisable()
    {
        mainBoardGrid.OnGridGeneratedEvent -= InitPosition;
        turnManager.PlayerTurnStart -= StartPlayerTurn;
    }

    // Utils
    void InitPosition()
    {
        UpdatePlayerPos(new Vector2Int(50, 50));
        EndPlayersTurn();
    }
    void UpdatePlayerPos(Vector2Int destination)
    {
        // Grid Logic
        if (!mainBoardGrid.MoveToTile(currentPos, destination, gameObject))
            return;

        // Visual Move
        Vector3 targetPos = mainBoardGrid.GetWorldPosByGridPos(destination);
        transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutQuad);
        currentPos = destination;
    }
    public void MovePlayer(Vector2Int moveAmount)
    {
        if (!mainBoardGrid.IsInRange(currentPos + moveAmount))
            return;

        UpdatePlayerPos(currentPos + moveAmount);
        EndPlayersTurn();
    }

    void StartPlayerTurn()
    {
        mainBoardGrid.GenerateDistanceField();
    }
    void EndPlayersTurn()
    {
        turnManager.EndPlayerTurn();
    }
}
