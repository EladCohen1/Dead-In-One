using UnityEngine;

public class TileManager : MonoBehaviour
{
    Material baseMat;
    Material attackedMat;
    MeshRenderer meshRenderer;

    public void Init(MeshRenderer meshRenderer, Material baseMat, Material attackedMat)
    {
        (this.meshRenderer, this.baseMat, this.attackedMat) = (meshRenderer, baseMat, attackedMat);
    }

    // Utils
    public void SetAttackedTile()
    {
        meshRenderer.material = attackedMat;
    }
    public void UpdateMaterial(Material newMat)
    {
        meshRenderer.material = newMat;
    }
    public void RevertMatToBase()
    {
        meshRenderer.material = baseMat;
    }
}
