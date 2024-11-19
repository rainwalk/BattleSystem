using System.Collections.Generic;
using UnityEngine;

public class SkillHandler
{
    private List<SkillBase> skills;

    public SkillHandler(List<SkillBase> skills) {
        this.skills = skills;
    }

    // 스킬 업데이트 처리
    public void UpdateSkills(float deltaTime) { foreach (SkillBase skill in skills) skill.Update(deltaTime); }
    public bool IsSkillCasting() => skills.Exists(skill => skill.State == CastingState.Casting);
    public SkillBase GetNextAvailableSkill() => skills.Find(skill => skill.IsAvailable());
    public bool TryUseSkill(Ship originShip, Fleet targetFleet) {
        SkillBase skill = GetNextAvailableSkill();
        if (skill == null) return false;

        var target = skill.TargetType == SkillTargetType.All ? (object)targetFleet : targetFleet.FindTarget();
        MessageManager.Instance.EnqueueMessage(new UseSkillMessage(MessagePriority.High, originShip, skill, target as Fleet, target as Ship));

        return true;
    }


}
