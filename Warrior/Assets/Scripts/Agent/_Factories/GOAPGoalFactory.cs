using System;

public enum E_GOAPGoals
{
	E_INVALID = -1,
	E_ORDER_ATTACK,
	E_ORDER_DODGE,
	E_ORDER_USE,
	E_GOTO,
	E_COMBAT_MOVE_LEFT,
	E_COMBAT_MOVE_RIGHT,
	E_COMBAT_MOVE_FORWARD,
	E_COMBAT_MOVE_BACKWARD,
	E_LOOK_AT_TARGET,
	E_KILL_TARGET,
	E_DODGE,
	E_DO_BLOCK,
	E_ALERT,
	E_CALM,
	E_USE_WORLD_OBJECT,
	E_PLAY_ANIM,
	E_IDLE_ANIM,
	E_REACT_TO_DAMAGE,
	E_BOSS_ATTACK,
	E_TELEPORT,
	E_COUNT,
}

class GOAPGoalFactory : System.Object
{
	public static GOAPGoal Create (E_GOAPGoals type, Agent owner)
	{
		GOAPGoal g;
		switch (type) {
		case E_GOAPGoals.E_ORDER_ATTACK:
			g = new GOAPGoalOrderAttack (owner);
			break;
		case E_GOAPGoals.E_ORDER_DODGE:
			g = new GOAPGoalOrderDodge (owner);
			break;
		case E_GOAPGoals.E_ORDER_USE:
			g = new GOAPGoalOrderUseWorldObject (owner);
			break;
		case E_GOAPGoals.E_GOTO:
			g = new GOAPGoalGoTo (owner);
			break;
		case E_GOAPGoals.E_COMBAT_MOVE_RIGHT:
			g = new GOAPGoalCombatMoveToRight (owner);
			break;
		case E_GOAPGoals.E_COMBAT_MOVE_LEFT:
			g = new GOAPGoalCombatMoveToLeft (owner);
			break;
		case E_GOAPGoals.E_COMBAT_MOVE_FORWARD:
			g = new GOAPGoalCombatMoveForward (owner);
			break;
		case E_GOAPGoals.E_COMBAT_MOVE_BACKWARD:
			g = new GOAPGoalCombatMoveBackward (owner);
			break;
		case E_GOAPGoals.E_LOOK_AT_TARGET:
			g = new GOAPGoalLookAtTarget (owner);
			break;
		case E_GOAPGoals.E_KILL_TARGET:
			g = new GOAPGoalKillTarget (owner);
			break;
		case E_GOAPGoals.E_DODGE:
			g = new GOAPGoalDodge (owner);
			break;
		case E_GOAPGoals.E_DO_BLOCK:
			g = new GOAPGoalDoBlock (owner);
			break;
		case E_GOAPGoals.E_ALERT:
			g = new GOAPGoalAlert (owner);
			break;
		case E_GOAPGoals.E_CALM:
			g = new GOAPGoalCalm (owner);
			break;
		case E_GOAPGoals.E_USE_WORLD_OBJECT:
			g = new GOAPGoalUseWorldObject (owner);
			break;
		case E_GOAPGoals.E_PLAY_ANIM:
			g = new GOAPGoalPlayAnim (owner);
			break;
		case E_GOAPGoals.E_IDLE_ANIM:
			g = new GOAPGoalIdleAction (owner);
			break;
		case E_GOAPGoals.E_REACT_TO_DAMAGE:
			g = new GOAPGoalReactToDamage (owner);
			break;
		case E_GOAPGoals.E_TELEPORT:
			g = new GOAPGoalTeleport (owner);
			break;
		default:
			return null;
		}

		g.InitGoal ();
		return g;
	}
}
