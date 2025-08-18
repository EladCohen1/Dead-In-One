using System;
using UnityEngine;

public class EnemyModel : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] EnemyController enemyController;

    [Header("Data")]
    [SerializeField] EnemyStatsSO _stats;

    [Header("Runtime Data")]
    public int HP;

    void Awake()
    {
        HP = _stats.Base_HP;
    }

    // Public Getters
    public string GetName()
    {
        return _stats.Name;
    }
    public int GetMaxHp()
    {
        // Add Max_Hp calculation using buffs and stat increases from proper managers here
        return _stats.Base_HP;
    }
    public int GetDamage()
    {
        return _stats.Damage;
    }
    public int GetNumberOfAttacks()
    {
        return _stats.Number_Of_Attacks;
    }
    public int GetCurrentHP()
    {
        return HP;
    }
    public int GetAttackRangeGrace()
    {
        return _stats.Attack_Range_Grace;
    }

    // Public Actions
    public void TakeDamage(int damage)
    {
        HP -= damage;
    }
}
