public class OmegaStrikeSkill : SkillBase
{
    public OmegaStrikeSkill() : base("���ް� ��Ʈ����ũ", SkillTargetType.Single, 5f, 1f) {
        AddEffect(new DamageEffect(90)); 
    }

}
