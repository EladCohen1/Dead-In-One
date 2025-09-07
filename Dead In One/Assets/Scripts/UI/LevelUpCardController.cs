using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpCardController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] Button btn;
    [SerializeField] LevelUpMenuManager levelUpMenuManager;

    [Header("UI Elements")]
    public Image image;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public TextMeshProUGUI newText;


    // Runtime Data
    public string weaponUID;

    void Awake()
    {
        btn.onClick.AddListener(HandleCardClick);
    }

    void HandleCardClick()
    {
        ServiceLocator.Get<PlayerController>().GetReward(weaponUID);
        levelUpMenuManager.CloseLevelUpMenu();
    }
}
