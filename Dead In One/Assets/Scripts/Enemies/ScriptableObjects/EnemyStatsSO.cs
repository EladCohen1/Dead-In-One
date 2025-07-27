using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSO", menuName = "EnemySO/Enemy Stats")]
public class EnemyStatsSO : ScriptableObject
{
    public string Name;
    public int Base_HP;
    public int Moves_Per_Turn;
    public float Base_Armor;
    public int Max_Movement_Squares;
    public int Damage;
    public int Number_Of_Attacks;
    public float Exp_Dropped;
}
