using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] PlayerInputChannel inputChannel;
    [SerializeField] TurnManager turnManager;

    [Header("Components")]
    [SerializeField] PlayerView playerView;
    [SerializeField] PlayerModel playerModel;

    // Events
    public event Action EndPlayerTurnEvent;
    public event Action StartPlayerTurnEvent;

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

        MovePlayer(dir);
        EndPlayersTurn();
    }

    // Logic
    void MovePlayer(Vector2Int dir)
    {
        playerView.MovePlayer(dir);
    }

    // Public Action
    public void EndPlayersTurn()
    {
        EndPlayerTurnEvent?.Invoke();
    }
    public void StartPlayerTurn()
    {
        StartPlayerTurnEvent?.Invoke();
    }
    public Vector2Int GetPlayerPos()
    {
        return playerView.currentPos;
    }
}
