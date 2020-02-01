using System;
using UnityEngine;

class GOAPActionGoToMeleeRange : GOAPAction
{
	private AgentActionGoTo Action;
    private Vector3 FinalPos;
    ParticleEmitter Effect;

    public GOAPActionGoToMeleeRange(Agent owner) : base(E_GOAPAction.gotoMeleeRange, owner) 
    {
        Transform t = owner.Transform.Find("sust");
        if(t)
            Effect = t.GetComponent<ParticleEmitter>();
    }


	public override void InitAction()
	{
		WorldEffects.SetWSProperty(E_PropKey.E_IN_WEAPONS_RANGE, true);
        Cost = 3;
        Precedence = 70;
	}

	public override void Update()
	{
        //test if target moved away of last checked pos... if yes we have to send new action
       // if (Owner.BlackBoard.DesiredTarget != null && (LastTargetPos - Owner.BlackBoard.DesiredTarget.Position).sqrMagnitude > 0.1f * 0.1f)
       //     ActionGoTo(Owner.BlackBoard.DesiredTarget);*/
	}

	public override void Activate()
	{   
		base.Activate();

        if (Owner.BlackBoard.DesiredTarget != null)
            ActionGoTo(Owner.BlackBoard.DesiredTarget);
	}

	public override void Deactivate()
	{
        if (Owner.BlackBoard.DesiredTarget != null && Owner.BlackBoard.DistanceToTarget < Owner.BlackBoard.WeaponRange)
           Owner.WorldState.SetWSProperty(E_PropKey.E_IN_WEAPONS_RANGE, true);

        base.Deactivate();
	}

	public override bool IsActionComplete() 
	{
        if (Action != null)
        {
            if (Action.IsActive() == false || Owner.BlackBoard.DistanceToTarget < Owner.BlackBoard.WeaponRange * 0.5f)
                return true;
        }
		return false; 
	}

	public override bool ValidateAction()
	{
        if (Owner.BlackBoard.DesiredTarget == null || Owner.BlackBoard.DesiredTarget.IsAlive == false)
            return false;

        if(Action != null && Action.IsFailed() == true)
		{
            //UnityEngine.Debug.Log(this.ToString() + " not valid anymore !" + FinalPos.ToString());
			return false;
		}

		return true;
	}

	private void ActionGoTo(Agent target)
	{
        FinalPos = GetBestAttackStart(target);

		Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_GOTO) as AgentActionGoTo;
		Action.MoveType = AgentActionGoTo.E_MoveType.E_MT_FORWARD;
		Action.Motion = E_MotionType.Sprint;
		Action.FinalPosition = FinalPos;
		Owner.BlackBoard.AddAction(Action);


        if (Effect && (FinalPos - Owner.Position).magnitude > 0.5f)
        {
            CombatEffectsManager.Instance.StartCoroutine(CombatEffectsManager.Instance.PlayAndStop(Effect, 0.1f));
        }
	}

    private Vector3 GetBestAttackStart(Agent Target)
    {
        Vector3 dirToTarget = Target.Position - Owner.Position;
        dirToTarget.Normalize();

        return Target.Position - dirToTarget * Owner.BlackBoard.WeaponRange;
    }
}
