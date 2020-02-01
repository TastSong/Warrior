using System;

class GOAPActionTeleport : GOAPAction
{
    private AgentActionTeleport Action;

    public GOAPActionTeleport(Agent owner) : base(E_GOAPAction.teleport, owner) { }


	public override void InitAction()
	{
		WorldEffects.SetWSProperty(E_PropKey.E_TELEPORT, false);
        Cost = 1;
        Interruptible = false;
        Precedence = 90;
	}


	public override void Activate()
	{
		base.Activate();

		Action = AgentActionFactory.Create(AgentActionFactory.E_Type.Teleport) as AgentActionTeleport;
        Action.Teleport = Owner.BlackBoard.TeleportDestination;
        Action.FadeGui = true;

		Owner.BlackBoard.AddAction(Action);

        Owner.BlackBoard.Stop = true;
	}

	public override void Deactivate()
	{
		base.Deactivate();

        Owner.WorldState.SetWSProperty(E_PropKey.E_TELEPORT, false);
        Owner.WorldState.SetWSProperty(E_PropKey.E_AT_TARGET_POS, true);

        Owner.BlackBoard.Stop = false;
	}

	public override bool IsActionComplete() 
	{
		if (Action.IsActive() == false)
			return true;

		return false; 
	}

}
