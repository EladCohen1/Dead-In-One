using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] PlayerInputChannel inputChannel;
    [SerializeField] TurnManager turnManager;
    [SerializeField] MainBoardGrid mainBoardGrid;

    [Header("Components")]
    [SerializeField] PlayerView playerView;
    [SerializeField] PlayerModel playerModel;

    // Events
    public event Action EndPlayerTurnEvent;
    public event Action StartPlayerTurnEvent;

    Vector2Int[] allAdjacentDirs = new Vector2Int[]
    {
        new Vector2Int(-1, -1),
        new Vector2Int( 0, -1),
        new Vector2Int( 1, -1),
        new Vector2Int(-1,  0),
        new Vector2Int( 1,  0),
        new Vector2Int(-1,  1),
        new Vector2Int( 0,  1),
        new Vector2Int( 1,  1),
    };

    void Awake()
    {
        ServiceLocator.Register(this);
    }

    void OnEnable()
    {
        inputChannel.OnMoveEvent += HandleMoveEvent;
        turnManager.PlayerTurnStart += StartPlayerTurn;
    }

    void OnDisable()
    {
        inputChannel.OnMoveEvent -= HandleMoveEvent;
        turnManager.PlayerTurnStart -= StartPlayerTurn;
    }

    // Event Handlers
    void HandleMoveEvent(Vector2Int dir)
    {
        if (turnManager.turnSate != TurnStateEnum.PlayerTurn)
            return;
        bool turnEnded = false;
        try
        {
            MovePlayer(dir);
            if (playerModel.UseActionPoints(1))
            {
                turnEnded = true;
                return;
            }
        }
        finally
        {
            Attack();
            if (turnEnded)
                EndPlayersTurn();
        }
    }

    // Logic
    void MovePlayer(Vector2Int dir)
    {
        playerView.MovePlayer(dir);
    }
    void Attack()
    {
        foreach (Vector2Int attackDir in allAdjacentDirs)
        {
            Vector2Int target = attackDir + playerView.currentPos;
            EntityView attackedEntity = mainBoardGrid.GetEntity(target);
            if (attackedEntity == null)
                continue;
            if (attackedEntity is not EnemyView enemyView)
                continue;

            enemyView.enemyController.TakeDamage(100);
        }
    }

    // Public Action
    public void EndPlayersTurn()
    {
        EndPlayerTurnEvent?.Invoke();
    }
    public void StartPlayerTurn()
    {
        StartPlayerTurnEvent?.Invoke();
        playerModel.ResetActionPoints();
    }
    public Vector2Int GetPlayerPos()
    {
        return playerView.currentPos;
    }

    public void TakeDamage(int damage)
    {
        playerModel.TakeDamage(damage);
    }

    // Public Data
    public Vector2Int GetCurrentPosition()
    {
        return playerView.currentPos;
    }
}
