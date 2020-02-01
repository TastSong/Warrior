using System;
using UnityEngine;

//E_PropKey.E_AT_TARGET_POS
class GOAPGoalReactToDamage : GOAPGoal
{
    //Fact Query;

    public GOAPGoalReactToDamage(Agent owner) : base(E_GOAPGoals.E_REACT_TO_DAMAGE, owner) { }

	public override void InitGoal()
	{
       /* Query = new Fact(Fact.E_FactType.E_EVENT);
        Query.EventType = E_EventTypes.Hit;*/
        Critical = true;
	}

    public override float GetMaxRelevancy()
    {
        return Owner.BlackBoard.GOAP_ReactToDamageRelevancy;
    }

	public override void CalculateGoalRelevancy()
	{
        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_EVENT);

        if (prop != null)
        {
            if (Owner.BlackBoard.ReactOnHits && prop.GetEvent() == E_EventTypes.Hit)
            {
                /*Fact fact = Owner.Memory.GetFact(Query);
                if (fact != null && fact.Belief > 0.7f)*/
                    GoalRelevancy = Owner.BlackBoard.GOAP_ReactToDamageRelevancy;
            }
            else if (Owner.BlackBoard.ReactOnHits && prop.GetEvent() == E_EventTypes.Knockdown)
            {
                GoalRelevancy = Owner.BlackBoard.GOAP_ReactToDamageRelevancy;
            }
            else if (prop.GetEvent() == E_EventTypes.Died)
                GoalRelevancy = Owner.BlackBoard.GOAP_ReactToDamageRelevancy;
            else
                GoalRelevancy = 0.0f;
        }
        else
            GoalRelevancy = 0;
	}

	public override void SetDisableTime() { NextEvaluationTime = 0;}

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
        worldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.None);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
        WorldStateProp prop = worldState.GetWSProperty(E_PropKey.E_EVENT);

		if (prop != null && prop.GetEvent() == E_EventTypes.None)
			return true;

		return false;
	}

    public override bool IsSatisfied()
    {
        return IsPlanFinished();
    }
}

