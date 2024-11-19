using System.Collections.Generic;
using UnityEngine;

public class PlasmaRainSkill : SkillBase
{
    public PlasmaRainSkill() : base("플라즈마 레인", SkillTargetType.All, 10f, 3f) {
        AddEffect(new DamageEffect(40));
    }
}
