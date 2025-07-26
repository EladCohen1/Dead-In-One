using DG.Tweening;
using UnityEngine;

public class PlayerView : EntityView
{
    [Header("Components")]
    [SerializeField] PlayerController playerController;

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
        UpdatePos(new Vector2Int(50, 50));
        playerController.EndPlayersTurn();
    }
    public void MovePlayer(Vector2Int moveAmount)
    {
        if (!mainBoardGrid.IsInRange(currentPos + moveAmount))
            return;

        UpdatePos(currentPos + moveAmount);
    }
}
