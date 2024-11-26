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
        Debug.Log($"{Time.time:F2}s �� {target.Name}��(��){origin.Name}�� ���� ���� ���¿� �����ϴ�. ������ �� �����ϴ�. (���� �ð�: {remain}��)");
    }
    public void UpdateEffect(Ship target, float deltaTime) {
        remain -= deltaTime;
        if (remain <= 0) {
            target.SetStunned(false);
            Debug.Log($"{Time.time:F2}s �� {target.Name}�� ���� ���°� �����Ǿ����ϴ�.");
            target.RemoveStatusEffect(this);
        }
    }
}
