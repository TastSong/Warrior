using System;
using UnityEngine;

//E_PropKey.E_AT_TARGET_POS
//移动，转到
// 目标位置
class GOAPGoalGoTo : GOAPGoal
{
	public GOAPGoalGoTo(Agent owner) : base(E_GOAPGoals.E_GOTO, owner) { }

	public override void InitGoal()
	{
	}

    public override float GetMaxRelevancy()
    {
        return Owner.BlackBoard.GOAP_GoToRelevancy;
    }

	public override void CalculateGoalRelevancy()
	{
		//AgentOrder order = Ai.BlackBoard.OrderGet();
		//if(order != null && order.Type == AgentOrder.E_OrderType.E_GOTO)

        if (Owner.BlackBoard.MotionType != E_MotionType.None)
        {
            GoalRelevancy = 0;
        }
        else
        {
            WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_AT_TARGET_POS);
            if (prop != null && prop.GetBool() == false)
                GoalRelevancy = Owner.BlackBoard.GOAP_GoToRelevancy;
            else
                GoalRelevancy = 0;
        }
	}

    public override void SetDisableTime() { NextEvaluationTime = Owner.BlackBoard.GOAP_GoToDelay + Time.timeSinceLevelLoad; }

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
		worldState.SetWSProperty(E_PropKey.E_AT_TARGET_POS, true);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
		WorldStateProp prop = worldState.GetWSProperty(E_PropKey.E_AT_TARGET_POS);

		if (prop != null && prop.GetBool() == true)
			return true;

		return false;
	}

    public override bool IsSatisfied()
    {
        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_AT_TARGET_POS);

        if (prop != null && prop.GetBool() == true)
            return true;

        return false;
    }
}

