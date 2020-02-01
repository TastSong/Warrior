using System;
using UnityEngine;

//E_PropKey.E_AT_TARGET_POS
class GOAPGoalDoBlock : GOAPGoal
{
    float WorldStateTime; 
    public GOAPGoalDoBlock(Agent owner) : base(E_GOAPGoals.E_DO_BLOCK, owner) { }

	public override void InitGoal()
	{

	}

    public override float GetMaxRelevancy()
    {
        return Owner.BlackBoard.GOAP_BlockRelevancy;
    }
	public override void CalculateGoalRelevancy()
    {
        GoalRelevancy = 0;

        if (Owner.BlackBoard.DesiredTarget == null)
            return;

        if (Owner.BlackBoard.Dodge < Owner.BlackBoard.DodgeMax * 0.25f)
            return;

        if (Owner.BlackBoard.DesiredTarget != null && Owner.BlackBoard.DistanceToTarget > 3.5f)
            return;

        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_IN_BLOCK);
        WorldStateProp prop2 = Owner.WorldState.GetWSProperty(E_PropKey.E_LOOKING_AT_TARGET);
        WorldStateProp prop3 = Owner.WorldState.GetWSProperty(E_PropKey.E_IN_COMBAT_RANGE);
        WorldStateProp prop4 = Owner.WorldState.GetWSProperty(E_PropKey.E_AHEAD_OF_ENEMY);

        // if (Owner.debugGOAP && prop != null) Debug.Log(this.ToString() + " " + prop.GetBool() + prop2.GetBool() + prop3.GetBool() + prop4.GetBool());

        if (prop != null && prop.GetBool() == false && prop2.GetBool() == true && prop3.GetBool() == true && prop4.GetBool() == true)
            GoalRelevancy = Owner.BlackBoard.GOAP_BlockRelevancy * Owner.BlackBoard.Dodge * 0.01f;
           

       // if(Owner.debugGOAP)  Debug.Log(this.ToString() + " " + GoalRelevancy);
    }

    public override void SetDisableTime() { NextEvaluationTime = Owner.BlackBoard.GOAP_BlockDelay + Time.timeSinceLevelLoad; }

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
        worldState.SetWSProperty(E_PropKey.E_IN_BLOCK, true);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
        WorldStateProp prop = worldState.GetWSProperty(E_PropKey.E_IN_BLOCK);

		if (prop != null && prop.GetBool() == true)
			return true;

		return false;
	}

    public override bool IsSatisfied()
    {
        return IsPlanFinished();
    }

	public override bool Activate(GOAPPlan plan)
	{
        Owner.BlackBoard.DesiredAttacker = Owner.BlackBoard.DesiredTarget;

        GoalRelevancy = Owner.BlackBoard.GOAP_BlockRelevancy;

        return base.Activate(plan);
	}
}

