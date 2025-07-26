using DG.Tweening;
using UnityEngine;

public abstract class EntityView : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] protected MainBoardGrid mainBoardGrid;

    // Runtime Data
    public Vector2Int currentPos { get; private set; } = new Vector2Int(-1, -1);

    void Start()
    {
        mainBoardGrid = ServiceLocator.Get<MainBoardGrid>();
    }

    public void UpdatePos(Vector2Int destination)
    {

        if (mainBoardGrid == null)
            Start();

        // Grid Logic
        if (!mainBoardGrid.MoveToTile(this, destination))
            return;

        // Visual Move
        Vector3 targetPos = mainBoardGrid.GetWorldPosByGridPos(destination);
        transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutQuad);
        currentPos = destination;
    }
}
