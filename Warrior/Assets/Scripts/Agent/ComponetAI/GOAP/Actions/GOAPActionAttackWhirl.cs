using System;
using UnityEngine;

class GOAPActionAttackWhirl : GOAPAction
{
	private AgentActionAttackWhirl Action;

    public GOAPActionAttackWhirl(Agent owner) : base(E_GOAPAction.attackWhirl, owner) { }


	public override void InitAction()
	{
		WorldPreconditions.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, true);
        WorldPreconditions.SetWSProperty(E_PropKey.E_LOOKING_AT_TARGET, true);
		WorldEffects.SetWSProperty(E_PropKey.E_ATTACK_TARGET, false);

        Precedence = 80;
        Interruptible = false;
        Cost = 2;
	}

    // Validates the context preconditions
    public override bool ValidateContextPreconditions(Agent ai)
    {
        if (Owner.BlackBoard.Berserk > Owner.BlackBoard.BerserkMax * 0.75f)
            return true;

        return false;
    }

	public override void Activate()
	{
		base.Activate();

        Owner.BlackBoard.Invulnerable = true;

        DoAttackAction();
	}

	public override void Deactivate()
	{
        Owner.BlackBoard.Invulnerable = false;

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
        Action = null;

        Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_ATTACK_WHIRL) as AgentActionAttackWhirl;
        Action.Data = Owner.AnimSet.GetWhirlAttackAnim();
        Owner.BlackBoard.AddAction(Action);
    }

}
