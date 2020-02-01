using System;
using UnityEngine;

public enum E_GOAPAction
{
	invalid = -1,
	move,
	//0
	gotoPos,
	//70
	gotoMeleeRange,
	//70
	combatMoveRight,
	//0
	combatMoveLeft,
	//0
	combatMoveForward,
	//0
	combatMoveBackward,
	//0
	combatRunForward,
	//0
	combatRunBackward,
	//0
	lookatTarget,
	//95
	weaponShow,
	//90
	weaponHide,
	//0
	attackMeleeOnce,
	//0
	attackWhirl,
	//0
	attackBerserk,
	//0
	attackMeleeTwoSwords,
	//0
	attackBow,
	//0
	attackBoss,
	//20
	attackRoll,
	//20
	attackCounter,
	//20
	orderAttack,
	//0
	orderDodge,
	//0
	rollToTarget,
	//80
	useLever,
	//0
	playAnim,
	//0
	injury,
	//100
	injuryOrochi,
	//100
	death,
	//100
	block,
	//50
	tount,
	//0
	knockdown,
	//100
	teleport,
	//90
	count
}

class GOAPActionFactory : System.Object
{
	public static GOAPAction Create (E_GOAPAction type, Agent owner)
	{
		GOAPAction a;
		switch (type) {
		case E_GOAPAction.move:
			a = new GOAPActionMove (owner);
			break;
		case E_GOAPAction.gotoPos:
			a = new GOAPActionGoTo (owner);
			break;
		case E_GOAPAction.combatMoveRight:
			a = new GOAPActionCombatMoveToRight (owner);
			break;
		case E_GOAPAction.combatMoveLeft:
			a = new GOAPActionCombatMoveToLeft (owner);
			break;
		case E_GOAPAction.combatMoveForward:
			a = new GOAPActionCombatMoveForward (owner);
			break;
		case E_GOAPAction.combatMoveBackward:
			a = new GOAPActionCombatMoveBackward (owner);
			break;
		case E_GOAPAction.combatRunForward:
			a = new GOAPActionCombatRunForward (owner);
			break;
		case E_GOAPAction.combatRunBackward:
			a = new GOAPActionCombatRunBackward (owner);
			break;
		case E_GOAPAction.lookatTarget:
			a = new GOAPActionLookAtTarget (owner);
			break;
		case E_GOAPAction.gotoMeleeRange:
			a = new GOAPActionGoToMeleeRange (owner);
			break;
		case E_GOAPAction.weaponShow:
			a = new GOAPActionWeaponShow (owner);
			break;
		case E_GOAPAction.weaponHide:
			a = new GOAPActionWeaponHide (owner);
			break;
		case E_GOAPAction.attackMeleeOnce:
			a = new GOAPActionAttackMeleeOnce (owner);
			break;
		case E_GOAPAction.attackBerserk:
			a = new GOAPActionAttackBerserk (owner);
			break;
		case E_GOAPAction.attackWhirl:
			a = new GOAPActionAttackWhirl (owner);
			break;
		case E_GOAPAction.attackCounter:
			a = new GOAPActionAttackCounter (owner);
			break;
		case E_GOAPAction.attackMeleeTwoSwords:
			a = new GOAPActionAttackMeleeTwoSwords (owner);
			break;
		case E_GOAPAction.attackBow:
			a = new GOAPActionAttackBow (owner);
			break;
		case E_GOAPAction.orderAttack:
			a = new GOAPActionOrderAttack (owner);
			break;
		case E_GOAPAction.attackBoss:
			a = new GOAPActionAttackBossSpecial (owner);
			break;
		case E_GOAPAction.attackRoll:
			a = new GOAPActionAttackRoll (owner);
			break;
		case E_GOAPAction.orderDodge:
			a = new GOAPActioOrderDodge (owner);
			break;
		case E_GOAPAction.rollToTarget:
			a = new GOAPActionRollToTarget (owner);
			break;
		case E_GOAPAction.useLever:
			a = new GOAPActionUseLever (owner);
			break;
		case E_GOAPAction.playAnim:
			a = new GOAPActionPlayAnim (owner);
			break;
		case E_GOAPAction.injury:
			a = new GOAPActionInjury (owner);
			break;
		case E_GOAPAction.injuryOrochi:
			a = new GOAPActionInjuryOrochi (owner);
			break;
		case E_GOAPAction.death:
			a = new GOAPActionDeath (owner);
			break;
		case E_GOAPAction.block:
			a = new GOAPActionBlock (owner);
			break;
		case E_GOAPAction.tount:
			a = new GOAPActionIdleActionTount (owner);
			break;
		case E_GOAPAction.knockdown:
			a = new GOAPActionKnockdown (owner);
			break;
		case E_GOAPAction.teleport:
			a = new GOAPActionTeleport (owner);
			break;
		default:
			Debug.LogError ("GOAPActionFactory -  unknow state " + type);
			return null;
		}

		a.InitAction ();
		return a;
	}
}
