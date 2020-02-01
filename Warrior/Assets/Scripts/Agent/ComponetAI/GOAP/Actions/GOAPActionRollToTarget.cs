using System;
using UnityEngine;

class GOAPActionRollToTarget : GOAPAction
{
	private AgentActionRoll Action;

    public GOAPActionRollToTarget(Agent owner) : base(E_GOAPAction.rollToTarget, owner) { }


	public override void InitAction()
	{
		WorldEffects.SetWSProperty(E_PropKey.E_IN_WEAPONS_RANGE, true);
        
        Cost = 2;
        Precedence = 80;
	}


    public override bool ValidateContextPreconditions(Agent ai)
    {
        Agent a = Owner.BlackBoard.DesiredTarget;

        if (a == null)
            return false;

        float dist = (a.Position - Owner.Position).sqrMagnitude;

        if (dist < 5 * 5 )
            return false;

        return true;
    }

	public override void Activate()
	{   
		base.Activate();

        ActionRollTo();

	}

	public override void Deactivate()
	{
        if ((Owner.Position - Owner.BlackBoard.DesiredTarget.Position).sqrMagnitude < Owner.BlackBoard.WeaponRange * Owner.BlackBoard.WeaponRange)
            Owner.WorldState.SetWSProperty(E_PropKey.E_IN_WEAPONS_RANGE, true);

		base.Deactivate();
	}

	public override bool IsActionComplete() 
	{
		if (Action != null && Action.IsActive() == false)
			return true;

		return false; 
	}

	public override bool ValidateAction()
	{
	
        if(Action != null && Action.IsFailed() == true)
			return false;

		return true;
	}

	private void ActionRollTo()
	{
 		Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_ROLL) as AgentActionRoll;
        Action.Direction = Owner.BlackBoard.DesiredTarget.Position - Owner.Position;

//        if (Action.Direction.sqrMagnitude < (Owner.BlackBoard.RollDistance + 2)  * (Owner.BlackBoard.RollDistance + 2))
            Action.ToTarget = Owner.BlackBoard.DesiredTarget;

        Action.Direction.Normalize(); 
		Owner.BlackBoard.AddAction(Action);
	}
}
