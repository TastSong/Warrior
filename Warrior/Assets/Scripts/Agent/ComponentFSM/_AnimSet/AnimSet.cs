//
// /**************************************************************************
//
// AnimSet.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-19
//
// Description: AnimSet提供获取各种动画名称接口，提供动画信息接口
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;

public abstract class AnimSet : MonoBehaviour
{
	/// <summary>
	/// Gets the idle animation.
	/// 获取Idle动画名字，普通Idle，拿各种武器的Idle
	/// </summary>
	/// <returns>The idle animation name.</returns>

	/// <summary>
	/// Gets the move animation.
	/// 获取Move动画名字，普通移动， 拿各种武器的移动动画
	/// </summary>
	/// <returns>The move animation name.</returns>

	/// <summary>
	/// Gets the show weapon animation.
	/// 获取拔除武器的动画名字
	/// </summary>
	/// <returns>The show weapon animation name.</returns>

	/// <summary>
	/// Gets the hide weapon animation.
	/// 获取放回武器的动画名字
	/// </summary>
	/// <returns>The hide weapon animation name.</returns>

	public abstract string GetIdleAnim(E_WeaponType weapon, E_WeaponState weaponState);

	public abstract string GetIdleActionAnim(E_WeaponType weapon, E_WeaponState weaponState);

	public abstract string GetMoveAnim(E_MotionType motion, E_MoveType move, E_WeaponType weapon, E_WeaponState weaponState);

	public abstract string GetRotateAnim(E_MotionType motionType, E_RotationType rotationType);

	public abstract string GetRollAnim(E_WeaponType weapon, E_WeaponState weaponState);

	public abstract string GetBlockAnim(E_BlockState block, E_WeaponType weapon);

	public abstract string GetKnockdowAnim(E_KnockdownState block, E_WeaponType weapon);

	public abstract string GetShowWeaponAnim(E_WeaponType weapon);

	public abstract string GetHideWeaponAnim(E_WeaponType weapon);

	public abstract string GetUseAnim(E_InteractionObjects objectType, E_InteractionType interaction);

	public abstract AnimAttackData GetFirstAttackAnim(E_WeaponType weapon, E_AttackType attackType);
	public abstract AnimAttackData GetChainedAttackAnim(AnimAttackData parent, E_AttackType attackType);
	public virtual AnimAttackData GetRollAttackAnim() {return null;}
	public virtual AnimAttackData GetWhirlAttackAnim() { return null; }
	public virtual string GetInjuryPhaseAnim(int phase) { return null; }

	public abstract string GetInjuryAnim(E_WeaponType weapon, E_DamageType type);

	public abstract string GetDeathAnim(E_WeaponType weapon, E_DamageType type);
}

public class AnimAttackData : System.Object
{
	// 动画名字
	public string AnimName;
	// 下次攻击数据
	public AnimAttackData[] NextAttacks = new AnimAttackData[(int)E_AttackType.Max];

	// 移动距离
	public float MoveDistance;// best attack distance

	//timers
	public float AttackMoveStartTime;
	public float AttackMoveEndTime;
	public float AttackEndTime;
	public float ZanshinEndTime;

	// hit parameters
	public float HitTime;
	public float HitDamage;
	public float HitAngle;
	public float HitMomentum;
	public E_CriticalHitType HitCriticalType;
	public bool HitAreaKnockdown;
	public bool BreakBlock;
	public bool UseImpuls;
	public float CriticalModificator = 1;
	public bool SloMotion;

	// trail
	public GameObject Trail;
	public Transform Parent;
	public GameObject Dust;
	public Animation AnimationDust;
	public Animation Animation;
	public Material Material;
	public Material MaterialDust;

	//meni se v realtime, pro playera
	public bool FirstAttackInCombo = true;
	public bool LastAttackInCombo = false;
	public int ComboIndex;
	public bool FullCombo;
	public int ComboStep;


	//effects


	public AnimAttackData(string animName, GameObject trail, float moveDistance, float hitTime, float attackEndTime, float hitDamage, float hitAngle, float hitMomentum,
		E_CriticalHitType criticalType, bool areaKnockDown )
	{
		AnimName = animName;
		Trail = trail;

		if (Trail)
		{
			Parent = Trail.transform.parent;

			if(Trail.transform.Find("dust"))
			{
				Dust = Trail.transform.Find("dust").gameObject;
				AnimationDust = Dust.GetComponent<Animation>();
				MaterialDust = Dust.GetComponent<Renderer>().material;
			}

			Animation = Trail.transform.parent.GetComponent<Animation>();
			if (Trail.GetComponentInChildren(typeof(Renderer)))
				Material = (Trail.GetComponentInChildren(typeof(Renderer)) as Renderer).material;
			else if (Trail.GetComponentInChildren(typeof(SkinnedMeshRenderer)))
				Material = (Trail.GetComponentInChildren(typeof(SkinnedMeshRenderer)) as Renderer).material;
			else
				Material = null;

			if (Material == null)
				Debug.LogError("Trail - no Material");
		}
		else
		{
			Animation = null;
			Material = null;
		}

		MoveDistance = moveDistance;

		AttackEndTime = attackEndTime;
		AttackMoveStartTime = 0;
		AttackMoveEndTime = AttackEndTime * 0.7f;

		HitTime = hitTime;
		HitDamage = hitDamage;
		HitAngle = hitAngle;
		HitMomentum = hitMomentum;
		HitCriticalType = criticalType;
		HitAreaKnockdown = areaKnockDown;
		BreakBlock = false;
		UseImpuls = false;
		CriticalModificator = 1;

	}

	public AnimAttackData(string animName, GameObject trail, float moveDistance, float hitTime, float moveStartTime, float moveEndTime, float attackEndTime, float hitDamage, float hitAngle, float hitMomentum,
		E_CriticalHitType criticalType, float criticalMod, bool areaKnockDown, bool breakBlock, bool useImpuls, bool sloMotion)
	{
		AnimName = animName;
		Trail = trail;

		if (Trail)
		{
			Parent = Trail.transform.parent;

			if (Trail.transform.Find("dust"))
			{
				Dust = Trail.transform.Find("dust").gameObject;
				AnimationDust = Dust.GetComponent<Animation>();
				MaterialDust = Dust.GetComponent<Renderer>().material;
			}

			Animation = Trail.transform.parent.GetComponent<Animation>();
			if (Trail.GetComponent(typeof(Renderer)))
				Material = (Trail.GetComponent(typeof(Renderer)) as Renderer).material;
			else if (Trail.GetComponentInChildren(typeof(Renderer)))
				Material = (Trail.GetComponentInChildren(typeof(Renderer)) as Renderer).material;
			else if (Trail.GetComponentInChildren(typeof(SkinnedMeshRenderer)))
				Material = (Trail.GetComponentInChildren(typeof(SkinnedMeshRenderer)) as Renderer).material;
			else
				Material = null;

			if (Material == null)
				Debug.LogError("Trail - no Material");
		}
		else
		{
			Animation = null;
			Material = null;
		}

		MoveDistance = moveDistance;

		AttackMoveStartTime = moveStartTime;
		AttackMoveEndTime = moveEndTime;
		AttackEndTime = attackEndTime;

		HitTime = hitTime;
		HitDamage = hitDamage;
		HitAngle = hitAngle;
		HitMomentum = hitMomentum;
		HitCriticalType = criticalType;
		HitAreaKnockdown = areaKnockDown;
		BreakBlock = breakBlock;
		UseImpuls = useImpuls;
		CriticalModificator = criticalMod;
		SloMotion = sloMotion;
	}


	override public string ToString() { return base.ToString() + ": " + AnimName; }
}

