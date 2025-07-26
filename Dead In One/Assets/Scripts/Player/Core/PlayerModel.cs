using System;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] PlayerController playerController;

    [Header("Data")]
    [SerializeField] PlayerStatsSO _stats;

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
}
