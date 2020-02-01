using UnityEngine;


public class AgentActionBlock : AgentAction
{
    public Agent Attacker;
    public E_WeaponType FromWeapon;
    public float Time;

    public AgentActionBlock() : base(AgentActionFactory.E_Type.E_BLOCK) { }
}
