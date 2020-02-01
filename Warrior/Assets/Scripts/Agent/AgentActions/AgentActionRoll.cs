using UnityEngine;

public class AgentActionRoll : AgentAction
{
    public Vector3 Direction;
    public Agent ToTarget;

    public AgentActionRoll() : base(AgentActionFactory.E_Type.E_ROLL) { }

    public override void Reset() {
        ToTarget = null;
    }
}
