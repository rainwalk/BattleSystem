using System.Collections.Generic;
using UnityEngine;

// ��ų�� ��� Ÿ�� ����
public enum SkillTargetType
{
    Single,
    All
}

// ��ų�� �⺻ Ŭ����
public abstract class SkillBase
{
    public SkillTargetType TargetType { get; protected set; }
    public bool IsCasting { get; private set; }

    private string name;
    private float cooldownTime;
    private float castTime;
    private List<IEffect> effects;
    private IStatusEffect statusEffect;
    private int id;

    // ��ų ���� ID �ʱ�ȭ��
    private static int skillCounter = 1000;

    // Ÿ�̸ӵ� (��Ÿ�� �� ĳ����)
    private float cooldownTimer = 0f;
    private float castTimer = 0f;

    // ���� (��ų Ÿ�� ����)
    private Fleet targetFleet;     // ��ų�� ��� �Դ�
    private Ship targetShip;       // ��ų�� ��� �Լ�
    private Ship originShip;      // ��ų�� ����ϴ� �Լ�

    // ������ (��ų �⺻ ����)
    public SkillBase(string name, SkillTargetType targetType, float cooldownTime, float castTime) {
        this.name = name;
        TargetType = targetType;
        this.cooldownTime = cooldownTime;
        this.castTime = castTime;
        effects = new List<IEffect>();
        id = skillCounter++; // ��ų ���� ID ����
    }

    // ��ų�� ȿ�� �߰�
    public void AddEffect(IEffect effect) => effects.Add(effect);

    public void SetStatusEffect(IStatusEffect statusEffect) => this.statusEffect = statusEffect;

    // ��ų�� ��� �������� üũ
    public bool IsAvailable() => !IsCasting && cooldownTimer <= 0f;

    // ��ų ĳ���� ����
    public void StartCast(Ship originShip, Fleet targetFleet = null, Ship targetShip = null) {
        if (!originShip.IsAlive()) {
            Debug.Log($"{Time.time:F2}s {originShip?.Name}��(��) �ı��Ǿ� ��ų({id}) {name} ĳ���� ���");
            return;
        }

        IsCasting = true;
        castTimer = castTime;

        this.originShip = originShip;
        this.targetFleet = targetFleet;
        this.targetShip = targetShip;

        Debug.Log($"<color=white>{Time.time:F2}s {originShip.Name}��(��) ��ų({id}) {name} ĳ���� ����</color>");
    }

    // ��Ÿ�� �� ĳ���� ���� ������Ʈ
    public void UpdateCooldownAndCast(float deltaTime) {
        if (IsCasting) {
            castTimer -= deltaTime;
            if (castTimer <= 0) {
                CompleteCasting();
            }
        }
        else {
            cooldownTimer -= deltaTime;
            if (cooldownTimer <= 0) cooldownTimer = 0;
        }
    }

    // ĳ���� �Ϸ� �� ó��
    private void CompleteCasting() {
        IsCasting = false;

        // ����� All�� ���, �Դ뿡 ��ų ����
        if (TargetType == SkillTargetType.All) {
            if (targetFleet == null) {
                Debug.LogWarning($"{Time.time:F2}s {originShip?.Name}��(��) ��� �Դ밡 ���� ��ų({id}) ���� ����");
                return;
            }

            LogSkillExecution(originShip.Name, targetFleet.Name, id, name);
            Execute(targetFleet.Ships);
        }
        // ����� Single�� ���, ���� �Լ��� ��ų ����
        else if (targetShip != null) {
            LogSkillExecution(originShip.Name, targetShip.Name, id, name);
            Execute(targetShip);
        }
        else {
            Debug.LogWarning($"{Time.time:F2}s {originShip?.Name}��(��) {id}] ��ų({id}) {name} ���. ��� �Լ� �Ǵ� �Դ밡 �����ϴ�.");
        }

        ResetCooldown();
    }

    // ��Ÿ�� �ʱ�ȭ
    private void ResetCooldown() {
        cooldownTimer = cooldownTime;
        Debug.Log($"{Time.time:F2}s {originShip?.Name}�� ��ų({id}) {name} ��Ÿ�� ����");
    }

    // Ÿ�� ��Ͽ� ��ų ����
    public virtual void Execute(List<Ship> targets) {
        foreach (Ship target in targets) {
            Execute(target);
        }
    }

    // ���� Ÿ�ٿ� ��ų ����
    public virtual void Execute(Ship target) {
        foreach (IEffect effect in effects) {
            if (target.IsAlive()) effect.ApplyEffect(originShip, target);
        }
        if (statusEffect != null && target.IsAlive()) {
            target.ApplyStatusEffect(statusEffect);
        }
    }

    private void LogSkillExecution(string shipName, string targetName, int skillId, string skillName) {
        Debug.Log($"{Time.time:F2}s <color=white>{shipName}��(��) {targetName}�� ��ų({skillId}) {skillName} ���</color>");
    }
}
