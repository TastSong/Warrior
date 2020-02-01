using UnityEngine;
using System.Collections;

// add enum
// add new  - in Initialize
// add if  - in DoAction

public class AnimFSMEnemyArcher: AnimFSM
{
	enum E_MyAnimState
	{
		idle,
		gotoPos,
        combatMove,
        Rotate,
		attack,
        playAnim,
        injury,
        knockdown,
        death,
	}

    public AnimFSMEnemyArcher(Animation anims, Agent owner) : base(anims, owner) 
    {
        AnimStates.Add(new AnimStateIdle(AnimEngine, Owner)); //E_MyAnimState.E_IDLE
        AnimStates.Add(new AnimStateGoTo(AnimEngine, Owner)); //E_MyAnimState.E_GOTO
        AnimStates.Add(new AnimStateCombatMove(AnimEngine, Owner)); //E_MyAnimState.combatMove
        AnimStates.Add(new AnimStateRotate(AnimEngine, Owner)); //E_MyAnimState.Rotate
        AnimStates.Add(new AnimStateAttackBow(AnimEngine, Owner)); //E_MyAnimState.attack
        AnimStates.Add(new AnimStatePlayAnim(AnimEngine, Owner)); //E_MyAnimState.E_PLAY_ANIM
        AnimStates.Add(new AnimStateInjury(AnimEngine, Owner)); //E_MyAnimState.E_INJURY
        AnimStates.Add(new AnimStateKnockdown(AnimEngine, Owner)); //E_MyAnimState.Knockdown
        AnimStates.Add(new AnimStateDeath(AnimEngine, Owner)); //E_MyAnimState._E_DEATH
    }

    public override void Initialize()
	{
  
		DefaultAnimState = AnimStates[(int)E_MyAnimState.idle];
		base.Initialize();
	}

	public override void DoAction(AgentAction action)
	{
		if (CurrentAnimState.HandleNewAction(action) == true)
		{
			//Debug.Log("AC - Do Action " + action.ToString());
			NextAnimState = null;
		}
		else
		{
            if (action is AgentActionGoTo)
                NextAnimState = AnimStates[(int)E_MyAnimState.gotoPos];
            if (action is AgentActioCombatMove)
                NextAnimState = AnimStates[(int)E_MyAnimState.combatMove];
            else if (action is AgentActionRotate)
                NextAnimState = AnimStates[(int)E_MyAnimState.Rotate];
            else if (action is AgentActionAttack)
                NextAnimState = AnimStates[(int)E_MyAnimState.attack];
            else if (action is AgentActionWeaponShow)
                NextAnimState = AnimStates[(int)E_MyAnimState.idle];
            else if (action is AgentActionPlayAnim || action is AgentActionPlayIdleAnim)
                NextAnimState = AnimStates[(int)E_MyAnimState.playAnim];
            else if (action is AgentActionInjury)
                NextAnimState = AnimStates[(int)E_MyAnimState.injury];
            else if (action is AgentActionKnockdown)
                NextAnimState = AnimStates[(int)E_MyAnimState.knockdown];
            else if (action is AgentActionDeath)
                NextAnimState = AnimStates[(int)E_MyAnimState.death];


            ProgressToNextStage(action);

		}
	}
}