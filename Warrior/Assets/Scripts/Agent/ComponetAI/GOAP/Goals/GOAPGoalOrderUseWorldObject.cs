using System;
using UnityEngine;


class GOAPGoalOrderUseWorldObject : GOAPGoal
{
    public GOAPGoalOrderUseWorldObject(Agent owner) : base(E_GOAPGoals.E_ORDER_USE, owner) { }

	public override void InitGoal()
	{

	}

    public override float GetMaxRelevancy()
    {
        return Owner.BlackBoard.GOAP_UseWorlObjectRelevancy;
    }

	public override void CalculateGoalRelevancy()
	{
        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_ORDER);
        if (prop != null && prop.GetOrder() ==  AgentOrder.E_OrderType.E_USE)
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

	public override void Deactivate()
	{
		base.Deactivate();

        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_ORDER);
        if (prop.GetOrder() == AgentOrder.E_OrderType.E_USE)
            Owner.WorldState.SetWSProperty(E_PropKey.E_ORDER, AgentOrder.E_OrderType.E_NONE);
	}
}

