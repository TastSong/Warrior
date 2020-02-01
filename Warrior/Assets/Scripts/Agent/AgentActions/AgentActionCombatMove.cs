using UnityEngine;

public class AgentActioCombatMove: AgentAction  
{
    public Agent Target;
    public float DistanceToMove;
    public float MinDistanceToTarget;

    public E_MoveType MoveType;
    public E_MotionType MotionType = E_MotionType.Walk; 

    public AgentActioCombatMove() : base(AgentActionFactory.E_Type.E_COMBAT_MOVE) { }

    public override void Reset()
    {
        base.Reset();
        DistanceToMove = 0;
        MinDistanceToTarget = 0;
        MoveType = E_MoveType.Forward;
        MotionType = E_MotionType.Walk;
    }
}
