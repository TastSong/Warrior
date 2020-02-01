//
// /**************************************************************************
//
// AnimComponent.cs
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

public enum E_AnimFSMTypes
{
	Player,
	Archer,
	Swordsman,
	Peasant,
	DoubleSwordsman,
	Musketeer,
	MiniBoss01,
	MiniBoss02,
	Boss01,
	Boss02,
	Boss03,
	BossOrochi,

}
public class AnimComponent : MonoBehaviour, IActionHandle
{
	public E_AnimFSMTypes TypeOfFSM;

	private AnimFSM FSM;
	private Animation Animation;
	private Agent Owner;

	public void Awake()
	{
		Animation = GetComponent<Animation>();
		Owner = GetComponent<Agent>();

		switch (TypeOfFSM)
		{
		case E_AnimFSMTypes.Player:
			FSM = new AnimFSMPlayer(Animation, Owner);
			break;
		case E_AnimFSMTypes.Archer:
			FSM = new AnimFSMEnemyArcher(Animation, Owner);
			break;
		case E_AnimFSMTypes.Swordsman:
			FSM = new AnimFSMEnemySwordsman(Animation, Owner);
			break;
		case  E_AnimFSMTypes.Peasant:
			FSM = new AnimFSMEnemyPeasant(Animation, Owner);
			break;
		case E_AnimFSMTypes.MiniBoss01:
			FSM = new AnimFSMEnemyMiniBoss(Animation, Owner);
			break;
		case E_AnimFSMTypes.MiniBoss02:
			FSM = new AnimFSMEnemyMiniBoss(Animation, Owner);
			break;
		case E_AnimFSMTypes.DoubleSwordsman:
			FSM = new AnimFSMEnemyDoubleSwordsman(Animation, Owner);
			break;
		case E_AnimFSMTypes.Musketeer:
			FSM = new AnimFSMEnemyDoubleSwordsman(Animation, Owner);
			break;
		case E_AnimFSMTypes.Boss01:
			FSM = new AnimFSMEnemyBoss01(Animation, Owner);
			break;
		case E_AnimFSMTypes.Boss02:
			FSM = new AnimFSMEnemyBoss02(Animation, Owner);
			break;
		case E_AnimFSMTypes.Boss03:
			FSM = new AnimFSMEnemyBoss03(Animation, Owner);
			break;
		case E_AnimFSMTypes.BossOrochi:
			FSM = new AnimFSMEnemyBossOrochi(Animation, Owner);
			break;
		default:
			Debug.LogError(this.name + "unknow type of FSM. Type: " + TypeOfFSM.ToString());
			break;
		}
	}
	
	// Use this for initialization
	void Start()
	{
		FSM.Initialize();
		Owner.BlackBoard.AddActionHandle(this);
	}
	
	// Update is called once per frame
	void Update()
	{
		FSM.Update();
	}

	#region IActionHandle implementation
	public void HandleAction (AgentAction _action)
	{
		if (_action.IsFailed())
			return;

		FSM.DoAction(_action);
	}
	#endregion

	
	public void Activate()
	{
		// todo: add animation op ...
		Animation.Stop();
		Animation.Rewind();

		FSM.Initialize();
	}
	
	public void Deactivate()
	{
		FSM.Reset();
	}
}

