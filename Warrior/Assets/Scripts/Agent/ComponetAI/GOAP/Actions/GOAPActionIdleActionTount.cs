using System;

class GOAPActionIdleActionTount : GOAPAction
{
    private AgentActionPlayIdleAnim Action;

    public GOAPActionIdleActionTount(Agent owner) : base(E_GOAPAction.tount, owner) { }


	public override void InitAction()
	{
		WorldEffects.SetWSProperty(E_PropKey.E_IDLING, false);

        Interruptible = false;

        Cost = 1;
	}


	public override void Activate()
	{
		base.Activate();

        Owner.BlackBoard.WeaponState = E_WeaponState.Ready;

        Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_PLAY_IDLE_ANIM) as AgentActionPlayIdleAnim;
		Owner.BlackBoard.AddAction(Action);
	}

	public override void Deactivate()
	{
		base.Deactivate();

        Owner.WorldState.SetWSProperty(E_PropKey.E_IDLING, true);
	}

	public override bool IsActionComplete() 
	{
		if (Action.IsActive() == false)
			return true;

		return false; 
	}

}
