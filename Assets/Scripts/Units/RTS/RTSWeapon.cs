using UnityEngine;
using System.Collections;

public class RTSWeapon : MonoBehaviour
{
	public bool ranged; // üũ�� ���, ����ü�� ����� �� �������� ��
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

	public virtual bool Aim(RTSUnit target) // ���� ���� ������ cooldown���� ������ �޴´�
	{
		return Time.time - lastAttackTime >= cooldown;
	}

	public bool isAttacking // �� �ð����� �ܼ��� �̵��� �����ϴ� ȿ��
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
