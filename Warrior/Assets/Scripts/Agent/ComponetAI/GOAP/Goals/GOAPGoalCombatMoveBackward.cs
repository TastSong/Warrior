using System;
using UnityEngine;


// activate goal when 
// E_PropKey.E_ENEMY_IN_SIGHT == true
// E_PropKey.E_IN_WEAPONS_RANGE == false

/******************************************************************
 * GOAL STATS
 * 
 * For initialize : 
 *                  E_PropKey.E_IN_COMBAT_RANGE     true
 *                  E_PropKey.E_ALERTED             true    
 * For planing : 
 *                  E_PropKey.E_WEAPON_IN_HANDS     false
 * Set : 
 *                  
 * Finished:
 *                  E_WEAPON_IN_HANDS               false
 * 
 * ***************************************************************/

//战斗向后移动
//在战斗范围，危险的  武器离手 武器离手
class GOAPGoalCombatMoveBackward : GOAPGoal
{
    public GOAPGoalCombatMoveBackward(Agent owner) : base(E_GOAPGoals.E_COMBAT_MOVE_BACKWARD, owner) { }

	public override void InitGoal()
	{
	}

    public override float GetMaxRelevancy()
    {
        return Owner.BlackBoard.GOAP_CombatMoveBackwardRelevancy;
    }

	public override void CalculateGoalRelevancy()
	{
        GoalRelevancy = 0;

        if (Owner.BlackBoard.DesiredTarget == null)
            return;

        if (Owner.BlackBoard.Fear < Owner.BlackBoard.FearMax * 0.25f)
            return;

		if (MissionZone.Instance.CurrentGameZone.GetNearestAliveEnemy(Owner, E_Direction.Backward, 0.4f) != null)
            return;


        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_ALERTED);
        WorldStateProp prop2 = Owner.WorldState.GetWSProperty(E_PropKey.E_IN_COMBAT_RANGE);

        if (prop.GetBool() != true || prop2.GetBool() != true)
            return;

        if (isGroundThere(Owner.Position - Owner.Forward * 2.0f) == false)
            return;
            
        
        GoalRelevancy = Owner.BlackBoard.GOAP_CombatMoveBackwardRelevancy * (Owner.BlackBoard.Fear * 0.01f);

        if (Owner.BlackBoard.DistanceToTarget < 2.0f)
            GoalRelevancy += 0.2f;

        if (isGroundThere(Owner.Position - Owner.Forward * 4.0f) == false)
            GoalRelevancy *= 0.5f;

        WorldStateProp prop3 = Owner.WorldState.GetWSProperty(E_PropKey.E_AHEAD_OF_ENEMY);
        if(prop3.GetBool() == false)
            GoalRelevancy *= 0.5f;

        if (GoalRelevancy > Owner.BlackBoard.GOAP_CombatMoveBackwardRelevancy)
            GoalRelevancy = Owner.BlackBoard.GOAP_CombatMoveBackwardRelevancy;
	}

    public override void SetDisableTime() { NextEvaluationTime = Owner.BlackBoard.GOAP_CombatMoveBackwardDelay + Time.timeSinceLevelLoad; }

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
		worldState.SetWSProperty(E_PropKey.E_IN_COMBAT_RANGE, false);
        worldState.SetWSProperty(E_PropKey.E_AT_TARGET_POS, true);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
        WorldStateProp prop = worldState.GetWSProperty(E_PropKey.E_IN_COMBAT_RANGE);
        WorldStateProp prop2 = worldState.GetWSProperty(E_PropKey.E_AT_TARGET_POS);
        if (prop.GetBool() == false &&  prop2.GetBool() == true)
            return true;


		return false;
	}

    public override bool IsSatisfied()
    {
        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_IN_COMBAT_RANGE);
        WorldStateProp prop2 = Owner.WorldState.GetWSProperty(E_PropKey.E_AT_TARGET_POS);
        if (prop != null && prop.GetBool() == false && prop2 != null && prop2.GetBool() == true)
            return true;

        return false;
    }
}

