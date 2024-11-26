using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StunEffect : IStatusEffect
{
    public float Duration { get; private set; }
    private float remain;

    public StunEffect(float duration) {
        Duration = duration;
    }
    public void ApplyEffect(Ship origin, Ship target) {
        remain = Duration;
        target.SetStunned(true);
        Debug.Log($"{Time.time:F2}s └ {target.Name}은(는){origin.Name}에 의해 마비 상태에 들어갔습니다. 공격할 수 없습니다. (지속 시간: {remain}초)");
    }
    public void UpdateEffect(Ship target, float deltaTime) {
        remain -= deltaTime;
        if (remain <= 0) {
            target.SetStunned(false);
            Debug.Log($"{Time.time:F2}s └ {target.Name}의 마비 상태가 해제되었습니다.");
            target.RemoveStatusEffect(this);
        }
    }
}
