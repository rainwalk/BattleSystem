public class OmegaStrikeSkill : SkillBase
{
    public OmegaStrikeSkill() : base("오메가 스트라이크", SkillTargetType.Single, 5f, 1f) {
        AddEffect(new DamageEffect(90)); 
    }

}
