using System;
using UnityEngine;

/* 
 * SLOW MOVE TO POSiTION
 *
 * Precondition - E_PropKey.E_WEAPON_IN_HANDS true
 * Effect - E_PropKey.E_AT_TARGET_POS true
 * 
 * Validate have target && distance < 6
 *  
 */
 
class GOAPActionCombatRunForward : GOAPAction
{
	private AgentActioCombatMove Action;

    public GOAPActionCombatRunForward(Agent owner) : base(E_GOAPAction.combatRunForward, owner) { }


	public override void InitAction()
	{
        WorldPreconditions.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, true);
		WorldEffects.SetWSProperty(E_PropKey.E_IN_COMBAT_RANGE, true);
        WorldEffects.SetWSProperty(E_PropKey.E_AT_TARGET_POS, true);
        Cost = 2;
	}

    // Validates the context preconditions
    public override bool ValidateContextPreconditions(Agent ai)
    {
        if (ai.BlackBoard.DistanceToTarget < 6)
            return false;

        return true;
    }


	public override void Update()
	{
        //Debug.Log("Owner pos - " + Owner.Transform.position + " my final - " + FinalPos.ToString() + " in WSprop " + p.GetVector().ToString());
	}

	public override void Activate()
	{   
		base.Activate();
        Owner.WorldState.SetWSProperty(E_PropKey.E_AT_TARGET_POS, false);

        SendAction();
	}

	public override void Deactivate()
	{
        Owner.WorldState.SetWSProperty(E_PropKey.E_AT_TARGET_POS, true);

        base.Deactivate();
	}

	public override bool IsActionComplete() 
	{
		if (Action != null && Action.IsActive() == false )
			return true;

		return false; 
	}

	public override bool ValidateAction()
	{
        if(Action != null && Action.IsFailed() == true)
			return false;

        if (Owner.BlackBoard.DesiredTarget != null)
        {
            if (Owner.BlackBoard.DistanceToTarget < 2)
                return false;
        }

		return true;
	}

	/*private void ActionRotate(Vector3 direction)
	{
		RotateAction = AgentActionFactory.Create(AgentActionFactory.E_Type.Rotate) as AgentActionRotate;
		RotateAction.Direction = direction;
		Owner.BlackBoard.AddAction(RotateAction);
	}*/

	private void SendAction()
	{
		Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_COMBAT_MOVE) as AgentActioCombatMove;
		Action.MoveType = E_MoveType.Forward;
        Action.MotionType = E_MotionType.Run;
        Action.Target = Owner.BlackBoard.DesiredTarget;
        Action.DistanceToMove =  Owner.BlackBoard.DistanceToTarget - (Owner.BlackBoard.CombatRange * 0.8f); // a little bit ahead of range
        Action.MinDistanceToTarget = 3;
		Owner.BlackBoard.AddAction(Action);

        //UnityEngine.Debug.Log(this.ToString() + "Send new goto action to pos " + FinalPos.ToString());

	}
}
