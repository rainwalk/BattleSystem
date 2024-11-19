using UnityEngine;

public class UseSkillMessage : IMessage
{
    public int Id { get; private set; }  // 메시지 ID
    public MessagePriority Priority { get; }

    private Ship originShip;      // 스킬을 사용하는 함선
    private SkillBase skill;       // 사용할 스킬
    private Fleet targetFleet;     // 스킬의 대상 함대
    private Ship targetShip;       // 스킬의 대상 함선

    public UseSkillMessage(MessagePriority priority, Ship originShip, SkillBase skill, Fleet targetFleet = null, Ship targetShip = null) {
        Priority = priority;
        this.originShip = originShip;
        this.skill = skill;
        this.targetFleet = targetFleet;
        this.targetShip = targetShip;
    }
    public void Execute() {
        skill.StartCast(originShip, targetFleet, targetShip);

    }
    public void SetId(int id) {
        Id = id;  // ID 설정
    }
}
