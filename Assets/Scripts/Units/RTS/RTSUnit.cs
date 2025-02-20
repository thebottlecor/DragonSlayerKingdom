using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Profiling;
using Pathfinding;
using Pathfinding.RVO;
using Pathfinding.Util;
using Pathfinding.Pooling;


public class RTSUnit : VersionedMonoBehaviour
{
	public UnitIdx idx;
	public int team;

	private Skill_Charge skill_Charge;

	public float currentHealth;
	public float currentMana;
	IAstarAI ai;
	FollowerEntity rvo;
	MovementMode movementMode;
	Vector3 lastDestination;

	RTSUnit attackTarget;
	RTSWeapon weapon;
	float lastSeenAttackTarget = float.NegativeInfinity;
	bool reachedDestination;
	public bool locked;
	new Transform transform;

	public SpriteRenderer sprite;

	/// <summary>Position at the start of the current frame</summary>
	protected Vector3 position;
	public UnitInfo Info => DataManager.Instance.units[idx];
	public HashSet<Skill> SkillSet => PerkManager.Instance.unitHasSkills[idx];
	public Dictionary<Skill, SkillInfo> SkillInfos => DataManager.Instance.skills;

	private List<Buff> buffs;

	public float armor;
	public float Armor => Info.Armor + armor;

	public float Radius
	{
		get
		{
			return rvo != null ? rvo.radius : 1f;
		}
	}

	public void SetDestination(Vector3 destination, MovementMode mode)
	{
		if (ai != null && this)
		{
			reachedDestination = false;
			movementMode = mode;
			ai.destination = lastDestination = destination;
			//(ai as AIBase).rvoDensityBehavior.ClearDestinationReached();

			ai.SearchPath();
			if (mode == MovementMode.Move)
			{
				attackTarget = null;
			}
		}
	}

	public void Set_Idx(UnitIdx idx)
    {
		this.idx = idx;
    }
	protected override void Awake()
	{
		base.Awake();
		transform = (this as MonoBehaviour).transform;
		ai = GetComponent<IAstarAI>();
		//rvo = GetComponent<RVOController>();
		rvo = GetComponent<FollowerEntity>();
		
		weapon = GetComponent<RTSWeapon>();
		buffs = new List<Buff>();
	}
    private void Start()
    {
		if (ai != null)
			ai.maxSpeed = Info.MoveSpeed;
	}

    static System.Action<RTSUnit[], int> OnUpdateDelegate;
	void OnEnable()
	{
		if (OnUpdateDelegate == null) OnUpdateDelegate = OnUpdate;
		RTSManager.instance.units.AddUnit(this);
		if (DataManager.Instance != null)
		{
			currentHealth = Info.MaxHealth;
			currentMana = Info.MaxMana;
		}
		movementMode = MovementMode.AttackMove;
		reachedDestination = true;
		if (ai != null) lastDestination = ai.destination;
		BatchedEvents.Add(this, BatchedEvents.Event.Update, OnUpdateDelegate);
	}

	void OnDisable()
	{
		BatchedEvents.Remove(this);
		if (RTSManager.instance != null) RTSManager.instance.units.RemoveUnit(this);
	}

	static void OnUpdate(RTSUnit[] units, int count)
	{
		// Get some lists and arrays from an object pool
		List<RTSUnit>[] unitsByOwner = ArrayPool<List<RTSUnit>>.ClaimWithExactLength(RTSManager.PlayerCount);
		for (int i = 0; i < unitsByOwner.Length; i++)
		{
			unitsByOwner[i] = ListPool<RTSUnit>.Claim();
		}
		for (int i = 0; i < count; i++)
		{
			units[i].position = units[i].transform.position;
			unitsByOwner[units[i].team].Add(units[i]);
		}
		for (int i = 0; i < count; i++)
		{
			units[i].OnUpdate(unitsByOwner);
		}

		// Release allocated lists back to a pool
		for (int i = 0; i < unitsByOwner.Length; i++)
		{
			ListPool<RTSUnit>.Release(ref unitsByOwner[i]);
		}
		ArrayPool<List<RTSUnit>>.Release(ref unitsByOwner, true);
	}

	// Update is called once per frame
	protected virtual void OnUpdate(List<RTSUnit>[] unitsByOwner)
	{
		if (ai == null)
		{
			// Stationary unit

			if (weapon != null)
            {
                DetectEnemy(unitsByOwner);

                if (attackTarget != null)
                {
                    if (!weapon.InRangeOf(attackTarget.position))
                    {
                        attackTarget = null;
                    }
                    else
                    {
                        if (weapon.Aim(attackTarget))
                        {
                            weapon.Attack(attackTarget);
                            MultiAttack(unitsByOwner);
                        }
                    }
                }
            }

            if (attackTarget != null)
			{
				lastSeenAttackTarget = Time.time;
			}
		}
		else
		{
			//rvo.locked = false | locked;
			if (rvo != null)
				rvo.rvoSettings.locked = false | locked;

			// this.reachedDestination will be true once the AI has reached its destination
			// and it will stay true until the next time SetDestination is called.
			//reachedDestination |= (ai as AIBase).rvoDensityBehavior.reachedDestination;
			reachedDestination |= (ai as FollowerEntity).reachedDestination;

			if (weapon != null)
			{
				bool canAttack = movementMode == MovementMode.AttackMove;
				// This takes into account path calculations as well as if the AI stops far away from the destination due to being part of a large group
				canAttack |= reachedDestination && movementMode == MovementMode.Move;

				if (canAttack)
				{
					//Profiler.BeginSample("Distance");
					DetectEnemy(unitsByOwner);
					//Profiler.EndSample();

					if (attackTarget != null && (attackTarget.position - position).magnitude > Info.rangeFuzz)
					{
						attackTarget = null;
					}

					bool wantsToAttack = false;

					if (attackTarget != null)
					{
						if (!weapon.InRangeOf(attackTarget.position))
						{
							ai.destination = attackTarget.position;

							if (SkillInfos[Skill.Charge].ChanceCheck(SkillSet))
							{
								if (skill_Charge == null)
									skill_Charge = gameObject.AddComponent<Skill_Charge>();
								skill_Charge.Run(rvo, this);
							}
						}
						else
						{
							wantsToAttack = true;
							if (weapon.Aim(attackTarget))
							{
								if (SkillInfos[Skill.DoubleShot].ChanceCheck(SkillSet))
								{
									StartCoroutine(DelayDoubleAttack());
								}
								weapon.Attack(attackTarget);
								MultiAttack(unitsByOwner);
							}
						}
					}

					if (attackTarget != null)
					{
						lastSeenAttackTarget = Time.time;
					}

					if (!Info.canMoveWhileAttacking && (wantsToAttack || weapon.IsAttacking))
					{
						//rvo.locked = true;
						if (rvo != null)
							rvo.rvoSettings.locked = true;
					}
				}
			}

			// Move back to original destination in case we followed an enemy for some time
			if (Time.time - lastSeenAttackTarget > 2)
			{
				ai.destination = lastDestination;
			}
		}

		if (Info.RegenHealth > 0f)
        {
			currentHealth += Time.deltaTime * Info.RegenHealth;
			if (currentHealth > Info.MaxHealth) currentHealth = Info.MaxHealth;
        }
		if (Info.RegenMana > 0f)
        {
			currentMana += Time.deltaTime * Info.RegenMana;
			if (currentMana > Info.MaxMana) currentMana = Info.MaxMana;
		}
	}

    private void DetectEnemy(List<RTSUnit>[] unitsByOwner)
    {
		float minDist = Info.detectionRangeSquare;
        for (int player = 0; player < unitsByOwner.Length; player++)
        {
            if (team == player) continue;

            var enemies = unitsByOwner[player];
            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];
                var dist = (enemy.position - position).sqrMagnitude;
                if (dist < minDist)
                {
                    attackTarget = enemy;
                    minDist = dist;
                }
            }
        }
	}
	private void MultiAttack(List<RTSUnit>[] unitsByOwner)
	{
		int bonusTarget = 0;
		if (SkillInfos[Skill.Stomping].ChanceCheck(SkillSet))
		{
			bonusTarget = (int)SkillInfos[Skill.Stomping].dataValue[0] - 1;
		}

		int maxTarget = Info.MaxTarget + bonusTarget;
		if (maxTarget > 1)
		{
			HashSet<RTSUnit> excludeSet = new HashSet<RTSUnit>();
			excludeSet.Add(attackTarget);
			for (int n = 1; n < maxTarget; n++)
			{
				var otherTarget = DetectEnemy(unitsByOwner, excludeSet);
				if (otherTarget != null)
					weapon.Attack(otherTarget);
			}
		}
	}
	private RTSUnit DetectEnemy(List<RTSUnit>[] unitsByOwner, HashSet<RTSUnit> excludeSet)
	{
		RTSUnit otherTarget = null;
		float minDist = Info.detectionRangeSquare;
		for (int player = 0; player < unitsByOwner.Length; player++)
		{
			if (team == player) continue;

			var enemies = unitsByOwner[player];
			for (int i = 0; i < enemies.Count; i++)
			{
				var enemy = enemies[i];

				if (excludeSet.Contains(enemy)) continue;

				if (weapon.InRangeOf(enemy.position))
				{
					var dist = (enemy.position - position).sqrMagnitude;
					if (dist < minDist)
					{
						otherTarget = enemy;
						minDist = dist;
					}
				}
			}
		}
		if (otherTarget != null)
        {
			excludeSet.Add(otherTarget);
        }
		return otherTarget;
	}

	private IEnumerator DelayDoubleAttack()
    {
		yield return CoroutineHelper.WaitForSeconds(0.1f);

		if (attackTarget != null)
			weapon.Attack(attackTarget);
	}

	public void Die()
	{
		if (Info.type == UnitType.EnemySpawner)
        {
			GM.Instance.Win(); // 임시
        }

		StartCoroutine(DieCoroutine());
	}

	IEnumerator DieCoroutine()
	{
		yield return new WaitForEndOfFrame();
		if (team == 2)
		{
			//if (gold > 0)
			//	GM.Instance.AddGold(gold);

			if (Info.exp > 0f)
				GM.Instance.AddExp(Info.exp);
		}
		if (Info.deathEffect != null) GameObject.Instantiate(Info.deathEffect, transform.position, transform.rotation);
		GameObject.Destroy(gameObject);
	}

	public void ApplyDamage(float damage, DamageType damageType, RTSUnit damageSource)
	{
		if (damageType == DamageType.ranged)
		{
			if (SkillInfos[Skill.Defend].ChanceCheck(SkillSet))
            {
				return;
            }
		}

		float realDam = damage - Armor;
		realDam = Mathf.Max(1f, realDam); // 최소 데미지 1

		if (SkillInfos[Skill.Block].ChanceCheck(SkillSet))
		{
			realDam -= SkillInfos[Skill.Block].dataValue[0];
			realDam = Mathf.Max(0f, realDam);
		}

		if (damageSource != null)
		{
			if (SkillInfos[Skill.FireArrow].ChanceCheck(SkillSet))
            {
				AddBuff<Buff_FireArrow>(this);
			}
		}

		currentHealth = Mathf.Clamp(currentHealth - realDam, 0, Info.MaxHealth);

		if (Info.type == UnitType.Wall)
		{
			GM.Instance.CalcWallHp();
		}

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	public void AddBuff<T>(RTSUnit target) where T : Buff
	{
		var sameBuff = target.SameBuffCheck<T>();
		if (sameBuff == null)
		{
			var newBuff = target.gameObject.AddComponent<T>();
			newBuff.Run(target);
			target.buffs.Add(newBuff);
		}
		else
			sameBuff.Run(target);
	}
	public void RemoveBuff(Buff buff)
    {
		for (int i = buffs.Count - 1; i >= 0; i--)
        {
			if (buffs[i] == buff)
            {
				buffs.RemoveAt(i);
				return;
            }
        }
    }
	public T SameBuffCheck<T>() where T : Buff
	{
		for (int i = 0; i < buffs.Count; i++)
		{
			if (buffs[i] is T)
			{
				return buffs[i] as T;
			}
		}
		return null;
	}
}
