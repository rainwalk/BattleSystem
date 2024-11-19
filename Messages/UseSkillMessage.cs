using UnityEngine;

public class UseSkillMessage : IMessage
{
    public int Id { get; private set; }  // �޽��� ID
    public MessagePriority Priority { get; }

    private Ship originShip;      // ��ų�� ����ϴ� �Լ�
    private SkillBase skill;       // ����� ��ų
    private Fleet targetFleet;     // ��ų�� ��� �Դ�
    private Ship targetShip;       // ��ų�� ��� �Լ�

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
        Id = id;  // ID ����
    }
}
