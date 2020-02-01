using UnityEngine;


public class AgentActionAttackRoll : AgentAction
{
    public AnimAttackData Data;
    public Agent Target;

    public AgentActionAttackRoll() : base(AgentActionFactory.E_Type.E_ATTACK_ROLL) { }
}
