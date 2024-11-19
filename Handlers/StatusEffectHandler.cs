using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectHandler
{
    private readonly List<IStatusEffect> activeStatusEffects; // 활성 상태 효과
    private readonly List<IStatusEffect> statusEffectsToRemove;// 제거 대기 상태 효과

    private bool isStunned;

    public bool IsStunned() => isStunned;
    public void SetStunned(bool stunned) => isStunned = stunned;

    public StatusEffectHandler() {
        activeStatusEffects = new List<IStatusEffect>();
        statusEffectsToRemove = new List<IStatusEffect>();
    }
    public void ApplyStatusEffect(IStatusEffect effect, Ship ship) {
        effect.ApplyEffect(ship);
        activeStatusEffects.Add(effect);
    }
    public void RemoveStatusEffect(IStatusEffect effect) => statusEffectsToRemove.Add(effect);
    public void UpdateStatusEffect(Ship ship, float deltaTime) {
        foreach (IStatusEffect effect in activeStatusEffects) effect.UpdateEffect(ship, deltaTime);
        foreach (IStatusEffect effect in statusEffectsToRemove) activeStatusEffects.Remove(effect);
        statusEffectsToRemove.Clear();
    }


}
