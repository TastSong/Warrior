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
 
class GOAPActionCombatMoveToRight : GOAPAction
{
	private AgentActioCombatMove Action;

    public GOAPActionCombatMoveToRight(Agent owner) : base(E_GOAPAction.combatMoveRight, owner) { }


	public override void InitAction()
	{
        WorldPreconditions.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, true);
	//	WorldEffects.SetWSProperty(E_PropKey.E_IN_COMBAT_RANGE, true);
        WorldEffects.SetWSProperty( E_PropKey.MoveToRight, true);
        Cost = 2;
	}
    
	public override void Activate()
	{   
		base.Activate();
        //Owner.WorldState.SetWSProperty(E_PropKey.E_AT_TARGET_POS, false);

        SendAction();
	}

	public override void Deactivate()
	{
       // Owner.WorldState.SetWSProperty(E_PropKey.E_AT_TARGET_POS, true);
        //WorldEffects.SetWSProperty(E_PropKey.MoveToRight, false);

        base.Deactivate();
	}

	public override bool IsActionComplete() 
	{
		if (Action != null && Action.IsActive() == false )
			return true;

       /* WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_IN_COMBAT_RANGE);
        if (prop.GetBool() == true)
            return true;*/

		return false; 
	}

	public override bool ValidateAction()
	{
        if(Action != null && Action.IsFailed() == true)
			return false;

       /* if (Owner.BlackBoard.DesiredTarget != null)
        {
            if (Owner.BlackBoard.DistanceToTarget < 2)
                return false;
        }*/

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
		Action.MoveType = E_MoveType.StrafeRight;
        Action.Target = Owner.BlackBoard.DesiredTarget;
        Action.DistanceToMove = UnityEngine.Random.Range(2.0f, 4.0f); ;
        Action.MinDistanceToTarget = Owner.BlackBoard.DistanceToTarget * 0.7f;
		Owner.BlackBoard.AddAction(Action);

        //UnityEngine.Debug.Log(this.ToString() + "Send new goto action to pos " + FinalPos.ToString());

	}
}
