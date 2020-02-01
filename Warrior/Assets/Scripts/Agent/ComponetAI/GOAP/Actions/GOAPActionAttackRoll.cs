using System;

class GOAPActionAttackRoll: GOAPAction
{
    private AgentActionAttackRoll  Action;


    public GOAPActionAttackRoll(Agent owner) : base(E_GOAPAction.attackRoll, owner) { }


	public override void InitAction()
	{
        WorldPreconditions.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, true);
        WorldPreconditions.SetWSProperty(E_PropKey.E_LOOKING_AT_TARGET, true);          
		WorldEffects.SetWSProperty(E_PropKey.E_ATTACK_TARGET, false);

        Interruptible = false;

        Cost = 1;
        Precedence = 80;
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

        SendAction();
	}

	public override void Deactivate()
	{
        Owner.BlackBoard.Invulnerable = false;

		base.Deactivate();
	}


    public override void Update()
    {
    }

	public override bool IsActionComplete() 
	{
        if (Action != null && Action.IsActive() == false)
			return true;

		return false; 
	}

    void SendAction()
    {
        Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_ATTACK_ROLL) as AgentActionAttackRoll;
        Action.Data = Owner.AnimSet.GetRollAttackAnim();
        Action.Target = Owner.BlackBoard.DesiredTarget;
        // Debug.Log("action attack  " + (Action.AttackTarget != null ? Action.AttackTarget.name : "no target"));

        Owner.BlackBoard.AddAction(Action);
    }

}
