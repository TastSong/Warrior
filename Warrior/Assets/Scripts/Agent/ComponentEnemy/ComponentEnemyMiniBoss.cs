using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Agent))]
[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(AnimSetEnemyMiniBoss))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AnimComponent))]
[RequireComponent(typeof(SensorEyes))]


public class ComponentEnemyMiniBoss : MonoBehaviour, IActionHandle
{
    public AudioClip ScreamSound;

	Agent Owner;
    AudioSource Audio;

    public Agent Agent { get { return Owner; } }

    void Awake()
    {
        Owner = GetComponent("Agent") as Agent;
        Audio = GetComponent<AudioSource>();
        }

    void Start()
    {
        // Agent.AddGOAPAction(E_GOAPAction.E_GOTO);
        Agent.AddAction(E_GOAPAction.gotoMeleeRange);
        Agent.AddAction(E_GOAPAction.combatMoveForward);
        Agent.AddAction(E_GOAPAction.lookatTarget);
        Agent.AddAction(E_GOAPAction.attackMeleeOnce);
        Agent.AddAction(E_GOAPAction.attackRoll);
        //     Agent.AddGOAPAction(E_GOAPAction.block);
        Agent.AddAction(E_GOAPAction.playAnim);
        Agent.AddAction(E_GOAPAction.injury);
        Agent.AddAction(E_GOAPAction.death);

		Agent.AddGoal(E_GOAPGoals.E_COMBAT_MOVE_FORWARD);
		Agent.AddGoal(E_GOAPGoals.E_LOOK_AT_TARGET);
		Agent.AddGoal(E_GOAPGoals.E_KILL_TARGET);
        //   Agent.AddGOAPGoal(E_GOAPGoals.E_DO_BLOCK);
		Agent.AddGoal(E_GOAPGoals.E_PLAY_ANIM);
		Agent.AddGoal(E_GOAPGoals.E_REACT_TO_DAMAGE);

        Agent.InitializeGOAP();

		Owner.BlackBoard.AddActionHandle(this);
    }

    void FixedUpdate()
    {
        Owner.BlackBoard.UpdateCombatSetting();

        // Debug.Log("MINI  Rage " + Owner.BlackBoard.Rage + " Fear " + Owner.BlackBoard.Fear + " Dodge " + Owner.BlackBoard.Dodge + " Berserk " + Owner.BlackBoard.Berserk);
    }

    public void HandleAction(AgentAction a)
    {
        Owner.BlackBoard.UpdateCombatSetting(a);
        
        if (a is AgentActionPlayIdleAnim)
        {
            Audio.PlayOneShot(ScreamSound);
        }
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
        Owner.WorldState.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, true);
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

    /*void OnControllerColliderHit(ControllerColliderHit hit)
    {

    }*/
}
