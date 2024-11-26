using UnityEngine;

public class RepairEffect : IEffect
{
    private int amount;

    public RepairEffect(int amount) {
        this.amount = amount;
    }
    public void ApplyEffect(Ship origin, Ship target) {
        origin.Repair(amount);
        Debug.Log($"{Time.time:F2}s └ {origin.Name}의 함선이 {amount}만큼 수리되었습니다. (현재 함선: {origin.Count})");
    }
}
