using System;
using UnityEngine;

class GOAPActionKnockdown : GOAPAction
{
    private AgentActionKnockdown Action;
    private AgentActionDeath ActionKill;
    //private float WorldStateTime = 0;

    public GOAPActionKnockdown(Agent owner) : base(E_GOAPAction.knockdown, owner) { }


	public override void InitAction()
	{
		WorldEffects.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.None);

		Interruptible = false;

        Cost = 1;
        Precedence = 100;
	}

    public override bool ValidateContextPreconditions(Agent ai)
    {
        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_EVENT);

        if(prop == null || prop.GetEvent() !=  E_EventTypes.Knockdown)
            return false;

        if(Owner.IsAlive == false)
            return false;

        return true;
    }

	public override void Activate()
	{   
		base.Activate();
        
        Action = null;

        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_EVENT);
        if (prop == null || prop.GetEvent() != E_EventTypes.Knockdown)
            return;

        //WorldStateTime = prop.Time;

        SendAction();

	}

	public override void Deactivate()
	{
        Action = null;
		base.Deactivate();
        
        Owner.WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.None);
	}

    public override void Update()
    {
        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_EVENT);
        if (prop.GetEvent() != E_EventTypes.Hit && prop.GetEvent() != E_EventTypes.Died)
            return;

        SendActionKill();
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

	private void SendAction()
	{

        Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_KNOCKDOWN) as AgentActionKnockdown;
        Action.FromWeapon = Owner.BlackBoard.AttackerWeapon;
        Action.Attacker = Owner.BlackBoard.Attacker;
        Action.Impuls = Owner.BlackBoard.Impuls;
        Action.Time = Owner.BlackBoard.MaxKnockdownTime * UnityEngine.Random.Range(0.7f, 1);

        Owner.BlackBoard.AddAction(Action);
        Owner.WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.Knockdown);
	}

    private void SendActionKill()
    {
        //if (Owner.debugGOAP) Debug.Log(Time.timeSinceLevelLoad + " " + this.ToString() + " Send action to kill");

        ActionKill = AgentActionFactory.Create(AgentActionFactory.E_Type.E_DEATH) as AgentActionDeath;
        ActionKill.FromWeapon = Owner.BlackBoard.AttackerWeapon;
        ActionKill.Attacker = Owner.BlackBoard.Attacker;

        Owner.BlackBoard.AddAction(ActionKill);

        Owner.WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.None);

        Owner.BlackBoard.DontUpdate = true;
    }

}
