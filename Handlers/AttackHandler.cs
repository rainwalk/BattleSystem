using UnityEngine;

public class AttackHandler
{
    private Weapon weapon;
    private float attackCooldownTimer;

    public AttackHandler(Weapon weapon) {
        this.weapon = weapon;
        attackCooldownTimer = 0f;
    }

    public void UpdateCooldown(float deltaTime) {
        attackCooldownTimer = Mathf.Max(attackCooldownTimer - deltaTime, 0f);
    }
    public void TryAttack(Fleet targetFleet, Ship originShip) {
        if (IsAttackReady()) {
            Ship targetShip = targetFleet.FindTarget();
            MessageManager.Instance.EnqueueMessage(new AttackMessage(MessagePriority.Normal, originShip, targetShip));
        }
    }

    public void ResetCooldown() => attackCooldownTimer = weapon.FireRate;
    public bool IsAttackReady() => attackCooldownTimer <= 0f;
}
