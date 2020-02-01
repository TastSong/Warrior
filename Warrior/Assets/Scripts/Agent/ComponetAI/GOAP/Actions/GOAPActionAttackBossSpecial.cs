using System;
using UnityEngine;

class GOAPActionAttackBossSpecial : GOAPAction
{
    private AgentActionPlayIdleAnim ActionTount;
    private AgentActionAttack  ActionAttack;

    public GOAPActionAttackBossSpecial(Agent owner) : base(E_GOAPAction.attackBoss, owner) { }


	public override void InitAction()
	{
        WorldPreconditions.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, true);
        WorldPreconditions.SetWSProperty(E_PropKey.E_LOOKING_AT_TARGET, true);          
		WorldEffects.SetWSProperty(E_PropKey.E_ATTACK_TARGET, false);

        Interruptible = false;

        Cost = 2;
        Precedence = 50;
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

        Owner.BlackBoard.ReactOnHits = false;

        SendTountAction();
	}

	public override void Deactivate()
	{
        Owner.BlackBoard.Berserk = Owner.BlackBoard.BerserkMin;
		base.Deactivate();
	}


    public override void Update()
    {
        if (ActionTount != null && ActionTount.IsActive() == false)
        {
            ActionTount = null;
            SendAttackAction();
        }

        if (ActionAttack != null && ActionAttack.AttackPhaseDone == true)
        {
            Owner.BlackBoard.ReactOnHits = true;
        }

    }

	public override bool IsActionComplete() 
	{
        if (ActionTount == null && ActionAttack != null && ActionAttack.IsActive() == false)
			return true;

		return false; 
	}

    void SendTountAction()
    {
		Owner.Sound.PlayBerserk();
        ActionTount = AgentActionFactory.Create(AgentActionFactory.E_Type.E_PLAY_IDLE_ANIM) as AgentActionPlayIdleAnim;
		Owner.BlackBoard.AddAction(ActionTount);
    }

    void SendAttackAction()
    {
        Owner.BlackBoard.ReactOnHits = false;
        ActionAttack = AgentActionFactory.Create(AgentActionFactory.E_Type.E_ATTACK) as AgentActionAttack;
        ActionAttack.Data = Owner.AnimSet.GetFirstAttackAnim(Owner.BlackBoard.WeaponSelected, E_AttackType.Berserk);
        ActionAttack.AttackDir = Owner.Forward;

        // Debug.Log("action attack  " + (Action.AttackTarget != null ? Action.AttackTarget.name : "no target"));

		Owner.BlackBoard.AddAction(ActionAttack);
    }

}
