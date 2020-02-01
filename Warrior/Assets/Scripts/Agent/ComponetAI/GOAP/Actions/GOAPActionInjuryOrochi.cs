using System;
using UnityEngine;

class GOAPActionInjuryOrochi : GOAPAction
{
    private AgentActionInjury Action;
    private AgentActionAttack ActionAttack;
    Fact Query;

    private int Count;

    public GOAPActionInjuryOrochi(Agent owner) : base(E_GOAPAction.injuryOrochi, owner) { }


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
        ActionAttack = null;
        Count = 0;
        Interruptible = false;

        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_EVENT);
        if (prop == null || prop.GetEvent() != E_EventTypes.Hit)
            return;

        SendAction();
	}

	public override void Deactivate()
	{
        Owner.BlackBoard.Invulnerable = false;
        Action = null;
        ActionAttack = null;
		base.Deactivate();

        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_EVENT);
        if (prop == null || prop.GetEvent() != E_EventTypes.Hit)
            return;
        // if there is some hit, clear it !!
        Owner.WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.None);
	}

    public override void Update()
    {
        if (Owner.IsAlive == false)
            Interruptible = true;

        if (Count == 3)
        {
            if (Action.IsActive() == false && ActionAttack == null)
            {
				Owner.Sound.PlayBerserk();
                Owner.BlackBoard.Invulnerable = true;
                ActionAttack = AgentActionFactory.Create(AgentActionFactory.E_Type.E_ATTACK) as AgentActionAttack;
                ActionAttack.Data = Owner.AnimSet.GetFirstAttackAnim(Owner.BlackBoard.WeaponSelected, E_AttackType.O);
                ActionAttack.AttackDir = Owner.Forward;
                Owner.BlackBoard.AddAction(ActionAttack);
            }
            return;
        }

        WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_EVENT);
        if (prop == null || prop.GetEvent() != E_EventTypes.Hit || Owner.IsAlive == false)
            return;

        SendAction();
    }


	public override bool IsActionComplete() 
	{
        if (Action != null && Action.IsActive() == false && (ActionAttack == null || ActionAttack.IsActive() == false))
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
        Count++; 

        Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_INJURY) as AgentActionInjury;
        Action.DamageType = Owner.BlackBoard.DamageType;
        Action.FromWeapon = Owner.BlackBoard.AttackerWeapon;
        Action.Attacker = Owner.BlackBoard.Attacker;
        Action.Impuls = Owner.BlackBoard.Impuls;

        Owner.BlackBoard.AddAction(Action);

        Owner.WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.ImInPain);

        //UnityEngine.Debug.Log(this.ToString() + " sending injury number " + Count);
	}
}
