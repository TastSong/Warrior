using System;
using UnityEngine;

class GOAPActionDeath : GOAPAction
{
    private AgentActionDeath Action;
    Fact Query;

    public GOAPActionDeath(Agent owner) : base(E_GOAPAction.death, owner) { }


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
        WorldStateProp prop = ai.WorldState.GetWSProperty(E_PropKey.E_EVENT);

        if (prop == null || prop.GetEvent() != E_EventTypes.Died)
            return false;

        return true;

    }

    public override void Activate()
    {
        base.Activate();

        Action = null;

        SendAction();
    }

    public override void Deactivate()
    {
        Action = null;
        base.Deactivate();
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

        if (Action != null && Action.IsFailed() == true)
        {
            //UnityEngine.Debug.Log(this.ToString() + " not valid anymore !");
            return false;
        }

        return true;
    }

    private void SendAction()
    {
        Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_DEATH) as AgentActionDeath;
        Action.DamageType = Owner.BlackBoard.DamageType;
        Action.FromWeapon = Owner.BlackBoard.AttackerWeapon;
        Action.Attacker = Owner.BlackBoard.Attacker;
        Action.Impuls = Owner.BlackBoard.Impuls;

		Owner.BlackBoard.AddAction(Action);

        Owner.WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.None);

        Owner.BlackBoard.DontUpdate = true;
    }
}
