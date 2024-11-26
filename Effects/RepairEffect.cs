using UnityEngine;

public class RepairEffect : IEffect
{
    private int amount;

    public RepairEffect(int amount) {
        this.amount = amount;
    }
    public void ApplyEffect(Ship origin, Ship target) {
        origin.Repair(amount);
        Debug.Log($"{Time.time:F2}s �� {origin.Name}�� �Լ��� {amount}��ŭ �����Ǿ����ϴ�. (���� �Լ�: {origin.Count})");
    }
}
