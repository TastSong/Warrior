using System;
using UnityEngine;

class GOAPActionAttackMeleeOnce : GOAPAction
{
	private AgentActionAttack Action;

	public GOAPActionAttackMeleeOnce(Agent owner) : base(E_GOAPAction.attackMeleeOnce, owner) { }


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

        DoAttackAction();
	}

	public override void Deactivate()
	{
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
        Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_ATTACK) as AgentActionAttack;
        Action.Target = null;// Owner.BlackBoard.DesiredTarget != null && Owner.BlackBoard.DesiredTarget.IsAlive ? Owner.BlackBoard.DesiredTarget : null;
        Action.AttackType = E_AttackType.X;

        if (Owner.BlackBoard.DesiredTarget != null && Owner.BlackBoard.DesiredTarget.IsAlive)
        {
            Vector3 dir = Owner.BlackBoard.DesiredTarget.Position - Owner.Position;
            dir.Normalize();
            Owner.BlackBoard.DesiredDirection = dir;
        }
        else
        {
            Owner.BlackBoard.DesiredDirection = Owner.Forward;
        }

        Action.AttackDir = Owner.BlackBoard.DesiredDirection;

        Action.Hit = false;
        Action.AttackPhaseDone = false;

       // Debug.Log("action attack  " + (Action.AttackTarget != null ? Action.AttackTarget.name : "no target"));

        Owner.BlackBoard.AddAction(Action);
        
    }

}
