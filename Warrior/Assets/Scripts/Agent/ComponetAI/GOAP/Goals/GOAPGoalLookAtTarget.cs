using System;
using UnityEngine;


// activate goal when 
// E_PropKey.E_LOOK_AT_TARGET == set true to plan this goal.. if false, then is satisfied

//
class GOAPGoalLookAtTarget : GOAPGoal
{
    Fact Query;

    public GOAPGoalLookAtTarget(Agent owner) : base(E_GOAPGoals.E_LOOK_AT_TARGET, owner) { }

	public override void InitGoal()
	{
        Query = new Fact(Fact.E_FactType.E_EVENT);
        Query.EventType = E_EventTypes.EnemyLost;

	}
    public override float GetMaxRelevancy()
    {
        return Owner.BlackBoard.GOAP_LookAtTargetRelevancy;
    }

	public override void CalculateGoalRelevancy()
	{
		//AgentOrder order = Ai.BlackBoard.OrderGet();
		//if(order != null && order.Type == AgentOrder.E_OrderType.E_GOTO)
        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_LOOKING_AT_TARGET);
        WorldStateProp prop2 = Owner.WorldState.GetWSProperty(E_PropKey.E_AT_TARGET_POS);

	if (prop != null && prop.GetBool() == false)
        {
            if (Owner.BlackBoard.DesiredTarget != null && prop2.GetBool() == true)
            {
                Vector3 dirToTarget = Owner.BlackBoard.DesiredTarget.Position - Owner.Position;
                GoalRelevancy = Mathf.Min(Owner.BlackBoard.GOAP_LookAtTargetRelevancy, Vector3.Angle(Owner.Forward, dirToTarget) * 0.01f);
                return;
            }

            Fact fact = Owner.Memory.GetFact(Query);

            if (fact != null && fact.Belief > 0.2f)
            {
                GoalRelevancy = Owner.BlackBoard.GOAP_LookAtTargetRelevancy;
                return;
            }
        }
        GoalRelevancy = 0;
	}

    public override void SetDisableTime() { NextEvaluationTime = Owner.BlackBoard.GOAP_LookAtTargetDelay + Time.timeSinceLevelLoad; }

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
		worldState.SetWSProperty(E_PropKey.E_LOOKING_AT_TARGET, true);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
        WorldStateProp prop = worldState.GetWSProperty(E_PropKey.E_LOOKING_AT_TARGET);
        if (prop != null && prop.GetBool() == true)
            return true;

		return false;
	}

    public override bool IsSatisfied()
    {
        return IsPlanFinished();
    }

	public override bool ReplanRequired()
	{
		return false; 
	}

    public override bool IsPlanValid()
    {
        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_LOOKING_AT_TARGET);

        if (prop.GetBool() == true)
            return false;

        return  base.IsPlanValid();
        
    }


	public override bool Activate(GOAPPlan plan)
	{
        Owner.WorldState.SetWSProperty(E_PropKey.E_LOOKING_AT_TARGET, false);

		return base.Activate(plan);
	}
}

