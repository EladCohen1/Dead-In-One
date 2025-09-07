using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSO", menuName = "EnemySO/Enemy Stats")]
public class EnemyStatsSO : ScriptableObject
{
    public string Name;
    public int Base_HP;
    public float Base_Armor;
    public int Damage;
    public int Number_Of_Attacks;
    public int Hp_Drop_Chance;
    public int Hp_Dropped;
    public int Exp_Drop_Chance;
    public int Exp_Dropped;
    public int Attack_Range_Grace;
}
