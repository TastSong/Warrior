using System;
using UnityEngine;


// activate goal when 
// E_PropKey.E_ENEMY_IN_SIGHT == true



//战斗中向左移动
//敌人在视野内 危险的
class GOAPGoalCombatMoveToLeft : GOAPGoal
{
    public GOAPGoalCombatMoveToLeft(Agent owner) : base(E_GOAPGoals.E_COMBAT_MOVE_LEFT, owner) { }

	public override void InitGoal()
	{
	}

    public override float GetMaxRelevancy()
    {
        return Owner.BlackBoard.GOAP_CombatMoveLeftRelevancy;
    }

	public override void CalculateGoalRelevancy()
	{
        GoalRelevancy = 0;

        if (Owner.BlackBoard.DesiredTarget == null)
            return;

        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_ALERTED);
        WorldStateProp prop2 = Owner.WorldState.GetWSProperty(E_PropKey.E_LOOKING_AT_TARGET);

		if (prop.GetBool() == true && prop2.GetBool() == true && MissionZone.Instance.CurrentGameZone.GetNearestAliveEnemy(Owner, E_Direction.Right, 0.4f) != null)
        {
            if (isGroundThere(Owner.Position + Owner.Right * -2.0f))//left
                GoalRelevancy = Owner.BlackBoard.GOAP_CombatMoveLeftRelevancy;// * Mathfx.Hermite(0, 1, Mathf.Min(2, Owner.BlackBoard.DistanceToTarget / 4.0f));
        }
	}

    public override void SetDisableTime() { NextEvaluationTime = Owner.BlackBoard.GOAP_CombatMoveLeftDelay + Time.timeSinceLevelLoad; }

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
        worldState.SetWSProperty(E_PropKey.MoveToLeft, true);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
        WorldStateProp prop = worldState.GetWSProperty(E_PropKey.MoveToLeft);
        if (prop != null && prop.GetBool() == true)
            return true;

		return false;
	}

    public override bool IsSatisfied()
    {
       return IsPlanFinished();
    }
}

