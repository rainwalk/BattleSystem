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
        Debug.Log($"{Time.time:F2}s �� {ship.Name}��(��) ���� ���¿� �����ϴ�. ������ �� �����ϴ�. (���� �ð�: {remain}��)");
    }
    public void UpdateEffect(Ship ship, float deltaTime) {
        remain -= deltaTime;
        if (remain <= 0) {
            ship.SetStunned(false);
            Debug.Log($"{Time.time:F2}s �� {ship.Name}�� ���� ���°� �����Ǿ����ϴ�.");
            ship.RemoveStatusEffect(this);
        }
    }
}
