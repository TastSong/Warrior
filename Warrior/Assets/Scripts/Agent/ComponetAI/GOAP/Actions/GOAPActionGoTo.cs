using System;
using UnityEngine;

class GOAPActionGoTo : GOAPAction
{
	private AgentActionGoTo Action;
	//private AgentActionRotate RotateAction;
	private Vector3 FinalPos;

    public GOAPActionGoTo(Agent owner) : base(E_GOAPAction.gotoPos, owner) { }


	public override void InitAction()
	{
		WorldEffects.SetWSProperty(E_PropKey.E_AT_TARGET_POS, true);
        Cost = 5;
        Precedence = 70;
	}

    // Validates the context preconditions
    public override bool ValidateContextPreconditions(Agent ai)
    {
        if(ai.WorldState.GetWSProperty(E_PropKey.E_USE_WORLD_OBJECT).GetBool() == true)
            return true;

        return false;
    }


	public override void Update()
	{

        //Debug.Log("Owner pos - " + Owner.Transform.position + " my final - " + FinalPos.ToString() + " in WSprop " + p.GetVector().ToString());
        if (FinalPos != Owner.BlackBoard.DesiredPosition)
        {
            ActionGoTo(Owner.BlackBoard.DesiredPosition);
         }
	}

	public override void Activate()
	{   
		base.Activate();
        ActionGoTo(Owner.BlackBoard.DesiredPosition);
	}

	public override void Deactivate()
	{
		base.Deactivate();

        Owner.WorldState.SetWSProperty(E_PropKey.E_AT_TARGET_POS, true);
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
	
        if(Action != null && Action.IsFailed() == true)
		{
            //UnityEngine.Debug.Log(this.ToString() + " not valid anymore !" + FinalPos.ToString());
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

	private void ActionGoTo(Vector3 finalPos )
	{
        FinalPos = finalPos;

		Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_GOTO) as AgentActionGoTo;
		Action.MoveType = AgentActionGoTo.E_MoveType.E_MT_FORWARD;
		Action.Motion = E_MotionType.Run;
		Action.FinalPosition = FinalPos;
		Owner.BlackBoard.AddAction(Action);

        //UnityEngine.Debug.Log(this.ToString() + "Send new goto action to pos " + FinalPos.ToString());

	}
}
