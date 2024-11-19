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

        Debug.Log($"{Time.time:F2}s <color=white>{attacker.Name}(�Լ�:{attacker.Count})�� {target.Name}(�Լ�:{target.Count})�� {attacker.Weapon.Type} �����մϴ�.</color>");
        target.TakeDamage(attacker.Weapon.Damage);
        Debug.Log($"{Time.time:F2}s �� {attacker.Name}��(��) {target.Name}���� {attacker.Weapon.Damage}�� ���ظ� �������ϴ�. ({target.Name}�� �Լ�: {target.Count})");
        attacker.ResetAttackCooldown(); // ���� �� ��Ÿ�� �ʱ�ȭ
    }
    public void SetId(int id) {
        Id = id;  // ID ����
    }
}
