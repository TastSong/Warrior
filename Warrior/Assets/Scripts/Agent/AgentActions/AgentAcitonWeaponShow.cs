//
// /**************************************************************************
//
// AgentActonWeaponShow.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-17
//
// Description:Idle状态接受的命令，隐藏和显示武器
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;

public class AgentActionWeaponShow : AgentAction
{
	/// <summary>
	/// 武器是否显示.
	/// </summary>
	public bool Show = true;

	/// <summary>
	/// Initializes a new instance of the <see cref="AgentActonWeaponShow"/> class.
	/// </summary>
	public AgentActionWeaponShow() : base(AgentActionFactory.E_Type.E_WEAPON_SHOW)
	{
	}
}

