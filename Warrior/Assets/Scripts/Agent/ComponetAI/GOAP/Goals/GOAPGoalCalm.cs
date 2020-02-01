using System;
using UnityEngine;

/******************************************************************
 * GOAL STATS
 * 
 * For initialize : 
 *                  E_PropKey.E_WEAPON_IN_HANDS     true
 *                  E_PropKey.E_ALERTED             false    
 * For planing : 
 *                  E_PropKey.E_WEAPON_IN_HANDS     false
 * Set : 
 *                  
 * Finished:
 *                  E_WEAPON_IN_HANDS               false
 * 
 * ***************************************************************/

//Calm 平静，安静，镇定
class GOAPGoalCalm : GOAPGoal
{
    public GOAPGoalCalm(Agent owner) : base(E_GOAPGoals.E_CALM, owner) { }

	public override void InitGoal()
	{

	}

    public override float GetMaxRelevancy()
    {
        return Owner.BlackBoard.GOAP_CalmRelevancy;
    }

	public override void CalculateGoalRelevancy()
	{
        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_WEAPON_IN_HANDS);
        WorldStateProp prop2 = Owner.WorldState.GetWSProperty(E_PropKey.E_ALERTED);
        if (prop != null && prop2 != null && prop.GetBool() == true && prop2.GetBool() == false && Owner.BlackBoard.IdleTimer > 1.5f && UnityEngine.Random.Range(0, 100) < 5)
            GoalRelevancy = Owner.BlackBoard.GOAP_CalmRelevancy;
        else
            GoalRelevancy = 0;

        SetDisableTime();
	}

    public override void SetDisableTime() { NextEvaluationTime = Owner.BlackBoard.GOAP_CalmDelay + Time.timeSinceLevelLoad; }

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
        worldState.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, false);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
        WorldStateProp prop = worldState.GetWSProperty(E_PropKey.E_WEAPON_IN_HANDS);

		if (prop.GetBool() == false)
			return true;

		return false;
	}

    public override bool IsSatisfied()
    {
        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_WEAPON_IN_HANDS);

        if (prop.GetBool() == false)
            return true;

        return false;
    }


	public override bool ReplanRequired()
	{ 
		return false; 
	}
}

