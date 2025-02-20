using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class Skill_Charge : MonoBehaviour
{

    SkillInfo Info => DataManager.Instance.skills[Skill.Charge];

    float cooldown => Info.hiddenValue[0];
    float duration => Info.dataValue[0];
    float moveSpeedMul => (Info.dataValue[1] + 100f) / 100f;
    private float moveBonus;

    float armorBonus => Info.dataValue[2];

    private float cooldownTimer;
    private float durationTimer;

    FollowerEntity ai;
    RTSUnit unit;


    public void Run(FollowerEntity ai, RTSUnit unit)
    {
        if (cooldownTimer > 0f)
            return;

        this.ai = ai;
        this.unit = unit;

        moveBonus = (moveSpeedMul - 1f) * unit.Info.MoveSpeed;
        cooldownTimer = cooldown;
        durationTimer = duration;

        ai.maxSpeed += moveBonus;
        unit.armor += armorBonus;
    }


    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (durationTimer > 0f)
        {
            durationTimer -= Time.deltaTime;
            if (durationTimer <= 0f)
            {
                ai.maxSpeed -= moveBonus;
                unit.armor -= armorBonus;
            }
        }
    }
}
