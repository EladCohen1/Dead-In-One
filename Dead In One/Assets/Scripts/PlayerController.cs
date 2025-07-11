using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] MainBoardGrid mainBoardGrid;

    // Data
    Vector2Int currentPos;

    void OnEnable()
    {
        mainBoardGrid.OnGridGeneratedEvent += InitPosition;
    }

    void OnDisable()
    {
        mainBoardGrid.OnGridGeneratedEvent -= InitPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveTileUp();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveTileLeft();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveTileDown();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveTileRight();
        }
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

    void MoveTileUp()
    {
        UpdatePlayerPos(currentPos.x, currentPos.y + 1);
    }
    void MoveTileLeft()
    {
        UpdatePlayerPos(currentPos.x - 1, currentPos.y);
    }
    void MoveTileDown()
    {
        UpdatePlayerPos(currentPos.x, currentPos.y - 1);
    }
    void MoveTileRight()
    {
        UpdatePlayerPos(currentPos.x + 1, currentPos.y);
    }
}
