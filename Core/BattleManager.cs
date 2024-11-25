using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    private Fleet playerFleet;
    private Fleet enemyFleet;
    private bool battleInProgress = false;
    private IBattleLoop battleLoop;

    private void InitializeFleets() {
        playerFleet = new Fleet("Player Fleet");
        playerFleet.AddShip(new Ship("Player Ship 1", 300, new Weapon(WeaponType.Cannon), new List<SkillBase> { new OmegaStrikeSkill() }));
        playerFleet.AddShip(new Ship("Player Ship 2", 300, new Weapon(WeaponType.Beam), new List<SkillBase> { new PlasmaRainSkill() }));
        playerFleet.AddShip(new Ship("Player Ship 3", 300, new Weapon(WeaponType.Beam), new List<SkillBase> { new EMPShockwaveSkill() }));

        enemyFleet = new Fleet("Enemy Fleet");
        enemyFleet.AddShip(new Ship("Enemy Ship 1", 100, new Weapon(WeaponType.Fighter), new List<SkillBase> { new OmegaStrikeSkill() }));
        enemyFleet.AddShip(new Ship("Enemy Ship 2", 200, new Weapon(WeaponType.Missile), new List<SkillBase> { new EMPShockwaveSkill() }));
        enemyFleet.AddShip(new Ship("Enemy Ship 3", 200, new Weapon(WeaponType.Missile), new List<SkillBase> { new PlasmaRainSkill() }));
        enemyFleet.AddShip(new Ship("Enemy Ship 4", 200, new Weapon(WeaponType.Missile), new List<SkillBase> { new OmegaStrikeSkill() }));
    }

    void Start() {
        InitializeFleets();
        battleLoop = new BattleLoop(playerFleet, enemyFleet);
        battleInProgress = true;
    }

    void Update() {
        if (battleInProgress) {
            battleLoop.Update();
            MessageManager.Instance.ProcessMessages();
            CheckVictory();
        }
    }

    private void CheckVictory() {
        if (!enemyFleet.IsAlive() || !playerFleet.IsAlive()) {
            battleInProgress = false;
            Debug.Log(enemyFleet.IsAlive() ? "적 승리!" : "플레이어 승리!");
        }
    }
}
