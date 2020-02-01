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

// Alert 警惕，危险。。
class GOAPGoalAlert : GOAPGoal
{
    public GOAPGoalAlert(Agent owner) : base(E_GOAPGoals.E_ALERT, owner) { }

	public override void InitGoal()
	{

	}

    public override float GetMaxRelevancy()
    {
        return Owner.BlackBoard.GOAP_AlertRelevancy;
    }

	public override void CalculateGoalRelevancy()
	{
        GoalRelevancy = 0;

//        if (Owner.BlackBoard.DesiredTarget == null)
//            return;

        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_WEAPON_IN_HANDS);
        WorldStateProp prop2 = Owner.WorldState.GetWSProperty(E_PropKey.E_ALERTED);
        if (prop != null  && prop2 != null && prop.GetBool() == false && prop2.GetBool() == true)
            GoalRelevancy = Owner.BlackBoard.GOAP_AlertRelevancy;
	}

	public override void SetDisableTime() { NextEvaluationTime = Owner.BlackBoard.GOAP_AlertDelay + Time.timeSinceLevelLoad;}

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
        worldState.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, true);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
        WorldStateProp prop = worldState.GetWSProperty(E_PropKey.E_WEAPON_IN_HANDS);

		if (prop.GetBool() == true)
			return true;

		return false;
	}

    public override bool IsSatisfied()
    {
        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_WEAPON_IN_HANDS);

        if (prop.GetBool() == true)
            return true;

        return false;
    }
}

