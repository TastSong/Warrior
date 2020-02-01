//
// /**************************************************************************
//
// AnimFSMPlayer.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-17
//
// Description:主角玩家状态机
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;

public class AnimFSMPlayer : AnimFSM
{
	/// <summary>
	/// 该状态机所有状态枚举
	/// </summary>
	enum E_AnimState
	{
		E_IDLE,
		E_GOTO,
		E_MOVE,
		E_ATTACK_MELEE,
		E_ROLL,
		E_USE_LEVER,
		Teleport,
		E_INJURY,
		E_DEATH,
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="AnimFSMPlayer"/> class.
	/// </summary>
	/// <param name="_anims">_anims.</param>
	/// <param name="_owner">_owner.</param>
	public AnimFSMPlayer (Animation _anims, Agent _owner) : base (_anims, _owner)
	{

	}

	/// <summary>
	/// Initialize this instance.
	/// </summary>
	public override void Initialize ()
	{
		// todo: add Anim State
		// 添加角色状态到列表
		AnimStates.Add (new AnimStateIdle (AnimEngine, Owner)); //E_MyAnimState.E_IDLE
		AnimStates.Add (new AnimStateGoTo (AnimEngine, Owner)); //E_MyAnimState.E_GOTO
		AnimStates.Add (new AnimStateMove (AnimEngine, Owner)); //E_MyAnimState.E_MOVE
		AnimStates.Add (new AnimStateAttackMelee (AnimEngine, Owner)); //E_MyAnimState.E_ATTACK_MELEE
		AnimStates.Add (new AnimStateRoll (AnimEngine, Owner)); //E_MyAnimState.E_ROLL
		AnimStates.Add (new AnimStateUseLever (AnimEngine, Owner)); //use lever
		AnimStates.Add (new AnimStateTeleport (AnimEngine, Owner)); // teleport
		AnimStates.Add (new AnimStateInjury (AnimEngine, Owner)); //E_MyAnimState.E_INJURY
		AnimStates.Add (new AnimStateDeath (AnimEngine, Owner)); //E_MyAnimState._EDEATHM

		DefaultAnimState = AnimStates [(int)E_AnimState.E_IDLE];
		base.Initialize ();
	}

	/// <summary>
	/// 子类实现该方法，每个角色在做一个操作时会传人操作对应的AgentAction.
	/// 该方法内执行状态切换和状态选择
	/// </summary>
	/// <param name="_action">_action.</param>
	public override void DoAction (AgentAction action)
	{
		// 当前状态是否执行新的AgentAction，不执行就返回false，然后切换状态
		if (CurrentAnimState.HandleNewAction (action)) {
			NextAnimState = null;
		} else {
			if (action is AgentActionGoTo)
				NextAnimState = AnimStates [(int)E_AnimState.E_GOTO];
			if (action is AgentActionMove)
				NextAnimState = AnimStates [(int)E_AnimState.E_MOVE];
			else if (action is AgentActionAttack)
				NextAnimState = AnimStates [(int)E_AnimState.E_ATTACK_MELEE];
			else if (action is AgentActionRoll)
				NextAnimState = AnimStates [(int)E_AnimState.E_ROLL];
			else if (action is AgentActionWeaponShow)
				NextAnimState = AnimStates [(int)E_AnimState.E_IDLE];
			else if (action is AgentActionUseLever)
				NextAnimState = AnimStates [(int)E_AnimState.E_USE_LEVER];
			else if (action is AgentActionTeleport)
				NextAnimState = AnimStates [(int)E_AnimState.Teleport];
			else if (action is AgentActionInjury)
				NextAnimState = AnimStates [(int)E_AnimState.E_INJURY];
			else if (action is AgentActionDeath)
				NextAnimState = AnimStates [(int)E_AnimState.E_DEATH];
			
			// 有下一个状态就切换到下一个状态
			if (null != NextAnimState) {
				ProgressToNextStage (action);
			}
		}
	}
}

