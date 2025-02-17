using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UnitInfo", menuName = "GameInfos/UnitInfo")]
public class UnitInfo : ScriptableObject
{
	[Header("유닛 정보")]
	public UnitIdx idx;
	public RTSUnit prefab;

	public GameObject deathEffect;
	public UnitType type;
	public float exp = 1f;

	public float detectionRange = 2f;
	[HideInInspector] public float rangeFuzz;
	[HideInInspector] public float detectionRangeSquare;

	[SerializeField] private float maxHealth = 20f;
	public float MaxHealth => maxHealth;
	[SerializeField] private float regenHealth;
	public float RegenHealth => regenHealth;

	[SerializeField] private float maxMana;
	public float MaxMana => maxMana;
	[SerializeField] private float regenMana;
	public float RegenMana => regenMana;

	[SerializeField] private float armor;
	public float Armor => armor;

	public List<Skill> initSkills;

	[SerializeField] private int maxTarget = 1;
	public int MaxTarget => maxTarget;

	[SerializeField] private float moveSpeed;
	public float MoveSpeed => moveSpeed;

	[Header("무기 정보")]
	public bool ranged; // 체크할 경우, 투사체가 닿았을 때 데미지가 들어감
	public DamageType damageType;

	public GameObject projectilePrefab;
	public float projectileMaxMoveSpeed;
	public float projectileMaxHeight;

	public AnimationCurve trajectoryAnimationCurve;
	public AnimationCurve axisCorrectionAnimationCurve;
	public AnimationCurve projectileSpeedAnimationCurve;

	[SerializeField] private float damage;
	public float Damage => damage;

	[SerializeField] private float range;
	public float Range => range;

	[SerializeField] private float cooldown;
	public float Cooldown => cooldown;

	public float attackDuration;
	public bool canMoveWhileAttacking = false;

	public GameObject sourceEffect;
	public GameObject targetEffect;

	public AudioClip[] sfx;
	public float volume = 1f;


	public void CalcSomeValue()
    {
		detectionRangeSquare = detectionRange * detectionRange;
		rangeFuzz = 1.1f * detectionRange;
	}
}

public enum Skill
{
	None = 0,
	Defend = 10,
	DoubleShot = 20,
	Charge = 30,
	Block = 40,
	FireArrow = 50,
	Stomping = 60,
}
public enum DamageType
{
	melee,
	ranged,
	fire,
}

public enum UnitIdx
{
	PlayerWall = 0,
	Footman = 10,
	Bowman = 11,
	Knight = 12,
	Goblin = 100,
	GoblinArcher = 101,
	Orc = 102,
	EnemySpawner1 = 900,
	EnemySpawner2 = 901,
	EnemySpawner3 = 902,
}

public enum UnitType
{
	Infantry,
	Heavy,
	Worker,
	Harvester,
	Wall,

	HarvesterDropoff = 100,
	HarvesterDropoffQueue,
	ResourceCrystal = 200,
}
