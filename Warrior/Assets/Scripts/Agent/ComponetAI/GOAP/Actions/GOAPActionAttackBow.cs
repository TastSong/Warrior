using System;
using UnityEngine;

class GOAPActionAttackBow : GOAPAction
{
	private AgentActionAttack Action;

    public GOAPActionAttackBow(Agent owner) : base(E_GOAPAction.attackBow, owner) { }


	public override void InitAction()
	{
        WorldPreconditions.SetWSProperty(E_PropKey.E_IN_WEAPONS_RANGE, true);
		WorldPreconditions.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, true);
        WorldPreconditions.SetWSProperty(E_PropKey.E_LOOKING_AT_TARGET, true);
		WorldEffects.SetWSProperty(E_PropKey.E_ATTACK_TARGET, false);

        Interruptible = false;

        Cost = 2;
	}


	public override void Activate()
	{
		base.Activate();

        DoAttackAction();
	}

	public override void Deactivate()
	{
        base.Deactivate();

       /* WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_ATTACK_TARGET);
        if (prop == null || prop.GetBool() == false)
            return;

        if (WorldStateTime == prop.Time)
            Owner.WorldState.SetWSProperty(E_PropKey.E_ATTACK_TARGET, false);
        */
	}

	public override bool IsActionComplete() 
	{
		if (Action.IsActive() == false)
			return true;

		return false; 
	}

    public override void Update()
    {
    }

    void DoAttackAction()
    {
        Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_ATTACK) as AgentActionAttack;
        Action.Target = Owner.BlackBoard.DesiredTarget != null && Owner.BlackBoard.DesiredTarget.IsAlive ? Owner.BlackBoard.DesiredTarget : null;
        Action.AttackType = E_AttackType.X;
        Action.AttackDir = Owner.BlackBoard.DesiredDirection;


        Action.Hit = false;
        Action.AttackPhaseDone = false;

       // Debug.Log("action attack  " + (Action.AttackTarget != null ? Action.AttackTarget.name : "no target"));

		Owner.BlackBoard.AddAction(Action);
        
    }

}
