using UnityEngine;


public class AgentActionAttackWhirl : AgentAction
{
    public AnimAttackData Data;

    public AgentActionAttackWhirl() : base(AgentActionFactory.E_Type.E_ATTACK_WHIRL) { }

    public override void Reset()
    {
        base.Reset();
    }
}
