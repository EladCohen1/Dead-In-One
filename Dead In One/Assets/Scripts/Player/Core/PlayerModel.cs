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
        ServiceLocator.Register(this);
    }
    void OnDestroy()
    {
        ServiceLocator.UnRegister(this);
    }


    // Public Getters
    public string GetName()
    {
        return _stats.Name;
    }
    public int GetMaxHp()
    {
        // Add Max_Hp calculation using buffs and stat increases from proper managers here
        return (int)(_stats.Base_HP * playerController.playerLevelManager.levelStatModifier);
    }
    public int GetCurrentHP()
    {
        return HP;
    }
    public float GetDamageMod()
    {
        return _stats.Base_Damage_Mod * playerController.playerLevelManager.levelStatModifier;
    }
    public List<WeaponSO> GetStarterWeaponSOs()
    {
        return starterWeapons;
    }
    public float GetCritChanceAdd()
    {
        return _stats.Base_Crit_Chance_Add * playerController.playerLevelManager.levelStatModifier;
    }
    public float GetCritDamageAdd()
    {
        return _stats.Base_Crit_Damage_Add * playerController.playerLevelManager.levelStatModifier;
    }

    // Public Actions
    public int TakeDamage(int damage)
    {
        int damageTaken = CalcDamageTaken(damage);
        HP -= damageTaken;
        return damageTaken;
    }
    public int Heal(int amount)
    {
        HP = Mathf.Clamp(HP + amount, 0, GetMaxHp());
        return HP;
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

    // Utils
    int CalcDamageTaken(int damage)
    {
        float reductionPercent = _stats.Base_Armor * playerController.playerLevelManager.levelStatModifier * 0.01f;
        reductionPercent = Mathf.Clamp01(reductionPercent);
        return (int)(damage * (1f - reductionPercent));
    }
}
