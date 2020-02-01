//
// /**************************************************************************
//
// AgentAction.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-17
//
// Description:  FSM接受的动作命令基类
//               定义了动作状态，提供状态访问接口
//               定义了动作命令类型
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;

public class AgentAction : System.Object
{
	/// <summary>
	/// 动作状态枚举.
	/// </summary>
	public enum E_State
	{
		/// <summary>
		/// 动作激活状态.
		/// </summary>
		E_Active,
		/// <summary>
		/// 动作执行成功.
		/// </summary>
		E_Success,
		/// <summary>
		/// 动作执行失败.
		/// </summary>
		E_Failed,
		/// <summary>
		/// 动作未使用，在缓存池中标志该动作可以使用.
		/// </summary>
		E_Unused,
	}

	/// <summary>
	/// 动作状态.
	/// </summary>
	public E_State Status = AgentAction.E_State.E_Active;
	/// <summary>
	/// 动作命令类型.
	/// </summary>
	public AgentActionFactory.E_Type Type;

	/// <summary>
	/// Initializes a new instance of the <see cref="AgentAction"/> class.
	/// </summary>
	/// <param name="_type">_type.</param>
	public AgentAction(AgentActionFactory.E_Type _type)
	{
		Type = _type;
	}

	/// <summary>
	/// 动作是否激活.
	/// </summary>
	/// <returns><c>true</c> if this instance is active; otherwise, <c>false</c>.</returns>
	public bool IsActive(){return Status == E_State.E_Active;}
	/// <summary>
	/// 动作是否失败.
	/// </summary>
	/// <returns><c>true</c> if this instance is failed; otherwise, <c>false</c>.</returns>
	public bool	IsFailed(){return Status == E_State.E_Failed;}
	/// <summary>
	/// 动作是否成功.
	/// </summary>
	/// <returns><c>true</c> if this instance is success; otherwise, <c>false</c>.</returns>
	public bool	IsSuccess() {return Status == E_State.E_Success;}
	/// <summary>
	/// 动作是否未使用.
	/// </summary>
	/// <returns><c>true</c> if this instance is unused; otherwise, <c>false</c>.</returns>
	public bool	IsUnused() { return Status == E_State.E_Unused; }
	
	/// <summary>
	/// 设置动作执行成功.
	/// </summary>
	public void	SetSuccess(){Status = E_State.E_Success; /*Debug.Log(this.ToString() + " set to " + Status.ToString());*/}
	/// <summary>
	/// 设置动作执行失败.
	/// </summary>
	public void SetFailed() {Status = E_State.E_Failed; /*Debug.Log(this.ToString() + " set to " + Status.ToString());*/}
	/// <summary>
	/// 设置动作未使用.
	/// </summary>
	public void	SetUnused() { Status = E_State.E_Unused; }
	/// <summary>
	/// 设置动作激活.
	/// </summary>
	public void	SetActive() { Status = E_State.E_Active; }

	/// <summary>
	/// Reset this instance.
	/// </summary>
	public virtual void Reset()
	{

	}
}

