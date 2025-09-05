using UnityEngine;

public class WeaponController
{
    // Data
    WeaponSO _weaponSO;

    // Runtime Data
    int currentCharge;

    public WeaponController(WeaponSO weaponSO)
    {
        _weaponSO = weaponSO;
        currentCharge = weaponSO.Attack_Charge_Cost;
    }


    // Public Getters
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

    // Public Actions
    public int RollDamage(out bool didCrit)
    {
        didCrit = Random.value < Mathf.Clamp01(_weaponSO.Crit_Chance * 0.01f);
        return didCrit ? (int)(_weaponSO.Damage * _weaponSO.Crit_Damage * 0.01f) : _weaponSO.Damage;
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
}
