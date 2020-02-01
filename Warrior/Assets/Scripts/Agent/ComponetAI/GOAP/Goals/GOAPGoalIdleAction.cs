using System;
using UnityEngine;

/******************************************************************
 * GOAL STATS
 * 
 * For initialize : 
 *                  E_PropKey.E_IDLING              true
 * For planing : 
 *                  E_PropKey.E_IDLING              false
 * Set : 
 *                  
 * Finished:
 *                  When action is done
 * 
 * ***************************************************************/

//闲置动画
//idle状态，武器在手，5秒后 －－> Idle状态
class GOAPGoalIdleAction : GOAPGoal
{
	public GOAPGoalIdleAction (Agent owner) : base (E_GOAPGoals.E_IDLE_ANIM, owner)
	{
	}

	public override void InitGoal ()
	{

	}

	public override float GetMaxRelevancy ()
	{
		return Owner.BlackBoard.GOAP_IdleActionRelevancy;
	}

	public override void CalculateGoalRelevancy ()
	{
		WorldStateProp prop = Owner.WorldState.GetWSProperty (E_PropKey.E_IDLING);
		WorldStateProp prop2 = Owner.WorldState.GetWSProperty (E_PropKey.E_WEAPON_IN_HANDS);

		if (prop != null && prop.GetBool () == true && prop2.GetBool () == true && Owner.BlackBoard.IdleTimer > 5)
			GoalRelevancy = Owner.BlackBoard.GOAP_IdleActionRelevancy;
		else
			GoalRelevancy = 0;
	}

	public override void SetDisableTime ()
	{
		NextEvaluationTime = Owner.BlackBoard.GOAP_IdleActionDelay + Time.timeSinceLevelLoad;
	}

	public override void SetWSSatisfactionForPlanning (WorldState worldState)
	{
		worldState.SetWSProperty (E_PropKey.E_IDLING, false);
	}

	public override bool IsWSSatisfiedForPlanning (WorldState worldState)
	{
		WorldStateProp prop = worldState.GetWSProperty (E_PropKey.E_IDLING);

		if (prop.GetBool () == false)
			return true;

		return false;
	}

	public override bool IsSatisfied ()
	{
		return IsPlanFinished ();
	}
}

