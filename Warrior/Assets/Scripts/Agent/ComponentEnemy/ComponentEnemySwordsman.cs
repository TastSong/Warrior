using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Agent))]
[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(AnimSetEnemySwordsMan))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AnimComponent))]
[RequireComponent(typeof(SensorEyes))]

public class ComponentEnemySwordsman : MonoBehaviour, IActionHandle
{
	Agent Owner;

    public Agent Agent { get { return Owner; } }

    void Awake()
    {
        Owner = GetComponent("Agent") as Agent;
    }

    void Start()
    {
        // Agent.AddGOAPAction(E_GOAPAction.E_GOTO);
        Agent.AddAction(E_GOAPAction.gotoMeleeRange);
        Agent.AddAction(E_GOAPAction.combatMoveRight);
        Agent.AddAction(E_GOAPAction.combatMoveLeft);
        Agent.AddAction(E_GOAPAction.combatMoveForward);
        Agent.AddAction(E_GOAPAction.combatMoveBackward);
        Agent.AddAction(E_GOAPAction.combatRunForward);
        Agent.AddAction(E_GOAPAction.combatRunBackward);
        Agent.AddAction(E_GOAPAction.lookatTarget);
        Agent.AddAction(E_GOAPAction.weaponShow);
        Agent.AddAction(E_GOAPAction.weaponHide);
        Agent.AddAction(E_GOAPAction.attackMeleeOnce);
        Agent.AddAction(E_GOAPAction.attackCounter);
        Agent.AddAction(E_GOAPAction.block);
        Agent.AddAction(E_GOAPAction.playAnim);
        Agent.AddAction(E_GOAPAction.tount);
        Agent.AddAction(E_GOAPAction.injury);
        Agent.AddAction(E_GOAPAction.knockdown);
        Agent.AddAction(E_GOAPAction.death);

		//Agent.AddGoal(E_GOAPGoals.E_GOTO);
		Agent.AddGoal(E_GOAPGoals.E_COMBAT_MOVE_RIGHT);
		Agent.AddGoal(E_GOAPGoals.E_COMBAT_MOVE_LEFT);
		Agent.AddGoal(E_GOAPGoals.E_COMBAT_MOVE_FORWARD);
		Agent.AddGoal(E_GOAPGoals.E_COMBAT_MOVE_BACKWARD);
		Agent.AddGoal(E_GOAPGoals.E_LOOK_AT_TARGET);
		Agent.AddGoal(E_GOAPGoals.E_KILL_TARGET);
		Agent.AddGoal(E_GOAPGoals.E_DO_BLOCK);
		Agent.AddGoal(E_GOAPGoals.E_ALERT);
		Agent.AddGoal(E_GOAPGoals.E_CALM);
		Agent.AddGoal(E_GOAPGoals.E_USE_WORLD_OBJECT);
		Agent.AddGoal(E_GOAPGoals.E_PLAY_ANIM);
		Agent.AddGoal(E_GOAPGoals.E_IDLE_ANIM);
		Agent.AddGoal(E_GOAPGoals.E_REACT_TO_DAMAGE);

        Agent.InitializeGOAP();

		Owner.BlackBoard.AddActionHandle(this);
        //Owner.debugGOAP = true;
        //Owner.debugAnims = true;
    }

    void FixedUpdate()
    {
        //Debug.Log("SWORD Rage " + Owner.BlackBoard.Rage + " Fear " + Owner.BlackBoard.Fear + " Dodge " + Owner.BlackBoard.Dodge + " Berserk " + Owner.BlackBoard.Berserk);

        Agent.BlackBoard.UpdateCombatSetting();
    }

    public void HandleAction(AgentAction a)
    {
        Owner.BlackBoard.UpdateCombatSetting(a);
    }


    public void StopMove(bool stop)
    { 
    
    }


    void Activate(Transform t)
    {
        Owner.BlackBoard.Rage = Owner.BlackBoard.RageMin;

        Owner.WorldState.SetWSProperty(E_PropKey.E_ORDER, AgentOrder.E_OrderType.E_NONE);

        Owner.WorldState.SetWSProperty(E_PropKey.E_IDLING, true);
        Owner.WorldState.SetWSProperty(E_PropKey.E_AT_TARGET_POS, true);
        Owner.WorldState.SetWSProperty(E_PropKey.E_ATTACK_TARGET, false);
        Owner.WorldState.SetWSProperty(E_PropKey.E_LOOKING_AT_TARGET, false);
        Owner.WorldState.SetWSProperty(E_PropKey.E_USE_WORLD_OBJECT, false);
        Owner.WorldState.SetWSProperty(E_PropKey.E_PLAY_ANIM, false);

        Owner.WorldState.SetWSProperty(E_PropKey.E_IN_DODGE, false);
        Owner.WorldState.SetWSProperty(E_PropKey.E_IN_WEAPONS_RANGE, false);
        Owner.WorldState.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, false);
        Owner.WorldState.SetWSProperty(E_PropKey.E_IN_BLOCK, false);
        Owner.WorldState.SetWSProperty(E_PropKey.E_ALERTED, false);
        Owner.WorldState.SetWSProperty(E_PropKey.E_IN_COMBAT_RANGE, false);
        Owner.WorldState.SetWSProperty(E_PropKey.E_AHEAD_OF_ENEMY, false);
        Owner.WorldState.SetWSProperty(E_PropKey.E_BEHIND_ENEMY, false);
        Owner.WorldState.SetWSProperty(E_PropKey.MoveToRight, false);
        Owner.WorldState.SetWSProperty(E_PropKey.MoveToLeft, false);

        Owner.WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.None);

    }

    void Deactivate()
    {

    }
}
