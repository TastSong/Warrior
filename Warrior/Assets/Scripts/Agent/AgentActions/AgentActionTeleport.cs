using UnityEngine;

public class AgentActionTeleport : AgentAction
{
	public bool ShowParticle = false;
    public bool FadeGui = false;
    public Teleport Teleport;


    public AgentActionTeleport() : base(AgentActionFactory.E_Type.Teleport) { }

    public override void Reset()
    {
        ShowParticle = false;
        FadeGui = false;
    }
}
