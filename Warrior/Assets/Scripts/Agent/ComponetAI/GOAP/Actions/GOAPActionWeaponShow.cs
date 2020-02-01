using System;

class GOAPActionWeaponShow : GOAPAction
{
	private AgentActionWeaponShow Action;

	public GOAPActionWeaponShow(Agent owner): base(E_GOAPAction.weaponShow, owner){}


	public override void InitAction()
	{
		WorldEffects.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, true);
        Cost = 1;
        Interruptible = false;
        Precedence = 90;
	}


	public override void Activate()
	{
		base.Activate();

        Owner.BlackBoard.WeaponState = E_WeaponState.Ready;

		Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_WEAPON_SHOW) as AgentActionWeaponShow;
        Action.Show = true;
		Owner.BlackBoard.AddAction(Action);
	}

	public override void Deactivate()
	{
		base.Deactivate();

        Owner.WorldState.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, true);
	}

	public override bool IsActionComplete() 
	{
		if (Action.IsActive() == false)
			return true;

		return false; 
	}

}
