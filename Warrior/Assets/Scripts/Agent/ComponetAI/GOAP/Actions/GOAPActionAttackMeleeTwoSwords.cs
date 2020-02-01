using System;
using UnityEngine;

class GOAPActionAttackMeleeTwoSwords : GOAPAction
{
	private AgentActionAttack Action;
    private int NumberOfAttacks;
    private E_AttackType CurrentAttacktype;

    public GOAPActionAttackMeleeTwoSwords(Agent owner) : base(E_GOAPAction.attackMeleeTwoSwords, owner) { }


	public override void InitAction()
	{
        WorldPreconditions.SetWSProperty(E_PropKey.E_IN_WEAPONS_RANGE, true);
		WorldPreconditions.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, true);
        WorldPreconditions.SetWSProperty(E_PropKey.E_LOOKING_AT_TARGET, true);
		WorldEffects.SetWSProperty(E_PropKey.E_ATTACK_TARGET, false);

        Interruptible = false;

        Cost = 2;
	}


	public override void Activate()
	{
		base.Activate();

        CurrentAttacktype = E_AttackType.X;
        NumberOfAttacks = UnityEngine.Random.Range(2, 4);


        DoAttackAction(Owner.BlackBoard.DesiredTarget); // first attack is aimed
	}

	public override void Deactivate()
	{
        base.Deactivate();
	}

	public override bool IsActionComplete() 
	{
        if (Action == null || (NumberOfAttacks == 0 && Action.IsActive() == false))
			return true;

		return false; 
	}

    public override void Update()
    {
        if (Action.AttackPhaseDone == true && NumberOfAttacks > 0)
        {
            if (CurrentAttacktype == E_AttackType.X)
                CurrentAttacktype = E_AttackType.O;
            else
                CurrentAttacktype = E_AttackType.X;

			Owner.Sound.PlayPrepareAttack();

            DoAttackAction(null); //next attack is just forward
        }
    }

    void DoAttackAction(Agent target)
    {
        Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_ATTACK) as AgentActionAttack;
        Action.AttackType = CurrentAttacktype;

        if (Owner.BlackBoard.DesiredTarget)
        {
            Owner.BlackBoard.DesiredDirection = Owner.BlackBoard.DesiredTarget.Position - Owner.Position;
            Owner.BlackBoard.DesiredDirection.Normalize();
            Action.AttackDir = Owner.BlackBoard.DesiredDirection;
        }
        else
            Action.AttackDir = Owner.Forward;

        Action.Hit = false;
        Action.AttackPhaseDone = false;
        Owner.BlackBoard.AddAction(Action);

        NumberOfAttacks--;
    }

}
