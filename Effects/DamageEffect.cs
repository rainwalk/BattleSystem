using UnityEngine;

public class DamageEffect : EffectBase
{
    private int damageAmount;

    public DamageEffect(int damageAmount) {
        EffectName = "Damage";
        this.damageAmount = damageAmount;
    }

    public override void ApplyEffect(Ship origin, Ship target) {
        base.ApplyEffect(origin, target);
        target.TakeDamage(damageAmount);
        Debug.Log($"{Time.time:F2}s �� {target.Name}���� {damageAmount}�� ���ظ� �������ϴ�. (���� �Լ�: {target.Count})");
    }
}
