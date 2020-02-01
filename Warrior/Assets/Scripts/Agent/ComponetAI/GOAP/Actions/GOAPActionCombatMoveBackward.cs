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

class GOAPActionCombatMoveBackward : GOAPAction
{
    private AgentActioCombatMove Action;

    public GOAPActionCombatMoveBackward(Agent owner) : base(E_GOAPAction.combatMoveBackward, owner) { }


    public override void InitAction()
    {
        WorldPreconditions.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, true);
        WorldEffects.SetWSProperty(E_PropKey.E_IN_COMBAT_RANGE, false);
        WorldEffects.SetWSProperty(E_PropKey.E_AT_TARGET_POS, true);
        Cost = 2;
    }

    // Validates the context preconditions
    public override bool ValidateContextPreconditions(Agent ai)
    {
        /*if (ai.BlackBoard.DesiredTarget == null)
            return false;

        Vector3 dirToEnemy = ai.BlackBoard.DesiredTarget.Position - ai.Position;

        if (ai.BlackBoard.BestPositionsToAttack.Count == 0)
            return false;
        */
        return true;
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
        if (Action != null && Action.IsActive() == false)
            return true;

        /* WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_IN_COMBAT_RANGE);
         if (prop.GetBool() == true)
             return true;*/

        return false;
    }

    public override bool ValidateAction()
    {
        if (Action != null && Action.IsFailed() == true)
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
        Action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_COMBAT_MOVE) as AgentActioCombatMove;
        Action.MoveType = E_MoveType.Backward;
        Action.Target = Owner.BlackBoard.DesiredTarget;
        Action.DistanceToMove = UnityEngine.Random.Range(2f, 5f);
        Action.MinDistanceToTarget = 0;
        Owner.BlackBoard.AddAction(Action);

        //UnityEngine.Debug.Log(this.ToString() + "Send new goto action to pos " + FinalPos.ToString());
    }
}
