using System;
using System.Collections.Generic;
using UnityEngine;

class GOAPActionOrderAttack : GOAPAction
{
    private AgentActionAttack Action;
    private Agent LastAttacketTarget = null;

    public GOAPActionOrderAttack(Agent owner) : base(E_GOAPAction.orderAttack, owner) { }


    public override void InitAction()
    {
        WorldPreconditions.SetWSProperty(E_PropKey.E_IN_WEAPONS_RANGE, true);
        WorldPreconditions.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, true);
        WorldEffects.SetWSProperty(E_PropKey.E_ATTACK_TARGET, true);

        Cost = 4;
    }


    public override void Activate()
    {
        base.Activate();
       // Owner.WorldState.SetWSProperty(E_PropKey.E_AT_TARGET_POS, true);
        DoAttackAction();
    }

    public override void Deactivate()
    {
        
       // Owner.WorldState.SetWSProperty(E_PropKey.E_IN_WEAPONS_RANGE, false);
        LastAttacketTarget = null;

        base.Deactivate();
    }

    public override bool IsActionComplete()
    {
        if (Owner.WorldState.GetWSProperty(E_PropKey.E_ORDER).GetOrder() == AgentOrder.E_OrderType.E_ATTACK && Action.AttackPhaseDone)
        {// pokud uz ceka novy utok a je to pokracovani stavajiciho komba, tak skoncime o torchu drive !!
            if (Owner.BlackBoard.DesiredAttackPhase.FirstAttackInCombo == false)
            {
               //if (Owner.debugGOAP) Debug.Log(Time.timeSinceLevelLoad + " " + this.ToString() + " - IsActionComplete, next attack is waiting"); 
                return true;
            }
        }

        if (Action.IsActive() == false )
            return true;

        return false;
    }

    public override void Update()
    {
        /*WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_ORDER);
        if (prop == null || prop.GetOrder() == AgentOrder.E_OrderType.E_ATTACK)
        {
            //pokud akce skoro skoncila a my uz mame informace o nove, tak vytvorime dalsi utok a poslem ho
            DoAttackAction();
        }*/
    }

    void DoAttackAction()
    {
        Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_ATTACK) as AgentActionAttack;
	Owner.BlackBoard.DesiredTarget = Action.Target = MissionZone.Instance.GetBestTarget(Owner, LastAttacketTarget);
        Action.AttackType = Owner.BlackBoard.DesiredAttackType;
        Action.AttackDir = Owner.BlackBoard.DesiredDirection;
        Action.Data = Owner.BlackBoard.DesiredAttackPhase;

        if (Action.Target != null)
        {
            if (Action.Target.BlackBoard.MotionType == E_MotionType.Knockdown)
            {
                Action.Data = Owner.AnimSet.GetFirstAttackAnim(Owner.BlackBoard.WeaponSelected, E_AttackType.Fatality);
                Action.AttackType = E_AttackType.Fatality;
            }
        }

		if (Action.Data !=null && ( Action.Data.FullCombo || Action.AttackType == E_AttackType.Fatality))
			Owner.Sound.PlayBerserk();
        else if (UnityEngine.Random.Range(0, 100) < 20)
			Owner.Sound.PlayPrepareAttack();

        Action.Hit = false;
        Action.AttackPhaseDone = false;

		//派发数据到FSM里面
        Owner.BlackBoard.AddAction(Action);

        if (Owner.WorldState.GetWSProperty(E_PropKey.E_ORDER).GetOrder() == AgentOrder.E_OrderType.E_ATTACK)
            Owner.WorldState.SetWSProperty(E_PropKey.E_ORDER, AgentOrder.E_OrderType.E_NONE);

        LastAttacketTarget = Action.Target;
    }


}
