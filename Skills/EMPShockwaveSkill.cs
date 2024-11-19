using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EMPShockwaveSkill : SkillBase
{
    public EMPShockwaveSkill() : base("EMP Ãæ°ÝÆÄ", SkillTargetType.Single, 10f, 2f) {
        AddEffect(new DamageEffect(30));
        AddEffect(new RepairEffect(20));
        SetStatusEffect(new StunEffect(5));
    }
}
