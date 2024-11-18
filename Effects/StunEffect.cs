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
    public void ApplyEffect(Ship ship) {
        remain = Duration;
        ship.SetStunned(true);
        Debug.Log($"{Time.time:F2}s └ {ship.Name}은(는) 마비 상태에 들어갔습니다. 공격할 수 없습니다. (지속 시간: {remain}초)");
    }
    public void UpdateEffect(Ship ship, float deltaTime) {
        remain -= deltaTime;
        if (remain <= 0) {
            ship.SetStunned(false);
            Debug.Log($"{Time.time:F2}s └ {ship.Name}의 마비 상태가 해제되었습니다.");
            ship.RemoveStatusEffect(this);
        }
    }
}
