using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] PlayerController playerController;

    [Header("Data")]
    [SerializeField] PlayerStatsSO _stats;

    [Header("Weapons")]
    [SerializeField] private List<WeaponSO> starterWeapons = new();

    [Header("Runtime Data")]
    public int HP;
    public int CurrentActionPoints;

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
    public float GetDamageMod()
    {
        return _stats.Base_Damage_Mod;
    }
    public List<WeaponSO> GetStarterWeaponSOs()
    {
        return starterWeapons;
    }

    // Public Actions
    public void TakeDamage(int damage)
    {
        HP -= damage;
    }

    // Action Points
    public void ResetActionPoints()
    {
        CurrentActionPoints = _stats.Base_Action_Points_Per_Turn;
    }
    public bool UseActionPoints(int pointsToUse)
    {
        CurrentActionPoints -= pointsToUse;
        return CurrentActionPoints <= 0;
    }
}
