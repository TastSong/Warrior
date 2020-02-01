using UnityEngine;


public class AgentActionKnockdown : AgentAction
{
    public Agent Attacker;
    public E_WeaponType FromWeapon;
    public Vector3 Impuls; 
    public float Time;

    public AgentActionKnockdown() : base(AgentActionFactory.E_Type.E_KNOCKDOWN) { }
}
