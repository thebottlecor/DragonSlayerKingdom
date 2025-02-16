using UnityEngine;
using System.Collections;

public class RTSWeaponSimpleRanged : RTSWeapon
{
	public Transform sourceEffectRoot;

	public override bool Aim(RTSUnit target)
	{
		return base.Aim(target);
	}

	public override void Attack(RTSUnit target)
	{
		base.Attack(target);

		if (!unit.Info.ranged)
		{
			AttackApply(target);
		}
	}

	private void AttackApply(RTSUnit target)
	{
		var info = unit.Info;
		if (info.sfx.Length > 0) AudioSource.PlayClipAtPoint(info.sfx[Random.Range(0, info.sfx.Length)], transform.position, info.volume);
		if (info.sourceEffect != null) GameObject.Instantiate(info.sourceEffect, sourceEffectRoot != null ? sourceEffectRoot.position : transform.position, sourceEffectRoot != null ? sourceEffectRoot.rotation : Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up));
		if (info.targetEffect != null) GameObject.Instantiate(info.targetEffect, target.transform.position, Quaternion.LookRotation(transform.position - target.transform.position, Vector3.up));

		target.ApplyDamage(info.Damage, unit.Info.damageType);
	}

    public override void ProjectileHit(Transform target)
    {
        base.ProjectileHit(target);

		if (target != null)
		{
			RTSUnit trueTarget = target.GetComponent<RTSUnit>();
			AttackApply(trueTarget);
		}
	}
}
