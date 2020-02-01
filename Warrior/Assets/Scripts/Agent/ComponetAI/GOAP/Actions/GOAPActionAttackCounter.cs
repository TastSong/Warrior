using System;
using UnityEngine;

class GOAPActionAttackCounter : GOAPAction
{
	private AgentActionAttack Action;

    public GOAPActionAttackCounter(Agent owner) : base(E_GOAPAction.attackCounter, owner) { }


	public override void InitAction()
	{
		WorldEffects.SetWSProperty(E_PropKey.E_ATTACK_TARGET, false);

        Precedence = 80;
        Interruptible = false;
        Cost = 2;
	}

    // Validates the context preconditions
    public override bool ValidateContextPreconditions(Agent ai)
    {
        if (Owner.IsBlocking && Owner.BlackBoard.Berserk > Owner.BlackBoard.BerserkMax * 0.75f)
            return true;

        return false;
    }

	public override void Activate()
	{
		base.Activate();
       // Time.timeScale = 0.2f;

        Owner.BlackBoard.Invulnerable = true;

        DoAttackAction();
	}

	public override void Deactivate()
	{
      //  Time.timeScale = 1;

        Owner.BlackBoard.Berserk = Owner.BlackBoard.BerserkMin;

        Owner.BlackBoard.Invulnerable = false;

        base.Deactivate();

       /* WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_ATTACK_TARGET);
        if (prop == null || prop.GetBool() == false)
            return;

        if (WorldStateTime == prop.Time)
            Owner.WorldState.SetWSProperty(E_PropKey.E_ATTACK_TARGET, false);
        */
	}

	public override bool IsActionComplete() 
	{
		if (Action.IsActive() == false)
			return true;

		return false; 
	}

    public override void Update()
    {
    }

    void DoAttackAction()
    {
		Owner.Sound.PlayBerserk();

        Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_ATTACK) as AgentActionAttack;
        Action.Target = Owner.BlackBoard.DesiredTarget != null && Owner.BlackBoard.DesiredTarget.IsAlive ? Owner.BlackBoard.DesiredTarget : null;
        Action.AttackType = E_AttackType.Counter;
        Action.AttackDir = Owner.BlackBoard.DesiredDirection;

        Action.Hit = false;
        Action.AttackPhaseDone = false;
        Owner.BlackBoard.AddAction(Action);
    }

}
