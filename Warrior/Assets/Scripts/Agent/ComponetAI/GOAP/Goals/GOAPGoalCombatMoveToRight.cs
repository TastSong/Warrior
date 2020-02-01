using System;
using UnityEngine;


// activate goal when 
// E_PropKey.E_ENEMY_IN_SIGHT == true
// E_PropKey.E_IN_WEAPONS_RANGE == false


//战斗中向右移动
//敌人在视野内 危险的 不在武器范围内
class GOAPGoalCombatMoveToRight : GOAPGoal
{
    public GOAPGoalCombatMoveToRight(Agent owner) : base(E_GOAPGoals.E_COMBAT_MOVE_RIGHT, owner) { }

	public override void InitGoal()
	{

	}

    public override float GetMaxRelevancy()
    {
        return Owner.BlackBoard.GOAP_CombatMoveRightRelevancy;
    }
	public override void CalculateGoalRelevancy()
	{
        GoalRelevancy = 0;

        if (Owner.BlackBoard.DesiredTarget == null)
            return;

        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_ALERTED);
        WorldStateProp prop2 = Owner.WorldState.GetWSProperty(E_PropKey.E_LOOKING_AT_TARGET);

		if (prop.GetBool() == true && prop2.GetBool() == true && MissionZone.Instance.CurrentGameZone.GetNearestAliveEnemy(Owner, E_Direction.Left, 0.4f) != null)
        {
            if (isGroundThere(Owner.Position + Owner.Right * 2.0f))//left
                GoalRelevancy = Owner.BlackBoard.GOAP_CombatMoveRightRelevancy;// * Mathfx.Hermite(0, 1, Mathf.Min(2, Owner.BlackBoard.DistanceToTarget / 4.0f));
            else
                GoalRelevancy = 0;
        }
        else
            GoalRelevancy = 0;
	}

    public override void SetDisableTime() { NextEvaluationTime = Owner.BlackBoard.GOAP_CombatMoveRightDelay + Time.timeSinceLevelLoad; }

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
        worldState.SetWSProperty(E_PropKey.MoveToRight, true);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
        WorldStateProp prop = worldState.GetWSProperty(E_PropKey.MoveToRight);
        if (prop != null && prop.GetBool() == true)
            return true;

		return false;
	}

    public override bool IsSatisfied()
    {
        return IsPlanFinished();
    }
}

