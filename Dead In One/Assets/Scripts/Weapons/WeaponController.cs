using UnityEngine;

public class WeaponController
{
    // Data
    WeaponSO _weaponSO;

    // Runtime Data
    int currentCharge;
    int currentLevel;
    float levelStatModifier;

    public WeaponController(WeaponSO weaponSO)
    {
        _weaponSO = weaponSO;
        currentCharge = weaponSO.Attack_Charge_Cost;
        currentLevel = 0;
        LevelUp();
    }


    // Public Getters
    public string GetUID()
    {
        return _weaponSO.UID;
    }
    public Vector2Int[] GetAttackDirs()
    {
        return _weaponSO.attacksDirs;
    }
    public Material GetAttackingMat()
    {
        return _weaponSO.attackingMat;
    }
    public Material GetAttackedMat()
    {
        return _weaponSO.attackedMat;
    }
    public Material GetAttackingCritMat()
    {
        return _weaponSO.attackingCritMat;
    }
    public Material GetAttackedCritMat()
    {
        return _weaponSO.attackedCritMat;
    }
    public int GetLevel()
    {
        return currentLevel;
    }

    // Public Actions
    public int RollDamage(float critRateAdd, float critDamageAdd, out bool didCrit)
    {
        didCrit = Random.value < Mathf.Clamp01((_weaponSO.Crit_Chance + critRateAdd) * 0.01f);
        return didCrit ? (int)(_weaponSO.Damage * levelStatModifier * (_weaponSO.Crit_Damage + critDamageAdd) * 0.01f) : _weaponSO.Damage;
    }
    public bool Attack()
    {
        if (currentCharge < _weaponSO.Attack_Charge_Cost)
            return false;

        currentCharge -= _weaponSO.Attack_Charge_Cost;
        return true;
    }
    public void RechargeOnMove()
    {
        currentCharge += _weaponSO.Charge_Per_Move;
    }
    public void LevelUp()
    {
        levelStatModifier = GetLevelStatsModifier();
        currentLevel++;
    }
    float GetLevelStatsModifier()
    {
        return currentLevel * 0.2f + 1; ;
    }
}
