using System;
using UnityEngine;

//使用世界物品
class GOAPGoalUseWorldObject : GOAPGoal
{
    public GOAPGoalUseWorldObject(Agent owner) : base(E_GOAPGoals.E_USE_WORLD_OBJECT, owner) { }


	public override void InitGoal()
	{

	}

    public override float GetMaxRelevancy()
    {
        return Owner.BlackBoard.GOAP_UseWorlObjectRelevancy;
    }

	public override void CalculateGoalRelevancy()
	{
        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_USE_WORLD_OBJECT);
        if (prop != null && prop.GetBool() == true)
            GoalRelevancy = Owner.BlackBoard.GOAP_UseWorlObjectRelevancy;
        else
            GoalRelevancy = 0;
	}

    public override void SetDisableTime() { NextEvaluationTime = Owner.BlackBoard.GOAP_UseWorlObjectDelay + Time.timeSinceLevelLoad; }

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
        worldState.SetWSProperty(E_PropKey.E_USE_WORLD_OBJECT, false);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
        WorldStateProp prop = worldState.GetWSProperty(E_PropKey.E_USE_WORLD_OBJECT);

		if (prop.GetBool() == false)
			return true;

		return false;
	}

    public override bool IsSatisfied()
    {
        return IsPlanFinished();
    }
}

