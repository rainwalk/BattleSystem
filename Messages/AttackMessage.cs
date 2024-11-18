using UnityEngine;

public class AttackMessage : IMessage
{
    public int Id { get; private set; }
    public MessagePriority Priority { get; }
    private Ship attacker;
    private Ship target;

    public AttackMessage(MessagePriority priority, Ship attacker, Ship target) {
        Priority = priority;
        this.attacker = attacker;
        this.target = target;
    }

    public void Execute() {
        if (attacker == null || target == null) return;

        Debug.Log($"{Time.time:F2}s <color=white>{attacker.Name}(함선:{attacker.Count})가 {target.Name}(함선:{target.Count})를 {attacker.Weapon.Type} 공격합니다.</color>");
        target.TakeDamage(attacker.Weapon.Damage);
        Debug.Log($"{Time.time:F2}s └ {attacker.Name}이(가) {target.Name}에게 {attacker.Weapon.Damage}의 피해를 입혔습니다. ({target.Name}의 함선: {target.Count})");
        attacker.ResetAttackCooldown(); // 공격 후 쿨타임 초기화
    }
    public void SetId(int id) {
        Id = id;  // ID 설정
    }
}
