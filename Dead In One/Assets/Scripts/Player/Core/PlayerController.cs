using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] PlayerInputChannel inputChannel;
    [SerializeField] TurnManager turnManager;
    [SerializeField] MainBoardGrid mainBoardGrid;

    [Header("Components")]
    [SerializeField] PlayerView playerView;
    [SerializeField] PlayerModel playerModel;

    [Header("Weapons")]
    public List<WeaponController> ownedWeapons = new();

    // Events
    public event Action EndPlayerTurnEvent;
    public event Action StartPlayerTurnEvent;
    public event Action PlayerDeathEvent;

    void Awake()
    {
        ServiceLocator.Register(this);
        InitStarterWeapons();
    }

    void OnEnable()
    {
        inputChannel.OnMoveEvent += HandleMoveEvent;
        turnManager.PlayerTurnStart += StartPlayerTurn;
    }

    void OnDisable()
    {
        inputChannel.OnMoveEvent -= HandleMoveEvent;
        turnManager.PlayerTurnStart -= StartPlayerTurn;
    }

    // Event Handlers
    void HandleMoveEvent(Vector2Int dir)
    {
        if (turnManager.turnSate != TurnStateEnum.PlayerTurn)
            return;
        bool turnEnded = false;
        try
        {
            playerView.MovePlayer(dir);

            // Recharge Weapons
            foreach (WeaponController weapon in ownedWeapons)
                weapon.RechargeOnMove();

            if (playerModel.UseActionPoints(1))
            {
                turnEnded = true;
                return;
            }
        }
        finally
        {
            Attack();
            if (turnEnded)
                EndPlayersTurn();
        }
    }

    // Logic
    void Attack()
    {
        foreach (WeaponController weapon in ownedWeapons)
        {
            if (!weapon.Attack())
                continue;

            int rolledDamage = weapon.RollDamage(out bool didCrit);

            foreach (Vector2Int attackDir in weapon.GetAttackDirs())
            {
                Vector2Int target = attackDir + playerView.currentPos;
                if (!mainBoardGrid.IsInRange(target))
                    continue;

                if (didCrit)
                    mainBoardGrid.GetTile(target).FlashAttackedMaterial(weapon.GetAttackingCritMat(), 0.2f);
                else
                    mainBoardGrid.GetTile(target).FlashAttackedMaterial(weapon.GetAttackingMat(), 0.2f);

                EntityView attackedEntity = mainBoardGrid.GetEntity(target);
                if (attackedEntity == null)
                    continue;
                if (attackedEntity is not EnemyView enemyView)
                    continue;

                if (didCrit)
                    enemyView.enemyController.TakeDamage((int)(rolledDamage * playerModel.GetDamageMod()), weapon.GetAttackedMat());
                else
                    enemyView.enemyController.TakeDamage((int)(rolledDamage * playerModel.GetDamageMod()), weapon.GetAttackedMat());
            }
        }
    }


    // Public Action
    public void EndPlayersTurn()
    {
        EndPlayerTurnEvent?.Invoke();
    }
    public void StartPlayerTurn()
    {
        StartPlayerTurnEvent?.Invoke();
        playerModel.ResetActionPoints();
    }
    public Vector2Int GetPlayerPos()
    {
        return playerView.currentPos;
    }
    public void TakeDamage(int damage)
    {
        playerModel.TakeDamage(damage);
        playerView.FlashAttacked();
        if (playerModel.HP <= 0)
            PlayerDeathEvent?.Invoke();
    }

    // Public Data
    public Vector2Int GetCurrentPosition()
    {
        return playerView.currentPos;
    }

    void InitStarterWeapons()
    {
        foreach (WeaponSO weaponSO in playerModel.GetStarterWeaponSOs())
        {
            ownedWeapons.Add(InitWeapon(weaponSO));
        }
    }
    WeaponController InitWeapon(WeaponSO weaponSO)
    {
        return new WeaponController(weaponSO);
    }
}
