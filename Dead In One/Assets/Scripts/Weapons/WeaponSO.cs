using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Weapons/Weapon")]
public class WeaponSO : ScriptableObject
{
    [Header("Data")]
    public string UID;
    public string Name;
    public string CardDescription;
    public int Charge_Per_Move;
    public int Attack_Charge_Cost;
    public int Damage;
    public float Crit_Chance;
    public float Crit_Damage;

    [Header("Attacks")]
    public Vector2Int[] attacksDirs;

    [Header("Materials")]
    public Material attackingMat;
    public Material attackedMat;

    public Material attackingCritMat;
    public Material attackedCritMat;

    [Header("Card Visuals")]
    public Sprite portrait;
}
