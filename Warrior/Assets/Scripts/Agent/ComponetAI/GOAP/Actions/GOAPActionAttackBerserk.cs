using System;
using UnityEngine;

class GOAPActionAttackBerserk : GOAPAction
{
	private AgentActionAttack Action;

    public GOAPActionAttackBerserk(Agent owner) : base(E_GOAPAction.attackBerserk, owner) { }


	public override void InitAction()
	{
		WorldPreconditions.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, true);
		WorldEffects.SetWSProperty(E_PropKey.E_ATTACK_TARGET, false);

        Precedence = 80;
        Interruptible = false;
        Cost = 1;
	}

    // Validates the context preconditions
    public override bool ValidateContextPreconditions(Agent ai)
    {
        if (Owner.BlackBoard.Berserk > Owner.BlackBoard.BerserkMax * 0.75f)
            return true;

        return false;
    }

	public override void Activate()
	{
		base.Activate();

        Owner.BlackBoard.Invulnerable = true;

        DoAttackAction();
	}

	public override void Deactivate()
	{
        Owner.BlackBoard.Invulnerable = false;
        Owner.BlackBoard.Berserk = Owner.BlackBoard.BerserkMin;

        base.Deactivate();
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
        Action = null;

        Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_ATTACK) as AgentActionAttack;
        Action.Data = Owner.AnimSet.GetFirstAttackAnim(Owner.BlackBoard.WeaponSelected, E_AttackType.Berserk);
        Action.AttackDir = Owner.Forward;
		Owner.BlackBoard.AddAction(Action);
    }

}
