//
// /**************************************************************************
//
// AnimSetPlayer.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-19
//
// Description:Provide  functions  to connect Oracle
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;

public class AnimSetPlayer : AnimSet
{
	public AnimAttackData[] AttackData = new AnimAttackData[24];
	private AnimAttackData AttackKnockdown;

	private GameObject Trails;


	void Awake()
	{
		Animation anims = GetComponent<Animation>();

		anims.wrapMode = WrapMode.Once;

		anims["idle"].layer = 0;
		anims["idleSword"].layer = 0;
		anims["run"].layer = 0;
		anims["runSword"].layer = 0;
		anims["walk"].layer = 0;
		anims["walkSword"].layer = 0;

		anims["deathBack"].layer = 2;
		anims["deathFront"].layer = 2;


		anims["injuryFrontSword"].layer = 1;
		anims["injuryFrontSword"].speed = 0.9f;
		anims["injuryBackSword"].layer = 1;
		anims["injuryBackSword"].speed = 0.9f;

		anims["evadeSword"].layer = 1;

		anims["showSword"].layer = 0;
		anims["hideSword"].layer = 0;
		anims["showSwordRun"].layer = 0;
		anims["hidSwordRun"].layer = 0;
		//  anims["showSwordRun"].blendMode = AnimationBlendMode.Additive;
		//  anims["hidSwordRun"].blendMode = AnimationBlendMode.Additive;

		anims["useLever"].layer = 0;
		// combo XXXXXX
		anims["attackX"].speed = 0.9f;
		anims["attackXX"].speed = 0.8f;
		anims["attackXXX"].speed = 0.8f;
		anims["attackXXXX"].speed = 0.8f;
		anims["attackXXXXX"].speed = 0.8f;

		anims["attackX"].layer = 1;
		anims["attackXX"].layer = 1;
		anims["attackXXX"].layer = 1;
		anims["attackXXXX"].layer = 1;
		anims["attackXXXXX"].layer = 1;
		// combo OOOXX
		anims["attackO"].speed = 1.2f;
		anims["attackOO"].speed = 1.5f;
		anims["attackOOO"].speed = 1.1f;
		anims["attackOOOX"].speed = 1;
		anims["attackOOOXX"].speed = 1.4f;

		anims["attackO"].layer = 1;
		anims["attackOO"].layer = 1;
		anims["attackOOO"].layer = 1;
		anims["attackOOOX"].layer = 1;
		anims["attackOOOXX"].layer = 1;
		// COMBO X00XX
		anims["attackXO"].speed = 1;
		anims["attackXOO"].speed = 1.2f;
		anims["attackXOOX"].speed = 1.2f;
		anims["attackXOOXX"].speed = 1.2f;

		anims["attackXO"].layer = 1;
		anims["attackXOO"].layer = 1;
		anims["attackXOOX"].layer = 1;
		anims["attackXOOXX"].layer = 1;

		// COMBO XX0XX
		anims["attackXXO"].speed = 1;
		anims["attackXXOX"].speed = 1.2f;
		anims["attackXXOXX"].speed = 1.3f;

		anims["attackXXO"].layer = 1;
		anims["attackXXOX"].layer = 1;
		anims["attackXXOXX"].layer = 1;

		// Combo OOXOO
		anims["attackOOX"].speed = 1;
		anims["attackOOXO"].speed = 1;
		anims["attackOOXOO"].speed = 1.3f;

		anims["attackOOX"].layer = 1;
		anims["attackOOXO"].layer = 1;
		anims["attackOOXOO"].layer = 1;

		// COMBO OXOOO
		anims["attackOX"].speed = 1.1f;
		anims["attackOXO"].speed = 1.2f;
		anims["attackOXOX"].speed = 1;
		anims["attackOXOXO"].speed = 1;

		anims["attackOX"].layer = 1;
		anims["attackOXO"].layer = 1;
		anims["attackOXOX"].layer = 1;
		anims["attackOXOXO"].layer = 1;


		Trails = Instantiate(Resources.Load("trails_combo01")) as GameObject;
		Trails.transform.parent = AutoElementManager.Instance.transform;
		Trails.SetActive(false);

		AttackKnockdown = new AnimAttackData("attackKnockdown", null, 1.5f, 0.65f, 0.2f, 0.6f, 1.0f, 20, 0, 0, E_CriticalHitType.None, 0, false, false, false, true);

		// AAAAA, critical later
		AttackData[0] = new AnimAttackData("attackX", Trails.transform.Find("trail_X").gameObject, 0.6f, 0.23f, 0.05f, 0.366f, 0.366f, 3, 15, 0.6f, E_CriticalHitType.Vertical, 0.2f, false, false, false, false);
		AttackData[1] = new AnimAttackData("attackXX", Trails.transform.Find("trail_XX").gameObject, 0.6f, 0.22f, 0.15f, 0.35f, 0.4f, 5, 15, 0.6f, E_CriticalHitType.Vertical, 0.25f, false, false, false, false);
		/*1*/   AttackData[2] = new AnimAttackData("attackXXX", Trails.transform.Find("trail_XXX").gameObject, 0.7f, 0.25f, 0.20f, 0.30f, 0.366f, 10, 45, 0.75f, E_CriticalHitType.Horizontal, 0.3f, false, false, false, false);
		AttackData[3] = new AnimAttackData("attackXXXX", Trails.transform.Find("trail_XXXX").gameObject, 0.8f, 0.28f, 0.22f, 0.35f, 0.366f, 12, 90, 0.8f, E_CriticalHitType.Horizontal, 0.5f, false, false, true, false);
		AttackData[4] = new AnimAttackData("attackXXXXX", Trails.transform.Find("trail_XXXXX").gameObject, 0.8f,0.3f, 0.15f, 0.35f, 0.366f, 16, 45, 1.5f, E_CriticalHitType.Vertical, 0.6f, false, false, true, true);
		// ANTI BLOCK , no critical
		AttackData[5] = new AnimAttackData("attackO", Trails.transform.Find("trail_O").gameObject, 0.7f, 0.38f, 0.30f, 0.40f, 0.45f, 6, 45, 0.8f, E_CriticalHitType.Horizontal, 0.1f, false, true, false, false);
		AttackData[6] = new AnimAttackData("attackOO", Trails.transform.Find("trail_OO").gameObject, 0.7f, 0.34f, 0.10f, 0.35f, 0.4f, 10, 45, 0.8f, E_CriticalHitType.Vertical, 0.15f, false, true, false, false);
		/*2*/   AttackData[7] = new AnimAttackData("attackOOO", Trails.transform.Find("trail_OOO").gameObject, 1.0f, 0.5f, 0.36f, 0.55f, 0.6f, 15, 15, 1.0f, E_CriticalHitType.None, 0, false, true, false, false);
		AttackData[8] = new AnimAttackData("attackOOOX", Trails.transform.Find("trail_OOOX").gameObject, 1.0f, 0.45f, 0.15f, 0.45f, 0.533f, 20, 45, 0.5f, E_CriticalHitType.Vertical, 0.8f, false, true, false, false);
		AttackData[9] = new AnimAttackData("attackOOOXX", Trails.transform.Find("trail_OOOXX").gameObject, 1.0f, 0.51f, 0.15f, 0.6f, 0.7f, 25, 20, 1.8f, E_CriticalHitType.Vertical, 1, false, true, true, true);
		// 
		AttackData[10] = new AnimAttackData("attackXO", Trails.transform.Find("trail_XO").gameObject, 0.85f, 0.45f, 0.10f, 0.8f, 0.833f, 15, 25, 0.8f, E_CriticalHitType.None, 0, false, false, false, false);
		/*4*/   AttackData[11] = new AnimAttackData("attackXOO", Trails.transform.Find("trail_XOO").gameObject, 0.8f, 0.45f, 0.20f, 0.50f, 0.55f, 20, 15, 0.8f, E_CriticalHitType.None, 0, false, false, false, false);
		AttackData[12] = new AnimAttackData("attackXOOX", Trails.transform.Find("trail_XOOX").gameObject, 0.7f, 0.4f, 0.3f, 0.7f, 0.8f, 20, 180, 1, E_CriticalHitType.Vertical, 0.8f, false, false, true, false);
		AttackData[13] = new AnimAttackData("attackXOOXX", Trails.transform.Find("trail_XOOXX").gameObject, 1.5f, 0.35f, 0.10f, 0.30f, 0.48f, 25, 20, 1.5f, E_CriticalHitType.Vertical, 1.0f, false, false, true, true);
		//KNOCK
		AttackData[14] = new AnimAttackData("attackXXO", Trails.transform.Find("trail_XXO").gameObject, 0.7f, 0.30f, 0.15f, 0.4f, 0.6f, 15, 90, 1.0f, E_CriticalHitType.None, 0, true, false, true, true);
		/*5*/   AttackData[15] = new AnimAttackData("attackXXOX", Trails.transform.Find("trail_XXOX").gameObject, 1.0f, 0.41f, 0.11f, 0.55f, 0.60f, 20, 15, 0.7f, E_CriticalHitType.Vertical, 1, false, false, true, false);
		AttackData[16] = new AnimAttackData("attackXXOXX", Trails.transform.Find("trail_XXOXX").gameObject, 1.0f, 0.5f, 0.1f, 0.4f, 0.6f, 20, 180, 1.2f, E_CriticalHitType.None, 0, true, false, true, true);
		//CRITICAL
		AttackData[17] = new AnimAttackData("attackOOX", Trails.transform.Find("trail_OOX").gameObject, 0.8f, 0.45f, 0.25f, 0.5f, 0.66f, 15, 25, 0.7f, E_CriticalHitType.Vertical, 1.2f, false, false, true, true);
		/*3*/   AttackData[18] = new AnimAttackData("attackOOXO", Trails.transform.Find("trail_OOXX").gameObject, 0.7f, 0.6f, 0.2f, 0.6f, 0.7f, 20, 45, 0.7f, E_CriticalHitType.Vertical, 1.4f, false, false, false, false);
		AttackData[19] = new AnimAttackData("attackOOXOO", Trails.transform.Find("trail_OOXXX").gameObject, 1.0f, 0.45f, 0.05f, 0.65f, 1.03f, 25, 30, 1.5f, E_CriticalHitType.Vertical, 1.6f, false, false, true, true);
		//area dmage 
		AttackData[20] = new AnimAttackData("attackOX", Trails.transform.Find("trail_OX").gameObject, 0.7f, 0.25f, 0.15f, 0.44f, 0.45f, 20, 25, 0.8f, E_CriticalHitType.Vertical, 0.4f, false, false, true, true);
		/*6*/   AttackData[21] = new AnimAttackData("attackOXO", Trails.transform.Find("trail_OXO").gameObject, 1.0f, 0.35f, 0.25f, 0.4f, 0.55f, 20, 90, 1, E_CriticalHitType.Horizontal, 0.5f, false, false, true, false);
		AttackData[22] = new AnimAttackData("attackOXOX", Trails.transform.Find("trail_OXOO").gameObject, 1.0f, 0.35f, 0.15f, 0.3f, 0.5f, 20, 180, 1.2f, E_CriticalHitType.Horizontal, .7f, false, false, true, false);
		AttackData[23] = new AnimAttackData("attackOXOXO", Trails.transform.Find("trail_OXOOO").gameObject, 1.0f, 0.35f, 0.15f, 0.5f, 1.1f, 25, 180, 1.8f, E_CriticalHitType.Horizontal, 0.9f, false, false, true, true);
	
		SkillData.Init(this);
	}

	public override string GetBlockAnim(E_BlockState state, E_WeaponType weapon)   {   return null;  }


	public override string GetIdleAnim(E_WeaponType weapon, E_WeaponState weaponState)
	{
		if (weaponState == E_WeaponState.NotInHands)
			return "idle";

		return "idleSword";
	}

	public override string GetIdleActionAnim(E_WeaponType weapon, E_WeaponState weaponState)
	{
		return "idle";
	}

	public override string GetMoveAnim(E_MotionType motion, E_MoveType move, E_WeaponType weapon, E_WeaponState weaponState)
	{
		if (weaponState == E_WeaponState.NotInHands)
		{
			if (motion != E_MotionType.Walk)
				return "run";
			else
				return "walk";
		}

		if (motion != E_MotionType.Walk)
			return "runSword";

		return "walkSword";
	}

	public override string GetRotateAnim(E_MotionType motionType, E_RotationType rotationType)
	{
		return null;
	}


	public override string GetRollAnim(E_WeaponType weapon, E_WeaponState weaponState)
	{
		return "evadeSword";
	}

	public override string GetShowWeaponAnim(E_WeaponType weapon)
	{
		return  "showSwordRun";
	}

	public override string GetHideWeaponAnim(E_WeaponType weapon)
	{
		return "hidSwordRun";
	}

	public override string GetUseAnim(E_InteractionObjects objectType, E_InteractionType interaction)
	{

		if (objectType == E_InteractionObjects.UseLever)
			return "useLever";

		if (objectType == E_InteractionObjects.Trigger)
			return "idle";

		if (objectType == E_InteractionObjects.UseExperience)
			return "attackJump"; 

		return "idle";
	}

	public AnimAttackData GetFatalityAttack()
	{
		return AttackKnockdown;
	}

	public override AnimAttackData GetFirstAttackAnim(E_WeaponType weapon, E_AttackType attackType)
	{
		if (attackType == E_AttackType.Fatality)
			return AttackKnockdown;

		return null;
	}

	public override AnimAttackData GetChainedAttackAnim(AnimAttackData parent, E_AttackType attackType)
	{
		return null;
	}

	public override string GetInjuryAnim(E_WeaponType weapon, E_DamageType type)
	{
		if (type == E_DamageType.Back)
			return "injuryBackSword";

		return "injuryFrontSword";
	}


	public override string GetDeathAnim(E_WeaponType weapon, E_DamageType type)
	{
		if(type == E_DamageType.Back)
			return "deathBack";

		return "deathFront";
	}

	public override string GetKnockdowAnim(E_KnockdownState block, E_WeaponType weapon)
	{
		return null;
	}

}
