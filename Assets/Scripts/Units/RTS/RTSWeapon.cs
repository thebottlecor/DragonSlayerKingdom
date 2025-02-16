using UnityEngine;
using System.Collections;

public class RTSWeapon : MonoBehaviour
{
	protected RTSUnit unit;

	float lastAttackTime = float.NegativeInfinity;

    private void Awake()
    {
		unit = GetComponent<RTSUnit>();
    }

    public virtual bool Aim(RTSUnit target) // 실제 공격 간격은 cooldown에만 영향을 받는다
	{
		return Time.time - lastAttackTime >= unit.Info.Cooldown;
	}

	public bool IsAttacking // 이 시간동안 단순히 이동만 금지하는 효과
	{
		get
		{
			return Time.time - lastAttackTime < unit.Info.attackDuration;
		}
	}

	public bool InRangeOf(Vector3 point)
	{
		return (transform.position - point).sqrMagnitude < unit.Info.Range * unit.Info.Range;
	}

	public virtual void Attack(RTSUnit target)
	{
		lastAttackTime = Time.time;

		var info = unit.Info;
		if (info.ranged)
        {
			ProjectileRTS projectile = Instantiate(info.projectilePrefab, transform.position, Quaternion.identity).GetComponent<ProjectileRTS>();

			projectile.source = this;
			projectile.InitializeProjectile(target.transform, info.projectileMaxMoveSpeed, info.projectileMaxHeight);
			projectile.InitializeAnimationCurves(info.trajectoryAnimationCurve, info.axisCorrectionAnimationCurve, info.projectileSpeedAnimationCurve);
		}
	}

	public virtual void ProjectileHit(Transform target)
    {

    }
}
