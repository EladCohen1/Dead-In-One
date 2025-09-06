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

        if (!mainBoardGrid)
            Start();

        // Grid Logic
        if (!mainBoardGrid.MoveToTile(this, destination))
            return;

        // Visual Move
        Vector3 targetPos = mainBoardGrid.GetWorldPosByGridPos(destination);
        Tween tween = null;
        var cachedTransform = transform;

        tween = cachedTransform.DOMove(targetPos, 0.3f)
            .SetEase(Ease.OutQuad)
            .SetTarget(gameObject);
        currentPos = destination;
    }

    void OnDestroy()
    {
        DOTween.Kill(gameObject);
    }
}
