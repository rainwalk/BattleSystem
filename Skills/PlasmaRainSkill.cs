using System.Collections.Generic;
using UnityEngine;

public class PlasmaRainSkill : SkillBase
{
    public PlasmaRainSkill() : base("�ö�� ����", SkillTargetType.All, 10f, 3f) {
        AddEffect(new DamageEffect(40));
    }
}
