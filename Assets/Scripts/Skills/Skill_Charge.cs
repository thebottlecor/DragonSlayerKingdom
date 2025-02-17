using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class Skill_Charge : MonoBehaviour
{
    public float cooldown = 10f;
    public float duration = 1.5f;
    public float moveSpeedMul = 2f;
    private float moveBonus;

    public float armorBonus = 1f;

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
