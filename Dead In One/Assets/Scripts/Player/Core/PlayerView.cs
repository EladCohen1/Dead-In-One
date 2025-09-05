using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerView : EntityView
{
    [Header("Components")]
    [SerializeField] PlayerController playerController;

    [Header("Data")]
    [SerializeField] Vector2Int startingPos;

    [Header("Materials")]
    public Material baseMat;
    public Material attackedMat;
    [SerializeField] MeshRenderer meshRenderer;

    [Header("Coroutines")]
    Coroutine flashCoroutine;

    void OnEnable()
    {
        mainBoardGrid.OnGridGeneratedEvent += InitPosition;
    }

    void OnDisable()
    {
        mainBoardGrid.OnGridGeneratedEvent -= InitPosition;
    }

    void OnDestroy()
    {
        KillAllAnimations();
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

    public void KillAllAnimations()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
    }

    public void FlashAttacked()
    {
        flashCoroutine = StartCoroutine(DoFlashAttacked());
    }

    IEnumerator DoFlashAttacked()
    {
        float timer = 1f;
        bool isBaseMat = true;

        while (timer > 0)
        {
            if (isBaseMat)
            {
                meshRenderer.material = attackedMat;
                isBaseMat = false;
            }
            else
            {
                meshRenderer.material = baseMat;
                isBaseMat = true;
            }

            yield return new WaitForSeconds(0.1f);
            timer -= 0.1f;
        }

        meshRenderer.material = baseMat;
    }
}
