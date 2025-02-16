using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class Skill_Charge : MonoBehaviour
{
    public float cooldown = 10f;
    public float duration = 1.5f;
    public float moveSpeedMul = 2f;

    private float baseMoveSpeed;

    private float cooldownTimer;
    private float durationTimer;

    FollowerEntity ai;


    public void Run(FollowerEntity ai)
    {
        if (cooldownTimer > 0f)
            return;

        this.ai = ai;

        baseMoveSpeed = ai.maxSpeed;
        cooldownTimer = cooldown;
        durationTimer = duration;

        ai.maxSpeed = baseMoveSpeed * moveSpeedMul;
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
                ai.maxSpeed = baseMoveSpeed;
            }
        }
    }
}
