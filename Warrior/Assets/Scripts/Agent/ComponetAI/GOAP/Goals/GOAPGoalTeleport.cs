using System;
using UnityEngine;

/******************************************************************
 * GOAL STATS
 * 
 * For initialize : 
 *                  E_PropKey.E_WEAPON_IN_HANDS     false
 *                  E_PropKey.E_ALERTED             true    
 * For planing : 
 *                  E_PropKey.E_WEAPON_IN_HANDS     true
 * Set : 
 *                  
 * Finished:
 *                  E_WEAPON_IN_HANDS                true
 * 
 * ***************************************************************/

//传送
//武器不在手，警惕 -> 武器在手
class GOAPGoalTeleport : GOAPGoal
{
    public GOAPGoalTeleport(Agent owner) : base(E_GOAPGoals.E_TELEPORT, owner) { }

	public override void InitGoal()
	{

	}

    public override float GetMaxRelevancy()
    {
        return Owner.BlackBoard.GOAP_TeleportRelevancy;
    }
	public override void CalculateGoalRelevancy()
	{
        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_TELEPORT);
        if (prop != null  &&  prop.GetBool() == true)
            GoalRelevancy = Owner.BlackBoard.GOAP_TeleportRelevancy;
        else
            GoalRelevancy = 0;
	}

	public override void SetDisableTime() { NextEvaluationTime = Owner.BlackBoard.GOAP_TeleportDelay + Time.timeSinceLevelLoad;}

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
        worldState.SetWSProperty(E_PropKey.E_TELEPORT, false);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
        WorldStateProp prop = worldState.GetWSProperty(E_PropKey.E_TELEPORT);

		if (prop.GetBool() == false)
			return true;

		return false;
	}

    public override bool IsSatisfied()
    {
        return IsPlanFinished();
    }
}

