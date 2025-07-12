using System;
using UnityEngine;

public class PlayerContext : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO _stats;
    public PlayerStatsSO stats
    {
        get => _stats;
        set
        {
            if (value == _stats)
                return;
            _stats = value;
            PlayerStatsChangedEvent?.Invoke(value);
        }
    }

    // Events
    public event Action<PlayerStatsSO> PlayerStatsChangedEvent;

    // Public Getters
    public string GetName()
    {
        return stats.Name;
    }
    public int GetMaxHp()
    {
        // Add Max_Hp calculation using buffs and stat increases from proper managers here
        return stats.Base_HP;
    }
}
