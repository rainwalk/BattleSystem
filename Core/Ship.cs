using System.Collections.Generic;
using UnityEngine;

public class Ship
{
    public string Name { get; private set; }
    public int Count { get; private set; }
    public Weapon Weapon { get; private set; }

    private AttackHandler attackHandler;
    private SkillHandler skillHandler;
    private StatusEffectHandler statusEffectHandler;

    public Ship(string name, int health, Weapon weapon, List<SkillBase> skills) {
        Name = name;
        Count = health;
        Weapon = weapon;

        skillHandler = new SkillHandler(skills);
        statusEffectHandler = new StatusEffectHandler();
        attackHandler = new AttackHandler(weapon);
    }
    public void UpdateShip(float deltaTime) {
        skillHandler.UpdateSkills(deltaTime);
        statusEffectHandler.UpdateStatusEffect(this, deltaTime);
        attackHandler.UpdateCooldown(deltaTime);
    }
    public void TryAttack(Fleet targetFleet) {
        if (CanAttack()) {
            attackHandler.TryAttack(targetFleet, this);
        }
    }
    public bool TryUseSkill(Fleet targetFleet) => skillHandler.TryUseSkill(this, targetFleet);
    public bool IsStunned() => statusEffectHandler.IsStunned();
    public bool CanAttack() => IsAlive() && !IsStunned() && !skillHandler.IsSkillCasting() && attackHandler.IsAttackReady();
    public bool IsAlive() => Count > 0;

    public void SetStunned(bool stunned) => statusEffectHandler.SetStunned(stunned);
    public void ApplyStatusEffect(IStatusEffect effect, Ship origin, Ship target) => statusEffectHandler.ApplyStatusEffect(effect,origin,target);
    public void RemoveStatusEffect(IStatusEffect effect) => statusEffectHandler.RemoveStatusEffect(effect);
    public void ResetAttackCooldown() => attackHandler.ResetCooldown();
    public void TakeDamage(int damage) => Count = Mathf.Max(Count - damage, 0);
    public void Repair(int amount) => Count += amount;
}
