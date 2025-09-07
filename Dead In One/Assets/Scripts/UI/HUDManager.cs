using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI HPTextElement;
    [SerializeField] TextMeshProUGUI MaxHPTextElement;
    [SerializeField] TextMeshProUGUI CurrentLevelTextElement;


    [SerializeField] TextMeshProUGUI CurrentExpTextElement;
    [SerializeField] TextMeshProUGUI ExpReqLevelUpTextElement;


    void Awake()
    {
        ServiceLocator.Register(this);
    }

    void OnDestroy()
    {
        ServiceLocator.UnRegister(this);
    }

    void OnEnable()
    {
        PlayerController playerController = ServiceLocator.Get<PlayerController>();
        if (!playerController)
            return;
        playerController.HPChanged += UpdatePlayerHpUI;
        playerController.MaxHPChanged += UpdatePlayerMaxHpUI;
        playerController.CurrentLevelChanged += UpdatePlayerCurrentLevel;
        playerController.CurrentExpChanged += UpdatePlayerCurrentExpUI;
        playerController.ReqExpLevelUpChanged += UpdatePlayerReqExpLevelUpUI;
    }
    void OnDisable()
    {
        PlayerController playerController = ServiceLocator.Get<PlayerController>();
        if (!playerController)
            return;
        playerController.HPChanged -= UpdatePlayerHpUI;
        playerController.MaxHPChanged -= UpdatePlayerMaxHpUI;
        playerController.CurrentLevelChanged -= UpdatePlayerCurrentLevel;
        playerController.CurrentExpChanged -= UpdatePlayerCurrentExpUI;
        playerController.ReqExpLevelUpChanged -= UpdatePlayerReqExpLevelUpUI;
    }

    void Start()
    {
        InitData();
    }

    void InitData()
    {
        PlayerModel playerModel = ServiceLocator.Get<PlayerModel>();
        if (playerModel)
        {
            UpdatePlayerHpUI(playerModel.GetCurrentHP());
            UpdatePlayerMaxHpUI(playerModel.GetMaxHp());
        }

        PlayerLevelManager playerLevelManager = ServiceLocator.Get<PlayerLevelManager>();
        if (playerLevelManager)
        {
            UpdatePlayerCurrentLevel(playerLevelManager.currentLevel);
            UpdatePlayerReqExpLevelUpUI(playerLevelManager.GetExpReqToLevelUp());
        }

        UpdatePlayerCurrentExpUI(0);
    }

    void UpdatePlayerHpUI(int newHp)
    {
        HPTextElement.text = newHp.ToString();
    }
    void UpdatePlayerMaxHpUI(int newHp)
    {
        MaxHPTextElement.text = newHp.ToString();
    }
    void UpdatePlayerCurrentLevel(int level)
    {
        CurrentLevelTextElement.text = level.ToString();
    }
    void UpdatePlayerCurrentExpUI(int exp)
    {
        CurrentExpTextElement.text = exp.ToString();
    }
    void UpdatePlayerReqExpLevelUpUI(int reqExp)
    {
        ExpReqLevelUpTextElement.text = reqExp.ToString();
    }
}
