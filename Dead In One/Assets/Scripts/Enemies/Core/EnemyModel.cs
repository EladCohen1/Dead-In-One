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
    public int GetChanceToDropHp()
    {
        return _stats.Hp_Drop_Chance;
    }
    public int GetChanceToDropExp()
    {
        return _stats.Exp_Drop_Chance;
    }
    public int GetHpDropped()
    {
        return _stats.Hp_Dropped;
    }
    public int GetExpDropped()
    {
        return _stats.Exp_Dropped;
    }

    // Public Actions
    public void TakeDamage(int damage)
    {
        float reductionPercent = Mathf.Clamp01(_stats.Base_Armor * 0.01f);
        HP -= (int)(damage * (1f - reductionPercent));
    }
}
