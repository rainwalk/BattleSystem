using UnityEngine;

public class DamageEffect : IEffect
{
    private int damageAmount;

    public DamageEffect(int damageAmount) {
        this.damageAmount = damageAmount;
    }

    public  void ApplyEffect(Ship origin, Ship target) {
        target.TakeDamage(damageAmount);
        Debug.Log($"{Time.time:F2}s └ {target.Name}에게 {damageAmount}의 피해를 입혔습니다. (남은 함선: {target.Count})");
    }
}
