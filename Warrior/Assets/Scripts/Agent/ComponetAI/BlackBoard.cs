//
// /**************************************************************************
//
// BlackBoard.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-21
//
// Description:Provide  functions  to connect Oracle
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IActionHandle
{
	void HandleAction (AgentAction _action);
}

[System.Serializable]
public class BlackBoard
{
	private List<AgentAction> _ActiveActions = new List<AgentAction> ();
	private List<IActionHandle> _ActionHandles = new List<IActionHandle> ();
	// 黑板拥有者
	[System.NonSerialized]
	public Agent Owner;
	[System.NonSerialized]
	public GameObject myGameObject;

	//角色是否停止移动
	[System.NonSerialized]
	private bool _Stop = false;

	public bool Stop {
		get{ return _Stop; }
		set {
			_Stop = value;
			//如果是玩家，玩家停止移动
			if (IsPlayer == true) {
				Player.Instance.StopMove (value);
			}
		}
	}


	[System.NonSerialized]
	public bool IsPlayer = false;

	// 最大冲刺速度
	public float MaxSprintSpeed = 8f;
	// 最大奔跑速度
	public float MaxRunSpeed = 4f;
	// 最大战斗移动速度
	public float MaxCombatMoveSpeed = 1;
	// 最大行走速度
	public float MaxWalkSpeed = 1.5f;
	//最大生命值
	public float MaxHealth = 100f;

	//实际最大生命值
	public float RealMaxHealth {
		get {
			//非玩家
			if (IsPlayer == false)
				return MaxHealth;
			//玩家最大生命值，由商店里面玩家生命等级确定
			switch (Game.Instance.HealthLevel) {
			case E_HealthLevel.One:
				return MaxHealth;
			case E_HealthLevel.Two:
				return MaxHealth + 25;
			case E_HealthLevel.Three:
				return MaxHealth + 50;
			}
			return MaxHealth;
		}
	}

	public float MaxKnockdownTime = 4f;

	public float SpeedSmooth = 2.0f;
	public float RotationSmooth = 2.0f;
	public float MoveSpeedModifier = 1;

	[System.NonSerialized]
	public Vector3 MoveDir = Vector3.zero;

	// 运动类型
	[System.NonSerialized]
	public E_MotionType MotionType = E_MotionType.None;
	[System.NonSerialized]
	public E_WeaponState WeaponState = E_WeaponState.NotInHands;
	// 武器状态
	[System.NonSerialized]
	public E_WeaponType WeaponSelected = E_WeaponType.Katana;
	// 武器类型
	[System.NonSerialized]
	public E_WeaponType WeaponToSelect = E_WeaponType.None;

	//武器攻击范围
	public float WeaponRange = 2;

	public float sqrWeaponRange{ get { return WeaponRange * WeaponRange; } }

	//攻击范围
	public float CombatRange = 4;

	public float sqrCombatRange{ get { return CombatRange * CombatRange; } }


	/// <summary>
	/// The desired position.
	/// 目标位置
	/// </summary>
	[System.NonSerialized]
	public Vector3 DesiredPosition;
	/// <summary>
	/// The desired direction.
	/// 目标方向
	/// </summary>
	[System.NonSerialized]
	public Vector3 DesiredDirection;
	[System.NonSerialized]
	public E_AttackType DesiredAttackType;
	[System.NonSerialized]
	public Agent DesiredTarget;
	[System.NonSerialized]
	public AnimAttackData DesiredAttackPhase;
	[System.NonSerialized]
	public string DesiredAnimation;
	[System.NonSerialized]
	public Agent DesiredAttacker;
	//可交互对象
	[System.NonSerialized]
	public InteractionGameObject InteractionObject;
	//交互类型：开，关
	[System.NonSerialized]
	public E_InteractionType Interaction;
	[System.NonSerialized]
	public Agent Attacker;
	[System.NonSerialized]
	public E_WeaponType AttackerWeapon;
	[System.NonSerialized]
	public E_DamageType DamageType;
	[System.NonSerialized]
	public E_LookType LookType;
	[System.NonSerialized]
	public E_MoveType MoveType;
	[System.NonSerialized]
	public float DistanceToTarget;
	[System.NonSerialized]
	public Teleport TeleportDestination;
	[System.NonSerialized]
	public Vector3 Impuls;
	[System.NonSerialized]
	public bool DontUpdate = true;
	[System.NonSerialized]
	public bool ReactOnHits = true;
	[System.NonSerialized]
	public float IdleTimer = 0.0f;

	///////////////// Stats /////////////////////

	/// <summary>
	/// The speed.
	/// 角色移动速度
	/// </summary>
	public float Speed = 0;
	/// <summary>
	/// The health.
	/// 角色当前生命值
	/// </summary>
	public float Health = 0;

	///////////////////////Combat Setting/////////////////
	//AI parameters
	/// <summary>
	/// The dodge.
	/// 躲避
	/// </summary>
	public float Dodge = 0;
	//实际值
	public float DodgeMin = 0;
	// 0 不攻击
	public float DodgeMax = 0;
	//100 攻击
	public float DodgeModificator = 0;
	//改变

	public void SetDodge (float value)
	{
		Dodge = Mathf.Clamp (value, DodgeMin, DodgeMax);
	}

	/// <summary>
	/// The berserk.
	/// 狂暴
	/// </summary>
	public float Berserk = 0;
	public float BerserkMin = 0;
	public float BerserkMax = 0;
	public float BerserkModificator = 0;

	public void SetBerserk (float value)
	{
		Berserk = Mathf.Clamp (value, BerserkMin, BerserkMax);
	}

	/// <summary>
	/// The fear.
	/// 恐惧
	/// </summary>
	public float Fear = 0;
	public float FearMin = 0;
	public float FearMax = 0;
	public float FearModificator = 0;

	public void SetFear (float value)
	{
		Fear = Mathf.Clamp (value, FearMin, FearMax);
	}

	/// <summary>
	/// The rage.
	/// 愤怒
	/// </summary>
	public float Rage = 0;
	public float RageMin = 0;
	public float RageMax = 0;
	public float RageModificator = 0;

	public void SetRage (float value)
	{
		Rage = Mathf.Clamp (value, RageMin, RageMax);
	}

	public float RageInjuryModificator = 0;
	//每次伤害改变Rage
	public float DodgeInjuryModificator = 0;
	//每次伤害改变Dodge
	public float FearInjuryModificator = 0;
	public float BerserkInjuryModificator = 0;

	public float DodgeAttackModificator = 0;
	public float FearAttackModificator = 0;
	public float BerserkAttackModificator = 0;

	public float RageBlockModificator = 0;
	public float FearBlockModificator = 0;
	public float BerserkBlockModificator = 0;

	public float RotationSmoothInMove = 8.0f;
	public float RollDistance = 4.0f;

	public float DontAttackTimer = 2.0f;

	// Damage settings
	public bool KnockDown = true;
	public bool KnockDownDamageDeadly = true;
	//霸体
	public bool Invulnerable = false;
	public bool CriticalAllowed = true;
	public bool DamageOnlyFromBack = false;
	public float CriticalHitModifier = 1;

	///////////////// GOAP Settings /////////////////////////////
	public float GOAP_AlertRelevancy = 0.8f;
	public float GOAP_CalmRelevancy = 0.2f;
	public float GOAP_BlockRelevancy = 0.7f;
	public float GOAP_DodgeRelevancy = 0.9f;
	public float GOAP_GoToRelevancy = 0.5f;
	public float GOAP_CombatMoveBackwardRelevancy = 0.7f;
	public float GOAP_CombatMoveForwardRelevancy = 0.75f;
	public float GOAP_CombatMoveLeftRelevancy = 0.6f;
	public float GOAP_CombatMoveRightRelevancy = 0.6f;
	public float GOAP_LookAtTargetRelevancy = 0.7f;
	public float GOAP_KillTargetRelevancy = 0.8f;
	public float GOAP_PlayAnimRelevancy = 0.95f;
	public float GOAP_UseWorlObjectRelevancy = 0.9f;
	public float GOAP_ReactToDamageRelevancy = 1.0f;
	public float GOAP_IdleActionRelevancy = 0.4f;
	public float GOAP_TeleportRelevancy = 0.9f;

	public float GOAP_AlertDelay = 2.0f;
	public float GOAP_CalmDelay = 2.2f;
	public float GOAP_BlockDelay = 2.7f;
	public float GOAP_DodgeDelay = 5.0f;
	public float GOAP_GoToDelay = 0.5f;
	public float GOAP_CombatMoveDelay = 3.5f;
	public float GOAP_CombatMoveLeftDelay = 3.6f;
	public float GOAP_CombatMoveRightDelay = 3.6f;
	public float GOAP_LookAtTargetDelay = 0.4f;
	public float GOAP_KillTargetDelay = 2.8f;
	public float GOAP_PlayAnimDelay = 0.0f;
	public float GOAP_UseWorlObjectDelay = 5.0f;
	public float GOAP_CombatMoveBackwardDelay = 3.5f;
	public float GOAP_CombatMoveForwardDelay = 3.5f;
	public float GOAP_IdleActionDelay = 10;
	public float GOAP_TeleportDelay = 4;


	


	public void Reset ()
	{
		_ActiveActions.Clear ();

		Stop = false;
		MotionType = E_MotionType.None;
		WeaponState = E_WeaponState.NotInHands;
		WeaponToSelect = E_WeaponType.None;

		Speed = 0;

		Health = RealMaxHealth;

		Rage = RageMin;
		Dodge = DodgeMin;
		Fear = FearMin;
		IdleTimer = 0;

		MoveDir = Vector3.zero;

		DesiredPosition = Vector3.zero;
		DesiredDirection = Vector3.zero;

		InteractionObject = null;
		Interaction = E_InteractionType.None;

		DesiredAnimation = "";

		DesiredTarget = null;
		DesiredAttackType = E_AttackType.None;

		DontUpdate = false;
	}

	public void AddAction (AgentAction _action)
	{
		IdleTimer = 0;
		//Debug.Log (_action.ToString ());
		_ActiveActions.Add (_action);

		// todo: call HandleAction...
		for (int i = 0; i < _ActionHandles.Count; i++)
			_ActionHandles [i].HandleAction (_action);
	}

	public AgentAction GetAction (int index)
	{
		return _ActiveActions [index];
	}

	public AgentActionAttack ActionGetAttackAction()
	{
		for (int i = 0; i < _ActiveActions.Count; i++)
			if (_ActiveActions[i] is AgentActionAttack)
				return _ActiveActions[i] as AgentActionAttack;

		return null;
	}

	public int ActionCount {
		get { return _ActiveActions.Count; }
	}

	public void AddActionHandle (IActionHandle _handle)
	{
		for (int i = 0; i < _ActionHandles.Count; i++)
			if (_ActionHandles [i] == _handle)
				return;
		_ActionHandles.Add (_handle);
	}

	public void RemoveActionHandle (IActionHandle _handle)
	{
		_ActionHandles.Remove (_handle);
	}

	public void Update ()
	{
		IdleTimer += Time.deltaTime;

		for (int i = 0; i < _ActiveActions.Count; i++) {
			if (_ActiveActions [i].IsActive ())
				continue;
			// todo: Action Done...
			ActionDone (_ActiveActions [i]);
			_ActiveActions.RemoveAt (i);

			return;
		}

		if (DesiredTarget && DesiredTarget.IsAlive == false)
			DesiredTarget = null;

		if (DesiredTarget == null)
			Owner.WorldState.SetWSProperty (E_PropKey.E_IN_WEAPONS_RANGE, true);
		else if ((DesiredTarget.Position - Owner.Position).magnitude <= (WeaponRange + 0.2f))
			Owner.WorldState.SetWSProperty (E_PropKey.E_IN_WEAPONS_RANGE, true);
		else
			Owner.WorldState.SetWSProperty (E_PropKey.E_IN_WEAPONS_RANGE, false);
	}

	private void ActionDone (AgentAction _action)
	{
		// todo: ....

		AgentActionFactory.Return (_action);
	}

	//////////////// ORDERS /////////////////////////

	public bool IsOrderAddPossible (AgentOrder.E_OrderType orderType)
	{
		AgentOrder.E_OrderType currentOrder = Owner.WorldState.GetWSProperty (E_PropKey.E_ORDER).GetOrder ();

		if (orderType == AgentOrder.E_OrderType.E_DODGE && currentOrder != AgentOrder.E_OrderType.E_DODGE && currentOrder != AgentOrder.E_OrderType.E_USE)
			return true;
		else if (currentOrder != AgentOrder.E_OrderType.E_ATTACK && currentOrder != AgentOrder.E_OrderType.E_DODGE && currentOrder != AgentOrder.E_OrderType.E_USE)
			return true;
		else
			return false;
	}


	public void AddOrder (AgentOrder order)
	{

		if (IsOrderAddPossible (order.Type)) {
			Owner.WorldState.SetWSProperty (E_PropKey.E_ORDER, order.Type);
			switch (order.Type) {
			case AgentOrder.E_OrderType.E_STOPMOVE:
				Owner.WorldState.SetWSProperty (E_PropKey.E_AT_TARGET_POS, true);
				DesiredPosition = Owner.Position;
				break;
			case AgentOrder.E_OrderType.E_GOTO:
				Owner.WorldState.SetWSProperty (E_PropKey.E_AT_TARGET_POS, false);
				DesiredPosition = order.Position;
				DesiredDirection = order.Direction;
				MoveSpeedModifier = order.MoveSpeedModifier;
				break;
			case AgentOrder.E_OrderType.E_DODGE:
				DesiredDirection = order.Direction;
				break;
			case AgentOrder.E_OrderType.E_USE:
				Owner.WorldState.SetWSProperty (E_PropKey.E_USE_WORLD_OBJECT, true);

				if ((order.Position - Owner.Position).sqrMagnitude <= 1)
					Owner.WorldState.SetWSProperty (E_PropKey.E_AT_TARGET_POS, true);
				else
					Owner.WorldState.SetWSProperty (E_PropKey.E_AT_TARGET_POS, false);
				DesiredPosition = order.Position;
				InteractionObject = order.InteractionObject;
				Interaction = order.Interaction;
				break;
			case AgentOrder.E_OrderType.E_ATTACK:
				if (order.Target == null || (order.Target.Position - Owner.Position).magnitude <= (WeaponRange + 0.2f))
					Owner.WorldState.SetWSProperty (E_PropKey.E_IN_WEAPONS_RANGE, true);
				else
					Owner.WorldState.SetWSProperty (E_PropKey.E_IN_WEAPONS_RANGE, false);

				DesiredAttackType = order.AttackType;
				DesiredTarget = order.Target;
				DesiredDirection = order.Direction;
				DesiredAttackPhase = order.AnimAttackData;
				break;
			}
		} else if (order.Type == AgentOrder.E_OrderType.E_ATTACK) {
		}

		AgentOrderFactory.Return (order);
	}
	public void UpdateCombatSetting()
	{
		if (Game.Instance.GameDifficulty == E_GameDifficulty.Hard && IsPlayer == false)
		{
			SetRage(Rage + RageModificator * 1.2f * Time.fixedDeltaTime);
			SetBerserk(Berserk + BerserkModificator * 1.2f * Time.fixedDeltaTime);
		}
		else if (Game.Instance.GameDifficulty == E_GameDifficulty.Easy && IsPlayer == false)
		{
			SetRage(Rage + RageModificator * 0.8f * Time.fixedDeltaTime);
			SetBerserk(Berserk + BerserkModificator * 0.8f * Time.fixedDeltaTime);
		}
		else
		{
			SetRage(Rage + RageModificator * Time.fixedDeltaTime);
			SetBerserk(Berserk + BerserkModificator * Time.fixedDeltaTime);
		}


		if (DesiredTarget && Owner.WorldState.GetWSProperty(E_PropKey.E_AHEAD_OF_ENEMY).GetBool())
			SetFear(Fear + FearModificator * Time.fixedDeltaTime);
		else
			SetFear(Fear - FearModificator * Time.fixedDeltaTime);

		if (Owner.WorldState.GetWSProperty(E_PropKey.E_IN_BLOCK).GetBool() != true)
			SetDodge(Dodge + Owner.BlackBoard.DodgeModificator * Time.fixedDeltaTime);
	}

	public void UpdateCombatSetting(AgentAction a)
	{
		if (a is AgentActionDamageBlocked)
		{
			if ((a as AgentActionDamageBlocked).BreakBlock)
			{
				SetFear(Fear + FearInjuryModificator * 0.5f);
				SetRage(Rage + RageInjuryModificator * 0.5f);
			}
			else
			{
				SetFear(Fear + FearBlockModificator);
				SetRage(Rage + RageBlockModificator);
				SetBerserk(Berserk + BerserkBlockModificator);

			}
		}
		else if (a is AgentActionInjury)
		{
			SetFear(Fear + FearInjuryModificator);
			SetRage(Rage + RageInjuryModificator);
			SetDodge(Dodge + DodgeInjuryModificator);
			SetBerserk(Berserk + BerserkInjuryModificator);
		}
		else if (a is AgentActionAttackWhirl || a is AgentActionAttackRoll)
		{
			SetBerserk(BerserkMin);
		}
		else if (a is AgentActionAttack)
		{
			SetRage(RageMin);// reset

			SetDodge(Dodge + DodgeAttackModificator);
			SetFear(Fear + FearAttackModificator);
			SetBerserk(Berserk + BerserkAttackModificator);
		}
		else if (a is AgentActionBlock) // reset
		{
			SetDodge(DodgeMin);
		}
		else if (a is AgentActioCombatMove) //reset
		{
			if ((a as AgentActioCombatMove).MoveType == E_MoveType.Backward)
				SetFear(Owner.BlackBoard.FearMin);
		}
	}
}

