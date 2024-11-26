using UnityEngine;

public class DamageEffect : IEffect
{
    private int damageAmount;

    public DamageEffect(int damageAmount) {
        this.damageAmount = damageAmount;
    }

    public  void ApplyEffect(Ship origin, Ship target) {
        target.TakeDamage(damageAmount);
        Debug.Log($"{Time.time:F2}s �� {target.Name}���� {damageAmount}�� ���ظ� �������ϴ�. (���� �Լ�: {target.Count})");
    }
}
