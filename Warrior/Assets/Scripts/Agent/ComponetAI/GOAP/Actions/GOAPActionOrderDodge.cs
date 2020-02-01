using System;
using UnityEngine;

class GOAPActioOrderDodge : GOAPAction
{
	private AgentActionRoll Action;

    public GOAPActioOrderDodge(Agent owner) : base(E_GOAPAction.orderDodge, owner) { }

	public override void InitAction()
	{
        WorldEffects.SetWSProperty(E_PropKey.E_IN_DODGE, true);
        Cost = 2;
	}

	public override void Activate()
	{   
		base.Activate();

        WorldEffects.SetWSProperty(E_PropKey.E_IN_DODGE, true);

        ActionRollTo();
	}

	public override void Deactivate()
	{
        Action = null;
        Owner.WorldState.SetWSProperty(E_PropKey.E_IN_DODGE, false);

		base.Deactivate();
	}

    public override void Update()
    {
        if (Action != null && Action.IsActive())
            return;

        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_ORDER);
        if (prop == null || prop.GetOrder() == AgentOrder.E_OrderType.E_DODGE)
        {
            //pokud akce skoro skoncila a my uz mame informace o nove, tak vytvorime dalsi utok a poslem ho
            ActionRollTo();
        }
    }
	public override bool IsActionComplete() 
	{
		if (Action != null && Action.IsActive() == false)
		{
			//UnityEngine.Debug.Log(this.ToString() + " complete !");
			return true;
		}

		return false; 
	}

	public override bool ValidateAction()
	{
	
        if(Action != null && Action.IsFailed() == true)
		{
            //UnityEngine.Debug.Log(this.ToString() + " not valid anymore !" + Action.Direction.ToString());
			return false;
		}

		return true;
	}

	private void ActionRollTo()
	{
        Action = null;

 		Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_ROLL) as AgentActionRoll;
        Action.Direction = Owner.BlackBoard.DesiredDirection;
		Owner.BlackBoard.AddAction(Action);

		Owner.Sound.PlayRoll();

        if (Owner.WorldState.GetWSProperty(E_PropKey.E_ORDER).GetOrder() == AgentOrder.E_OrderType.E_DODGE)
            Owner.WorldState.SetWSProperty(E_PropKey.E_ORDER, AgentOrder.E_OrderType.E_NONE);
        //UnityEngine.Debug.Log(this.ToString() + "Send new roll action to pos " + Action.Direction.ToString());
	}
}
