using System;
using UnityEngine;

class GOAPActionBlock : GOAPAction
{
    private AgentActionBlock Action;
    AgentActionDamageBlocked ActionInjuryBlocked;
    //private float WorldStateTime = 0;
    //private Fact Query;

    public GOAPActionBlock(Agent owner) : base(E_GOAPAction.block, owner) { }


	public override void InitAction()
	{
		//WorldEffects.SetWSProperty(E_PropKey.E_IN_DODGE, true);
        WorldEffects.SetWSProperty(E_PropKey.E_IN_BLOCK, true);
        Cost = 2;
        Interruptible = true;

        Precedence = 100;

       // Query = FactsFactory.Create(Fact.E_FactType.E_EVENT);
        //Query.EventType = E_EventTypes.E_SOMEBODY_IS_ATTACKING_ME;
	}

	public override void Activate()
	{   
		base.Activate();
        Owner.WorldState.SetWSProperty(E_PropKey.E_IN_BLOCK, true);

        DoAction();
	}

	public override void Deactivate()
	{
        Owner.WorldState.SetWSProperty(E_PropKey.E_IN_BLOCK, false);

        //AgentActionFactory.Return(Action);
        Action = null;

		base.Deactivate();
	}

    public override void Update()
    {
       E_EventTypes eventType = Owner.WorldState.GetWSProperty(E_PropKey.E_EVENT).GetEvent();
       if (eventType == E_EventTypes.HitBlocked)
       {
           ActionInjuryBlocked  = AgentActionFactory.Create(AgentActionFactory.E_Type.E_DAMAGE_BLOCKED) as AgentActionDamageBlocked;
           ActionInjuryBlocked.Attacker = Owner.BlackBoard.Attacker;
           ActionInjuryBlocked.AttackerWeapon = Owner.BlackBoard.AttackerWeapon;
           ActionInjuryBlocked.BreakBlock = Owner.BlackBoard.DamageType == E_DamageType.BreakBlock;


           Owner.BlackBoard.AddAction(ActionInjuryBlocked);
           Owner.WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.None);
       }

       if (ActionInjuryBlocked != null && ActionInjuryBlocked.IsActive() == false)
       {
           //AgentActionFactory.Return(ActionInjuryBlocked);
           ActionInjuryBlocked = null;
       }

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

        if (Owner.BlackBoard.DistanceToTarget > 4)
            return false;

		return true;
	}

    private void DoAction()
	{
        Action = null;

        if (Owner.BlackBoard.DesiredTarget == null)
            return;

        Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_BLOCK) as AgentActionBlock;

        Action.Attacker = Owner.BlackBoard.DesiredTarget;
        Action.Time = 3.0f;

		Owner.BlackBoard.AddAction(Action);

        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_IN_DODGE);
        if (prop == null || prop.GetBool() == true)
            return;

        //WorldStateTime = prop.Time;


        //UnityEngine.Debug.Log(this.ToString() + "Send new roll action to pos " + Action.Direction.ToString());
	}
}
