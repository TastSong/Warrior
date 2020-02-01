using UnityEngine;

public class AgentActionIncommingAttack : AgentAction
{
	public Agent Attacker;
	public float HitTime;

    public AgentActionIncommingAttack() : base(AgentActionFactory.E_Type.E_INCOMMING_ATTACK) { }
}
