using System;
using UnityEngine;

//E_PropKey.E_AT_TARGET_POS
class GOAPGoalKillTarget : GOAPGoal
{
	public GOAPGoalKillTarget(Agent owner) : base(E_GOAPGoals.E_KILL_TARGET, owner) { }

	public override void InitGoal()
	{

	}

    public override float GetMaxRelevancy()
    {
        return Owner.BlackBoard.GOAP_KillTargetRelevancy;
    }

	public override void CalculateGoalRelevancy()
	{
        GoalRelevancy = 0;

        if (Game.Instance.GameDifficulty == E_GameDifficulty.Easy)
        {
            if (MissionBlackBoard.Instance.LastAttackTime > Time.timeSinceLevelLoad + Owner.BlackBoard.DontAttackTimer * 1.5f)
                return;
        }
        else if (MissionBlackBoard.Instance.LastAttackTime > Time.timeSinceLevelLoad + Owner.BlackBoard.DontAttackTimer)
            return;

        float attackValue = 0;
        
        if(Owner.BlackBoard.RageMax > 0)
            attackValue = Owner.BlackBoard.Rage / Owner.BlackBoard.RageMax;
        
        if(Owner.BlackBoard.BerserkMax > 0)
            attackValue = Mathf.Max(attackValue, Owner.BlackBoard.Berserk / Owner.BlackBoard.BerserkMax);
        
        if (attackValue < 0.25f)
            return;

        if (Owner.WorldState.GetWSProperty(E_PropKey.E_IN_COMBAT_RANGE).GetBool() == false)
            return;

		if (Owner.BlackBoard.WeaponRange < 3 && MissionZone.Instance.CurrentGameZone.GetNearestAliveEnemy(Owner, E_Direction.Forward, 0.4f) != null)
            return;

        if (Owner.WorldState.GetWSProperty(E_PropKey.E_ATTACK_TARGET).GetBool() == true)
            GoalRelevancy = Owner.BlackBoard.GOAP_KillTargetRelevancy * attackValue;
	}

    public override void SetDisableTime() { NextEvaluationTime = Owner.BlackBoard.GOAP_KillTargetDelay + Time.timeSinceLevelLoad; }

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
		worldState.SetWSProperty(E_PropKey.E_ATTACK_TARGET, false);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
		WorldStateProp prop = worldState.GetWSProperty(E_PropKey.E_ATTACK_TARGET);

		if (prop != null && prop.GetBool() == false)
			return true;

		return false;
	}

    public override bool IsSatisfied()
    {
        return IsPlanFinished();
    }

    public override bool Activate(GOAPPlan plan)
    {
        GoalRelevancy = Owner.BlackBoard.GOAP_KillTargetRelevancy;

		Owner.Sound.PlayPrepareAttack();

        MissionBlackBoard.Instance.LastAttackTime = Time.timeSinceLevelLoad;

        return base.Activate(plan);
    }
}

