using UnityEngine;

public class RepairEffect : EffectBase
{
    private int amount;

    public RepairEffect(int amount) {
        EffectName = "수리";
        this.amount = amount;
    }
    public override void ApplyEffect(Ship origin, Ship target) {
        base.ApplyEffect(origin, target);
        origin.Repair(amount);
        Debug.Log($"{Time.time:F2}s └ {origin.Name}의 함선이 {amount}만큼 수리되었습니다. (현재 함선: {origin.Count})");
    }
}
