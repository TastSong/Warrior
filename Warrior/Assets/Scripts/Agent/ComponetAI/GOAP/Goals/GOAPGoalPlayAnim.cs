using System;
using UnityEngine;

//E_PropKey.E_AT_TARGET_POS
//播放动画
class GOAPGoalPlayAnim : GOAPGoal
{
	public GOAPGoalPlayAnim(Agent owner) : base(E_GOAPGoals.E_PLAY_ANIM, owner) { }

	public override void InitGoal()
	{

	}
    public override float GetMaxRelevancy()
    {
        return Owner.BlackBoard.GOAP_PlayAnimRelevancy;
    }

	public override void CalculateGoalRelevancy()
	{
		//AgentOrder order = Ai.BlackBoard.OrderGet();
		//if(order != null && order.Type == AgentOrder.E_OrderType.E_GOTO)
        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_PLAY_ANIM);
        if (prop != null && prop.GetBool() == true)
            GoalRelevancy = Owner.BlackBoard.GOAP_PlayAnimRelevancy;
        else
            GoalRelevancy = 0;
	}

    public override void SetDisableTime() { NextEvaluationTime = Owner.BlackBoard.GOAP_PlayAnimDelay + Time.timeSinceLevelLoad; }

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
        worldState.SetWSProperty(E_PropKey.E_PLAY_ANIM, false);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
        WorldStateProp prop = worldState.GetWSProperty(E_PropKey.E_PLAY_ANIM);

		if (prop != null && prop.GetBool() == false)
			return true;

		return false;
	}

    public override bool IsSatisfied()
    {
        return IsPlanFinished();
    }

}

