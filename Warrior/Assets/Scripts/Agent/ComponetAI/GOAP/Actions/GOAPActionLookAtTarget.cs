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

class GOAPActionLookAtTarget : GOAPAction
{
	private AgentActionRotate Action;
    Fact Query;

    public GOAPActionLookAtTarget(Agent owner) : base(E_GOAPAction.lookatTarget, owner) { }


	public override void InitAction()
	{
        Query = new Fact(Fact.E_FactType.E_EVENT);
        Query.EventType = E_EventTypes.EnemyLost;

      //  WorldPreconditions.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, true);
		WorldEffects.SetWSProperty(E_PropKey.E_LOOKING_AT_TARGET, true);
        Cost = 1;
        Precedence = 95;
	}

    // Validates the context preconditions
    public override bool ValidateContextPreconditions(Agent ai)
    {
        Fact fact = Owner.Memory.GetFact(Query);
        if (fact == null && Owner.BlackBoard.DesiredTarget == null)
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

        SendAction();
	}

	public override void Deactivate()
	{
  //      Owner.BlackBoard.LookType = E_LookType.E_NONE;
        WorldEffects.SetWSProperty(E_PropKey.E_LOOKING_AT_TARGET, true);
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
        Fact fact = Owner.Memory.GetFact(Query);
        if (fact != null)
        {
            Action = AgentActionFactory.Create(AgentActionFactory.E_Type.Rotate) as AgentActionRotate;
            Action.Target = fact.Agent;

            Action.RotationModifier = 0.8f;

            Owner.BlackBoard.AddAction(Action);

            //if (Owner.debugGOAP) Debug.Log(this.ToString() + " - " + Action.Target.GameObject.ToString());
        }
        else if(Owner.BlackBoard.DesiredTarget != null)
        {
            Action = AgentActionFactory.Create(AgentActionFactory.E_Type.Rotate) as AgentActionRotate;
            Action.Target = Owner.BlackBoard.DesiredTarget;

            Action.RotationModifier = 0.5f;

            Owner.BlackBoard.AddAction(Action);

            //if (Owner.debugGOAP) Debug.Log(this.ToString() + " - " + Action.Target.GameObject.ToString());
        }

        
        //UnityEngine.Debug.Log(this.ToString() + "Send new goto action to pos " + FinalPos.ToString());

	}
}
