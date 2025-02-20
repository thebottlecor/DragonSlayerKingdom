using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class Buff_FireArrow : Buff
{
    SkillInfo Info => DataManager.Instance.skills[Skill.FireArrow];

    float duration => Info.dataValue[0];
    float damage => Info.dataValue[1];

    private float durationTimer;

    private float damageTimer;


    public override void Run(RTSUnit unit)
    {
        this.unit = unit;

        durationTimer = duration;
    }


    void Update()
    {
        damageTimer += Time.deltaTime;
        if (damageTimer >= 1f)
        {
            damageTimer = 0f;
            unit.ApplyDamage(damage, DamageType.fire, null);
        }

        if (durationTimer > 0f)
        {
            durationTimer -= Time.deltaTime;
            if (durationTimer <= 0f)
            {
                Remove();
            }
        }
    }
}
