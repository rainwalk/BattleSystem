using System.Collections.Generic;
using UnityEngine;

// 스킬의 대상 타입 정의
public enum SkillTargetType
{
    Single,
    All
}

// 스킬의 기본 클래스
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

    // 스킬 고유 ID 초기화용
    private static int skillCounter = 1000;

    // 타이머들 (쿨타임 및 캐스팅)
    private float cooldownTimer = 0f;
    private float castTimer = 0f;

    // 대상들 (스킬 타겟 관련)
    private Fleet targetFleet;     // 스킬의 대상 함대
    private Ship targetShip;       // 스킬의 대상 함선
    private Ship originShip;      // 스킬을 사용하는 함선

    // 생성자 (스킬 기본 설정)
    public SkillBase(string name, SkillTargetType targetType, float cooldownTime, float castTime) {
        this.name = name;
        TargetType = targetType;
        this.cooldownTime = cooldownTime;
        this.castTime = castTime;
        effects = new List<IEffect>();
        id = skillCounter++; // 스킬 고유 ID 설정
    }

    // 스킬에 효과 추가
    public void AddEffect(IEffect effect) => effects.Add(effect);

    public void SetStatusEffect(IStatusEffect statusEffect) => this.statusEffect = statusEffect;

    // 스킬이 사용 가능한지 체크
    public bool IsAvailable() => !IsCasting && cooldownTimer <= 0f;

    // 스킬 캐스팅 시작
    public void StartCast(Ship originShip, Fleet targetFleet = null, Ship targetShip = null) {
        if (!originShip.IsAlive()) {
            Debug.Log($"{Time.time:F2}s {originShip?.Name}이(가) 파괴되어 스킬({id}) {name} 캐스팅 취소");
            return;
        }

        IsCasting = true;
        castTimer = castTime;

        this.originShip = originShip;
        this.targetFleet = targetFleet;
        this.targetShip = targetShip;

        Debug.Log($"<color=white>{Time.time:F2}s {originShip.Name}이(가) 스킬({id}) {name} 캐스팅 시작</color>");
    }

    // 쿨타임 및 캐스팅 진행 업데이트
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

    // 캐스팅 완료 후 처리
    private void CompleteCasting() {
        IsCasting = false;

        // 대상이 All일 경우, 함대에 스킬 적용
        if (TargetType == SkillTargetType.All) {
            if (targetFleet == null) {
                Debug.LogWarning($"{Time.time:F2}s {originShip?.Name}이(가) 대상 함대가 없어 스킬({id}) 시전 실패");
                return;
            }

            LogSkillExecution(originShip.Name, targetFleet.Name, id, name);
            Execute(targetFleet.Ships);
        }
        // 대상이 Single일 경우, 단일 함선에 스킬 적용
        else if (targetShip != null) {
            LogSkillExecution(originShip.Name, targetShip.Name, id, name);
            Execute(targetShip);
        }
        else {
            Debug.LogWarning($"{Time.time:F2}s {originShip?.Name}이(가) {id}] 스킬({id}) {name} 취소. 대상 함선 또는 함대가 없습니다.");
        }

        ResetCooldown();
    }

    // 쿨타임 초기화
    private void ResetCooldown() {
        cooldownTimer = cooldownTime;
        Debug.Log($"{Time.time:F2}s {originShip?.Name}의 스킬({id}) {name} 쿨타임 시작");
    }

    // 타겟 목록에 스킬 적용
    public virtual void Execute(List<Ship> targets) {
        foreach (Ship target in targets) {
            Execute(target);
        }
    }

    // 단일 타겟에 스킬 적용
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
