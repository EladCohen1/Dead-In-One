using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("Dependencies")]
    Material baseMaterial;

    void Awake()
    {
        baseMaterial = GetComponent<MeshRenderer>().material;
    }


    // Utils
    public void UpdateMaterial(Material newMat)
    {
        GetComponent<MeshRenderer>().material = newMat;
    }
    public void RevertMatToBase()
    {
        GetComponent<MeshRenderer>().material = baseMaterial;
    }
}
