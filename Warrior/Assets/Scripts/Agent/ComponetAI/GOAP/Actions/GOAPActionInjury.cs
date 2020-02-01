using System;
using UnityEngine;

class GOAPActionInjury : GOAPAction
{
    private AgentActionInjury Action;
    Fact Query;
    private float WorldStateTime = 0;

    public GOAPActionInjury(Agent owner) : base(E_GOAPAction.injury, owner) { }


	public override void InitAction()
	{
		WorldEffects.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.None);

		Interruptible = false;

        Cost = 1;
        Precedence = 100;

        Query = FactsFactory.Create(Fact.E_FactType.E_EVENT);
        Query.EventType = E_EventTypes.Hit;
	}

    public override bool ValidateContextPreconditions(Agent ai)
    {
        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_EVENT);

        if(prop == null || prop.GetEvent() !=  E_EventTypes.Hit)
            return false;

        if(Owner.IsAlive == false)
            return false;

        return true;
    }

	public override void Activate()
	{   
		base.Activate();
        
        Action = null;
        Interruptible = false;

        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_EVENT);
        if (prop == null || prop.GetEvent() != E_EventTypes.Hit)
            return;

        WorldStateTime = prop.Time;

        SendAction();

	}

	public override void Deactivate()
	{
        Action = null;
		base.Deactivate();

        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_EVENT);
        if (prop == null || prop.GetEvent() != E_EventTypes.ImInPain)
            return;

        if (WorldStateTime == prop.Time)
            Owner.WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.None);
	}

    public override void Update()
    {
        if (Owner.IsAlive == false)
            Interruptible = true;


        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_EVENT);
        if (prop == null || prop.GetEvent() != E_EventTypes.Hit || Owner.IsAlive == false)
            return;

        SendAction();
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
			return false;

        return Owner.IsAlive;
	}

	private void SendAction()
	{
        Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_INJURY) as AgentActionInjury;
        Action.DamageType = Owner.BlackBoard.DamageType;
        Action.FromWeapon = Owner.BlackBoard.AttackerWeapon;
        Action.Attacker = Owner.BlackBoard.Attacker;
        Action.Impuls = Owner.BlackBoard.Impuls;

		Owner.BlackBoard.AddAction(Action);

        Owner.WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.ImInPain);
	}
}
