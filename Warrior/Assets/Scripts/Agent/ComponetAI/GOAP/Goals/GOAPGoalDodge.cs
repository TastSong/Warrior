using System;
using UnityEngine;

//E_PropKey.E_AT_TARGET_POS
class GOAPGoalDodge : GOAPGoal
{
    float WorldStateTime; 
    public GOAPGoalDodge(Agent owner) : base(E_GOAPGoals.E_DODGE, owner) { }

	public override void InitGoal()
	{

	}
    
    public override float GetMaxRelevancy()
    {
        return Owner.BlackBoard.GOAP_DodgeRelevancy;
    }

	public override void CalculateGoalRelevancy()
    {
        if (Owner.CurrentGOAPGoal == this)
        {
            GoalRelevancy = 0;
        }
        else 
        {
            WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_IN_DODGE);
            if (prop != null && prop.GetBool() == false)
                GoalRelevancy = Owner.BlackBoard.GOAP_DodgeRelevancy;
            else
                GoalRelevancy = 0;
        }
    }

    public override void SetDisableTime() { NextEvaluationTime = Owner.BlackBoard.GOAP_DodgeDelay + Time.timeSinceLevelLoad; }

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
        worldState.SetWSProperty(E_PropKey.E_IN_DODGE, true);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
        WorldStateProp prop = worldState.GetWSProperty(E_PropKey.E_IN_DODGE);

		if (prop != null && prop.GetBool() == true)
			return true;

		return false;
	}

    public override bool IsSatisfied()
    {
        return IsPlanFinished();
    }
}

