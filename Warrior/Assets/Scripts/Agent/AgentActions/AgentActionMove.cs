using UnityEngine;

public class AgentActionMove : AgentAction  
{
    public enum E_MoveType
    {
        E_MT_FORWARD,
        E_MT_BACKWARD,
        E_STRAFE_LEFT,
        E_STRAFE_RIGHT,
    }
 
    public E_MoveType MoveType = E_MoveType.E_MT_FORWARD;

    public AgentActionMove() : base(AgentActionFactory.E_Type.E_MOVE) { }

	public override string ToString() { return "AgentActionMove MoveType: " + MoveType.ToString() + " " + Status.ToString(); }
	
}
