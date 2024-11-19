using System.Collections.Generic;
using UnityEngine;

// ��ų�� ��� Ÿ�� ����
public enum SkillTargetType
{
    Single,
    All
}
public enum CastingState
{
    Idle,
    Casting,
    Cooldown
}

public abstract class SkillBase
{
    public SkillTargetType TargetType { get; protected set; }
    public CastingState State { get; private set; }

    private string name;
    private float cooldownTime;
    private float castTime;
    private List<IEffect> effects;
    private IStatusEffect statusEffect;
    private int id;

    private static int skillCounter = 1000;

    private float cooldownTimer = 0f;
    private float castTimer = 0f;

    private Ship originShip;
    private Ship targetShip;
    private Fleet targetFleet;

    // ������
    public SkillBase(string name, SkillTargetType targetType, float cooldownTime, float castTime) {
        this.name = name;
        TargetType = targetType;
        this.cooldownTime = cooldownTime;
        this.castTime = castTime;
        effects = new List<IEffect>();
        id = skillCounter++; // ��ų ���� ID ����
    }
    public void AddEffect(IEffect effect) => effects.Add(effect);
    public void SetStatusEffect(IStatusEffect statusEffect) => this.statusEffect = statusEffect;

    // ��� �������� Ȯ��
    public bool IsAvailable() => State == CastingState.Idle && cooldownTimer <= 0f;

    public void StartCast(Ship originShip, Fleet targetFleet = null, Ship targetShip = null) {
        if (!originShip.IsAlive()) {
            Debug.Log($"{Time.time:F2}s {originShip.Name}��(��) �ı��Ǿ� ��ų({id}) {name} ĳ���� ���");
            return;
        }

        State = CastingState.Casting;
        castTimer = castTime;

        this.originShip = originShip;
        this.targetFleet = targetFleet;
        this.targetShip = targetShip;

        Debug.Log($"{Time.time:F2}s {originShip.Name}��(��) ��ų({id}) {name} ĳ���� ����");
    }

    public void Update(float deltaTime) {
        switch (State) {
            case CastingState.Casting:
                castTimer -= deltaTime;
                if (castTimer <= 0) CompleteCasting();
                break;

            case CastingState.Idle:
                cooldownTimer -= deltaTime;
                if (cooldownTimer <= 0) cooldownTimer = 0;
                break;
        }
    }

    private void CompleteCasting() {
        State = CastingState.Idle;

        if (TargetType == SkillTargetType.All) {
            if (targetFleet == null) {
                Debug.LogWarning($"{Time.time:F2}s {originShip.Name}��(��) ��� �Դ밡 ���� ��ų({id}) ���� ����");
                return;
            }
            LogSkillExecution(originShip.Name, targetFleet.Name, id, name);
            Execute(targetFleet.Ships);
        }
        else if (targetShip != null) {
            LogSkillExecution(originShip.Name, targetShip.Name, id, name);
            Execute(targetShip);
        }
        else {
            Debug.LogWarning($"{Time.time:F2}s {originShip.Name}��(��) {id}] ��ų({id}) {name} ���. ��� �Լ� �Ǵ� �Դ밡 �����ϴ�.");
        }

        ResetCooldown();
    }

    private void ResetCooldown() {
        cooldownTimer = cooldownTime;
        Debug.Log($"{Time.time:F2}s {originShip?.Name}�� ��ų({id}) {name} ��Ÿ�� ����");
    }

    public virtual void Execute(List<Ship> targets) {
        foreach (Ship target in targets) {
            Execute(target);
        }
    }

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
