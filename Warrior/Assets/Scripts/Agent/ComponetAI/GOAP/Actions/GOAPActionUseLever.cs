using System;

class GOAPActionUseLever : GOAPAction
{
	private AgentActionUseLever Action;

	public GOAPActionUseLever(Agent owner) : base(E_GOAPAction.useLever, owner) { }


	public override void InitAction()
	{
		WorldPreconditions.SetWSProperty(E_PropKey.E_AT_TARGET_POS, true);
        WorldPreconditions.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, false);
		WorldEffects.SetWSProperty(E_PropKey.E_USE_WORLD_OBJECT, false);

        Interruptible = false;

        Cost = 2;
	}


	public override void Activate()
	{
		base.Activate();

        Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_USE_LEVER) as AgentActionUseLever;
        Action.Interaction = Owner.BlackBoard.Interaction;
        Action.InterObj = Owner.BlackBoard.InteractionObject;

		Owner.BlackBoard.AddAction(Action);

        Owner.BlackBoard.Stop = true;
	}

	public override void Deactivate()
	{
        Owner.BlackBoard.Stop = false;
		base.Deactivate();

        Owner.WorldState.SetWSProperty(E_PropKey.E_USE_WORLD_OBJECT, false);
        Owner.WorldState.SetWSProperty(E_PropKey.E_AT_TARGET_POS, true);
	}

	public override bool IsActionComplete() 
	{
        if (Action.IsActive() == false)
			return true;

		return false; 
	}

}
