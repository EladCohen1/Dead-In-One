using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    [Header("Data")]
    public int baseExpReqLevelUp;
    public float expReqIncreasePerLevel;

    // Runtime Data
    public int currentLevel { get; private set; }
    public int currentExp { get; private set; }
    public float levelStatModifier { get; private set; }


    void Awake()
    {
        DataInit();
        ServiceLocator.Register(this);
    }
    void OnDestroy()
    {
        ServiceLocator.UnRegister(this);
    }

    public bool GainExp(int amount)
    {
        currentExp += amount;
        if (currentExp >= GetExpReqToLevelUp())
        {
            LevelUp();
            return true;
        }
        return false;
    }

    void DataInit()
    {
        currentExp = 0;
        currentLevel = 0;
        LevelUp();
    }

    public int GetExpReqToLevelUp()
    {
        return (int)(baseExpReqLevelUp * ((expReqIncreasePerLevel * (currentLevel - 1)) + 1));
    }

    // Utils
    void LevelUp()
    {
        currentLevel++;
        levelStatModifier = GetLevelStatsModifier();
    }
    float GetLevelStatsModifier()
    {
        return currentLevel * 0.1f + 0.9f;
    }
}
