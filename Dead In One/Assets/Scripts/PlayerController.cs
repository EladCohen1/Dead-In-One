using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] MainBoardGrid mainBoardGrid;
    [SerializeField] PlayerInputChannel inputChannel;

    // Data
    Vector2Int currentPos;

    void OnEnable()
    {
        mainBoardGrid.OnGridGeneratedEvent += InitPosition;
        inputChannel.OnMoveEvent += HandleMoveEvent;
    }

    void OnDisable()
    {
        mainBoardGrid.OnGridGeneratedEvent -= InitPosition;
        inputChannel.OnMoveEvent -= HandleMoveEvent;
    }

    // Utils
    void InitPosition()
    {
        UpdatePlayerPos(50, 50);
    }
    void UpdatePlayerPos(int xPos, int yPos)
    {
        Vector3 targetPos = mainBoardGrid.GetWorldPosByGridPos(new Vector2Int(xPos, yPos));
        transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutQuad);
        currentPos = new Vector2Int(xPos, yPos);
    }

    // Event Handlers
    void HandleMoveEvent(Vector2Int dir)
    {
        Vector2Int newPlayerPos = currentPos + dir;
        UpdatePlayerPos(newPlayerPos.x, newPlayerPos.y);
    }
}
