using DG.Tweening;
using UnityEngine;

public class PlayerView : EntityView
{
    [Header("Components")]
    [SerializeField] PlayerController playerController;

    [Header("Data")]
    [SerializeField] Vector2Int startingPos;

    void OnEnable()
    {
        mainBoardGrid.OnGridGeneratedEvent += InitPosition;
    }

    void OnDisable()
    {
        mainBoardGrid.OnGridGeneratedEvent -= InitPosition;
    }

    // Utils
    void InitPosition()
    {
        UpdatePos(startingPos);
        playerController.EndPlayersTurn();
    }
    public void MovePlayer(Vector2Int moveAmount)
    {
        if (!mainBoardGrid.IsInRange(currentPos + moveAmount))
            return;

        UpdatePos(currentPos + moveAmount);
    }
}
