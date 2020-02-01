//
// /**************************************************************************
//
// AnimFSM.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-17
//
// Description:	有限状态机基类
//              抽象基类,更新当前状态，提供状态切换接口
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AnimFSM
{
	/// <summary>
	/// 该状态机所有状态列表，子类Initialize内初始化改列表
	/// </summary>
	protected List<AnimState> AnimStates;

	/// <summary>
	/// 当前状态
	/// </summary>
	protected AnimState CurrentAnimState;

	/// <summary>
	/// 下一个状态
	/// </summary>
	protected AnimState NextAnimState;

	/// <summary>
	/// 默认状态
	/// </summary>
	protected AnimState DefaultAnimState;
	
	protected Animation AnimEngine;
	protected Agent Owner;

	/// <summary>
	/// Initializes a new instance of the <see cref="AnimFSM"/> class.
	/// </summary>
	/// <param name="_anims">角色A的nimation.</param>
	/// <param name="_owner">角色的Agent.</param>
	public AnimFSM(Animation _anims, Agent _owner)
	{
		AnimEngine = _anims;
		Owner = _owner;
		AnimStates = new List<AnimState>();
	}

	/// <summary>
	/// Initialize this instance.
	/// </summary>
	public virtual void Initialize()
	{
		CurrentAnimState = DefaultAnimState;
		CurrentAnimState.OnActivate(null);
		NextAnimState = null;
	}

	
	/// <summary>
	/// FSM中调用状态的每帧更新.
	/// </summary>
	public void Update()
	{
		// 当前状态执行完成时
		if (CurrentAnimState.IsFinished())
		{
			// 禁用当前状态
			CurrentAnimState.OnDeactivate();
			
			// 切换到默认状态
			CurrentAnimState = DefaultAnimState;
			CurrentAnimState.OnActivate(null);
		}
		CurrentAnimState.Update();
	}

	/// <summary>
	/// Reset this instance.
	/// </summary>
	public void Reset()
	{
		for (int i=0; i<AnimStates.Count; i++)
		{
			if (AnimStates[i].IsFinished() == false)
			{
				AnimStates[i].OnDeactivate();
				AnimStates[i].SetFinished(true);
			}
		}
	}

	/// <summary>
	/// 子类实现该方法，每个角色在做一个操作时会传人操作对应的AgentAction.
	/// 该方法内执行状态切换和状态选择
	/// </summary>
	/// <param name="_action">_action.</param>
	public abstract void DoAction(AgentAction _action);

	/// <summary>
	/// 切换到下一个状态
	/// </summary>
	/// <param name="_action">_action.</param>
	protected void ProgressToNextStage(AgentAction _action)
	{
		if (NextAnimState != null)
		{
			CurrentAnimState.Release();
			CurrentAnimState.OnDeactivate();
			
			CurrentAnimState = NextAnimState;
			CurrentAnimState.OnActivate(_action);
			
			NextAnimState = null;
		}
	}
}

