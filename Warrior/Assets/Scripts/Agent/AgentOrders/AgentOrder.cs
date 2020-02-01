/***************************************************************
 * Class Name : Goal
 * Function   : A base class for all orders.. these are orders for squads or player input..
 *				
 * Created by : Marek Rabas
 **************************************************************/

using System;
using UnityEngine;

[System.Serializable]
public class AgentOrder
{
	public enum E_Status
	{
		E_INVALID,
		E_WAITING,
		E_EXECUTING,
		E_DONE,
		E_FAILED,
	}

	public enum E_OrderType
	{
		E_NONE,
		E_GOTO,
		E_ATTACK,
		E_DODGE,
		E_USE,
		E_STOPMOVE,
	}

	public E_OrderType Type;
	public Vector3 Position;
	public Vector3 Direction;
	public InteractionGameObject InteractionObject;
	public E_InteractionType Interaction;
	public Agent Target;
	public E_AttackType AttackType;
	public float MoveSpeedModifier;
	public AnimAttackData AnimAttackData;


	private AgentOrder ()
	{
		Type = E_OrderType.E_NONE;
	}


	public AgentOrder (E_OrderType type)
	{
		Type = type;
	}
}
