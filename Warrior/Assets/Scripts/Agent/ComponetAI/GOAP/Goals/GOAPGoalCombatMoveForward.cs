using System;
using UnityEngine;


// activate goal when 
// E_PropKey.E_ENEMY_IN_SIGHT == true
// E_PropKey.E_IN_WEAPONS_RANGE == false


//战斗中向前移动
//敌人在视野内 不在武器范围内
class GOAPGoalCombatMoveForward : GOAPGoal
{
    public GOAPGoalCombatMoveForward(Agent owner) : base(E_GOAPGoals.E_COMBAT_MOVE_FORWARD, owner) { }

	public override void InitGoal()
	{
	}

    public override float GetMaxRelevancy()
    {
        return Owner.BlackBoard.GOAP_CombatMoveForwardRelevancy;
    }

	public override void CalculateGoalRelevancy()
	{
        if (Owner.BlackBoard.DesiredTarget == null)
            return;

		if (MissionZone.Instance.CurrentGameZone.GetNearestAliveEnemy(Owner, E_Direction.Forward, 0.4f) != null)
        {
            GoalRelevancy = 0;
            return;
        }

        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_ALERTED);
        WorldStateProp prop2 = Owner.WorldState.GetWSProperty(E_PropKey.E_IN_COMBAT_RANGE);

        if (prop.GetBool() == true && prop2.GetBool() == false && Owner.BlackBoard.DistanceToTarget > Owner.BlackBoard.CombatRange)
            GoalRelevancy = Mathf.Min(Owner.BlackBoard.GOAP_CombatMoveForwardRelevancy,  (Owner.BlackBoard.DistanceToTarget - Owner.BlackBoard.CombatRange) * 0.2f);
        else
            GoalRelevancy = 0;
	}

    public override void SetDisableTime() { NextEvaluationTime = Owner.BlackBoard.GOAP_CombatMoveForwardDelay + Time.timeSinceLevelLoad; }

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
		worldState.SetWSProperty(E_PropKey.E_IN_COMBAT_RANGE, true);
        worldState.SetWSProperty(E_PropKey.E_AT_TARGET_POS, true);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
        WorldStateProp prop = worldState.GetWSProperty(E_PropKey.E_IN_COMBAT_RANGE);
        WorldStateProp prop2 = worldState.GetWSProperty(E_PropKey.E_AT_TARGET_POS);
        if (prop.GetBool() == true && prop2.GetBool() == true)
            return true;

		return false;
	}

    public override bool IsSatisfied()
    {
        return base.IsPlanFinished();
    }

    public override bool IsPlanValid()
    {
        /*WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_IN_COMBAT_RANGE);

        if (prop.GetBool() == false)
            return false;*/

        if (Owner.BlackBoard.DistanceToTarget < 2)
            return false;

        return  base.IsPlanValid();
        
    }
}

