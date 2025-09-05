using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("Data")]
    Material baseMat;
    Material attackedMat;
    MeshRenderer meshRenderer;


    // Coroutines
    bool isInCoroutine = false;
    Coroutine flashCoroutine;

    // Runtime Vars
    Material currentMaterial;
    HashSet<EnemyController> attackers = new();

    public void Init(MeshRenderer meshRenderer, Material baseMat, Material attackedMat)
    {
        (this.meshRenderer, this.baseMat, this.attackedMat) = (meshRenderer, baseMat, attackedMat);
        currentMaterial = baseMat;
    }

    // Public Methods
    public void AttackTile(EnemyController enemyController)
    {
        attackers.Add(enemyController);
        UpdateMatState();
    }
    public void RemoveAttacker(EnemyController enemyController)
    {
        attackers.Remove(enemyController);
        UpdateMatState();
    }

    // Utils
    void SetAttackedTile()
    {
        currentMaterial = attackedMat;
        ApplyMat();
    }
    public void UpdateMaterial(Material newMat)
    {
        currentMaterial = newMat;
        ApplyMat();
    }
    void RevertMatToBase()
    {
        currentMaterial = baseMat;
        ApplyMat();
    }
    private void UpdateMatState()
    {
        if (attackers.Count > 0)
            SetAttackedTile();
        else
            RevertMatToBase();
    }

    // Animations
    public void FlashAttackedMaterial(Material flashMat, float flashTime = 0.5f)
    {
        flashCoroutine = StartCoroutine(DoFlashAttackedMaterial(flashMat, flashTime));
    }
    public void KillAllAnimations()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
        ApplyMat();
    }

    void ApplyMat()
    {
        if (!isInCoroutine && meshRenderer)
            meshRenderer.material = currentMaterial;
    }
    IEnumerator DoFlashAttackedMaterial(Material flashMat, float flashTime)
    {
        float timer = flashTime;
        bool isBaseMat = true;
        isInCoroutine = true;

        while (timer > 0)
        {
            if (isBaseMat)
            {
                meshRenderer.material = flashMat;
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

        meshRenderer.material = currentMaterial;
        isInCoroutine = false;
    }
}
