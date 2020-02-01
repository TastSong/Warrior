using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Agent))]
[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(AnimSetEnemyBossOrochi))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AnimComponent))]
[RequireComponent(typeof(SensorEyes))]


public class ComponentEnemyBossOrochi : MonoBehaviour, IActionHandle
{
	Agent Owner;
    private float StepTime;

    public Agent Agent { get { return Owner; } }
    

    void Awake()
    {
        Owner = GetComponent("Agent") as Agent;
    }

    void Start()
    {
        Agent.AddAction(E_GOAPAction.gotoMeleeRange);
        Agent.AddAction(E_GOAPAction.combatMoveForward);
        Agent.AddAction(E_GOAPAction.lookatTarget);
        Agent.AddAction(E_GOAPAction.attackBoss);
        Agent.AddAction(E_GOAPAction.attackMeleeOnce);
        Agent.AddAction(E_GOAPAction.playAnim);
        Agent.AddAction(E_GOAPAction.injuryOrochi);
        Agent.AddAction(E_GOAPAction.death);

		Agent.AddGoal(E_GOAPGoals.E_COMBAT_MOVE_FORWARD);
		Agent.AddGoal(E_GOAPGoals.E_LOOK_AT_TARGET);
		Agent.AddGoal(E_GOAPGoals.E_KILL_TARGET);
		//    Agent.AddGoal(E_GOAPGoals.E_PLAY_ANIM);
		Agent.AddGoal(E_GOAPGoals.E_REACT_TO_DAMAGE);

        Agent.InitializeGOAP();

		Owner.BlackBoard.AddActionHandle(this);
    }

    void LateUpdate()
    {
        if (StepTime < Time.timeSinceLevelLoad)
        {
            if (Owner.BlackBoard.MotionType == E_MotionType.Run)
            {
		Owner.Sound.PlayStep();
                StepTime = Time.timeSinceLevelLoad + Random.Range(0.25f, 0.28f);
            }
            else if (Owner.BlackBoard.MotionType == E_MotionType.Walk)
            {
                Owner.Sound.PlayStep();
                StepTime = Time.timeSinceLevelLoad + Random.Range(0.40f, 0.43f);
            }
        }
    }
    void FixedUpdate()
    {
        //Debug.Log("OROCHI Rage " + Owner.BlackBoard.Rage + " Fear " + Owner.BlackBoard.Fear + " Dodge " + Owner.BlackBoard.Dodge + " Berserk " + Owner.BlackBoard.Berserk);
        Owner.BlackBoard.UpdateCombatSetting();
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
        Owner.WorldState.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, true);
        Owner.WorldState.SetWSProperty(E_PropKey.E_IN_BLOCK, false);
        Owner.WorldState.SetWSProperty(E_PropKey.E_ALERTED, false);
        Owner.WorldState.SetWSProperty(E_PropKey.E_IN_COMBAT_RANGE, false);
        Owner.WorldState.SetWSProperty(E_PropKey.E_AHEAD_OF_ENEMY, false);
        Owner.WorldState.SetWSProperty(E_PropKey.E_BEHIND_ENEMY, false);
        Owner.WorldState.SetWSProperty(E_PropKey.MoveToRight, false);
        Owner.WorldState.SetWSProperty(E_PropKey.MoveToLeft, false);

        Owner.WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.None);

        StepTime = 0;
    }

    void Deactivate()
    {

    }
}
