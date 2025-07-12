using DG.Tweening;
using UnityEngine;

public class PlayerAgent : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] MainBoardGrid mainBoardGrid;


    // Runtime Data
    Vector2Int currentPos = new Vector2Int(-1, -1);


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
        UpdatePlayerPos(new Vector2Int(50, 50));
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
    }
}
