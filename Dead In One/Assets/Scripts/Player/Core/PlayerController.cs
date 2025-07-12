using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] PlayerInputChannel inputChannel;
    TurnManager turnManager;

    PlayerAgent playerAgent;
    PlayerContext playerContext;

    void Awake()
    {
        playerContext = GetComponent<PlayerContext>();
        playerAgent = GetComponent<PlayerAgent>();

        turnManager = FindFirstObjectByType<TurnManager>();
    }

    void OnEnable()
    {
        inputChannel.OnMoveEvent += HandleMoveEvent;
    }

    void OnDisable()
    {
        inputChannel.OnMoveEvent -= HandleMoveEvent;
    }


    // Event Handlers
    void HandleMoveEvent(Vector2Int dir)
    {
        MovePlayer(dir);
    }

    // Logic
    void MovePlayer(Vector2Int dir)
    {
        if (turnManager.turnSate != TurnStateEnum.PlayerTurn)
            return;

        playerAgent.MovePlayer(dir);
    }
}
