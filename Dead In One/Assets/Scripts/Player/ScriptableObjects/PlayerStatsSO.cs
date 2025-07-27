using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "PlayerSO/Player Stats")]
public class PlayerStatsSO : ScriptableObject
{
    public string Name;
    public int Base_HP;
    public int Base_Move_Points_Per_Turn;
    public float Base_Armor;
    public float Base_Damage_Mod;
    public float Base_Crit_Chance_Add;
    public float Base_Crit_Damage_Add;
}
