using UnityEngine;

public class RepairEffect : EffectBase
{
    private int amount;

    public RepairEffect(int amount) {
        EffectName = "����";
        this.amount = amount;
    }
    public override void ApplyEffect(Ship origin, Ship target) {
        base.ApplyEffect(origin, target);
        origin.Repair(amount);
        Debug.Log($"{Time.time:F2}s �� {origin.Name}�� �Լ��� {amount}��ŭ �����Ǿ����ϴ�. (���� �Լ�: {origin.Count})");
    }
}
