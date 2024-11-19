using System.Collections.Generic;
using UnityEngine;

// 스킬의 대상 타입 정의
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

    // 생성자
    public SkillBase(string name, SkillTargetType targetType, float cooldownTime, float castTime) {
        this.name = name;
        TargetType = targetType;
        this.cooldownTime = cooldownTime;
        this.castTime = castTime;
        effects = new List<IEffect>();
        id = skillCounter++; // 스킬 고유 ID 설정
    }
    public void AddEffect(IEffect effect) => effects.Add(effect);
    public void SetStatusEffect(IStatusEffect statusEffect) => this.statusEffect = statusEffect;

    // 사용 가능한지 확인
    public bool IsAvailable() => State == CastingState.Idle && cooldownTimer <= 0f;

    public void StartCast(Ship originShip, Fleet targetFleet = null, Ship targetShip = null) {
        if (!originShip.IsAlive()) {
            Debug.Log($"{Time.time:F2}s {originShip.Name}이(가) 파괴되어 스킬({id}) {name} 캐스팅 취소");
            return;
        }

        State = CastingState.Casting;
        castTimer = castTime;

        this.originShip = originShip;
        this.targetFleet = targetFleet;
        this.targetShip = targetShip;

        Debug.Log($"{Time.time:F2}s {originShip.Name}이(가) 스킬({id}) {name} 캐스팅 시작");
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
                Debug.LogWarning($"{Time.time:F2}s {originShip.Name}이(가) 대상 함대가 없어 스킬({id}) 시전 실패");
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
            Debug.LogWarning($"{Time.time:F2}s {originShip.Name}이(가) {id}] 스킬({id}) {name} 취소. 대상 함선 또는 함대가 없습니다.");
        }

        ResetCooldown();
    }

    private void ResetCooldown() {
        cooldownTimer = cooldownTime;
        Debug.Log($"{Time.time:F2}s {originShip?.Name}의 스킬({id}) {name} 쿨타임 시작");
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
        Debug.Log($"{Time.time:F2}s <color=white>{shipName}이(가) {targetName}에 스킬({skillId}) {skillName} 사용</color>");
    }
}
