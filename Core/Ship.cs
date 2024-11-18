using System.Collections.Generic;
using UnityEngine;

public class Ship
{
    public string Name { get; private set; }
    public int Count { get; private set; }
    public Weapon Weapon { get; private set; }

    private SkillHandler skillHandler;
    private StatusEffectHandler statusEffectHandler;

    private float attackCooldown; 
    private float attackCooldownTimer; 

    private bool isStunned;

    public Ship(string name, int health, Weapon weapon, List<SkillBase> skills) {
        Name = name;
        Count = health;
        Weapon = weapon;

        skillHandler = new SkillHandler(skills);
        statusEffectHandler = new StatusEffectHandler();

        attackCooldown = weapon.FireRate;
        attackCooldownTimer = 0f;
        isStunned = false;
    }
    public bool IsStunned() => isStunned;
    public void SetStunned(bool stunned) => isStunned = stunned;
    public void ApplyStatusEffect(IStatusEffect effect) => statusEffectHandler.ApplyStatusEffect(effect,this);
    public void RemoveStatusEffect(IStatusEffect effect) => statusEffectHandler.RemoveStatusEffect(effect);

    public void UpdateShip(float deltaTime) {
        attackCooldownTimer = Mathf.Max(attackCooldownTimer - deltaTime, 0f);
        skillHandler.UpdateSkills(deltaTime);
        statusEffectHandler.UpdateStatusEffect(this,deltaTime);
    }
    public bool TryUseSkill(Fleet targetFleet) { return skillHandler.TryUseSkill(this, targetFleet); }
    public void TryAttack(Fleet targetFleet) {
        if (!CanAttack()) return;

        Ship targetShip = targetFleet.FindTarget();
        MessageManager.Instance.EnqueueMessage(new AttackMessage(MessagePriority.Normal, this, targetShip));
    }
    public bool CanAttack() => !skillHandler.IsSkillCasting() && IsAlive() && !isStunned && attackCooldownTimer <= 0;
    public void ResetAttackCooldown() => attackCooldownTimer = attackCooldown;
    public void TakeDamage(int damage) => Count = Mathf.Max(Count - damage, 0);
    public void Repair(int amount) => Count += amount;
    public bool IsAlive() => Count > 0;
}
