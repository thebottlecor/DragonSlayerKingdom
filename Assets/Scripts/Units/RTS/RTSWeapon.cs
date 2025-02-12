using UnityEngine;
using System.Collections;

public class RTSWeapon : MonoBehaviour
{
	public bool ranged; // 체크할 경우, 투사체가 닿았을 때 데미지가 들어감
	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private float projectileMaxMoveSpeed;
	[SerializeField] private float projectileMaxHeight;

	[SerializeField] private AnimationCurve trajectoryAnimationCurve;
	[SerializeField] private AnimationCurve axisCorrectionAnimationCurve;
	[SerializeField] private AnimationCurve projectileSpeedAnimationCurve;

	[Space(20f)]
	public float range;
	public float cooldown;
	public float attackDuration;
	public bool canMoveWhileAttacking = false;

	float lastAttackTime = float.NegativeInfinity;

	public virtual bool Aim(RTSUnit target) // 실제 공격 간격은 cooldown에만 영향을 받는다
	{
		return Time.time - lastAttackTime >= cooldown;
	}

	public bool isAttacking // 이 시간동안 단순히 이동만 금지하는 효과
	{
		get
		{
			return Time.time - lastAttackTime < attackDuration;
		}
	}

	public bool InRangeOf(Vector3 point)
	{
		return (transform.position - point).sqrMagnitude < range * range;
	}

	public virtual void Attack(RTSUnit target)
	{
		lastAttackTime = Time.time;

		if (ranged)
        {
			ProjectileRTS projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<ProjectileRTS>();

			projectile.source = this;
			projectile.InitializeProjectile(target.transform, projectileMaxMoveSpeed, projectileMaxHeight);
			projectile.InitializeAnimationCurves(trajectoryAnimationCurve, axisCorrectionAnimationCurve, projectileSpeedAnimationCurve);
		}
	}

	public virtual void ProjectileHit(Transform target)
    {

    }
}
