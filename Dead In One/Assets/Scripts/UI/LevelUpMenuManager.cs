using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelUpMenuManager : MonoBehaviour
{
    [Header("Dependencies")]
    PlayerController playerController;
    [SerializeField] PlayerInputChannel inputChannel;

    [Header("UI Elements")]
    [SerializeField] GameObject levelUpMenu;
    [SerializeField] List<LevelUpCardController> levelUpCardControllers = new();

    void Awake()
    {
        playerController = ServiceLocator.Get<PlayerController>();
    }

    void OnEnable()
    {
        playerController.CurrentLevelChanged += HandleLevelUp;
    }
    void OnDisable()
    {
        playerController.CurrentLevelChanged -= HandleLevelUp;
    }


    void HandleLevelUp(int newLevel)
    {
        UpdateUpgradeCards();
        OpenLevelUpMenu();
    }

    // Utils
    public void OpenLevelUpMenu()
    {
        inputChannel.input.Disable();
        levelUpMenu.SetActive(true);
    }
    public void CloseLevelUpMenu()
    {
        levelUpMenu.SetActive(false);
        inputChannel.input.Enable();
    }
    void UpdateUpgradeCards()
    {
        // cache lists once
        var nonOwned = playerController.GetNonOwnedWeaponUIDs().ToList();
        var owned = playerController.ownedWeapons.Select(w => w.GetUID()).ToList();

        HashSet<string> offeredUpgrades = new();
        HashSet<string> offeredWeapons = new();
        foreach (LevelUpCardController upgradeCard in levelUpCardControllers)
        {
            var weaponsToOffer = nonOwned.Where(uid => !offeredWeapons.Contains(uid)).ToList();
            var upgradesToOffer = owned.Where(uid => !offeredUpgrades.Contains(uid)).ToList();

            bool newWeapon = RollChance(30);

            if ((newWeapon || upgradesToOffer.Count == 0) && weaponsToOffer.Count > 0)
            {
                // offer new weapon
                System.Random rnd = new System.Random();
                int randomIndex = rnd.Next(weaponsToOffer.Count);
                string randomItem = weaponsToOffer[randomIndex];
                offeredWeapons.Add(randomItem);

                UpdateRewardCard(upgradeCard, randomItem, true);
                upgradeCard.gameObject.SetActive(true);
            }
            else if (upgradesToOffer.Count > 0)
            {
                // offer upgrade
                System.Random rnd = new System.Random();
                int randomIndex = rnd.Next(upgradesToOffer.Count);
                string randomItem = upgradesToOffer[randomIndex];
                offeredUpgrades.Add(randomItem);

                UpdateRewardCard(upgradeCard, randomItem, false);
                upgradeCard.gameObject.SetActive(true);
            }
            else
            {
                upgradeCard.gameObject.SetActive(false);
            }
        }
    }

    void UpdateRewardCard(LevelUpCardController CardController, string UID, bool isNew)
    {
        CardController.weaponUID = UID;
        WeaponSO weapon = playerController.GetWeapon(UID);
        CardController.image.sprite = weapon.portrait;
        CardController.title.text = weapon.Name;
        CardController.description.text = weapon.CardDescription;
        if (isNew)
            CardController.newText.text = "NEW!";
        else
        {
            int currentLevel = playerController.ownedWeapons.Find(x => x.GetUID() == UID).GetLevel();
            int nextLevel = currentLevel + 1;
            CardController.newText.text = currentLevel + " -> " + nextLevel;
        }
    }
    public bool RollChance(float percent)
    {
        return Random.Range(0f, 100f) < percent;
    }
}
