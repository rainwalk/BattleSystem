using System.Collections.Generic;
using UnityEngine;

public class BattleLoop : IBattleLoop
{
    private Fleet playerFleet;
    private Fleet enemyFleet;
    private float timeSinceLastUpdate = 0f;
    private float updateInterval = 0.1f;

    public BattleLoop(Fleet playerFleet, Fleet enemyFleet) {
        this.playerFleet = playerFleet;
        this.enemyFleet = enemyFleet;
    }
    
    public void Update() {
        timeSinceLastUpdate += Time.deltaTime;

        if (timeSinceLastUpdate >= updateInterval) {
            ProcessShips(playerFleet, enemyFleet);
            ProcessShips(enemyFleet, playerFleet);
            timeSinceLastUpdate = 0f;
        }
    }
    
    private void ProcessShips(Fleet fleet, Fleet targetFleet) {
        foreach (Ship ship in fleet.Ships) {
            if (!ship.IsAlive() || ship.IsStunned()) continue;
            if (!ship.TryUseSkill(targetFleet)) ship.TryAttack(targetFleet);
            ship.UpdateShip(updateInterval);
        }
    }

}
