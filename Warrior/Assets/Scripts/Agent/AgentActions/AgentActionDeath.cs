using UnityEngine;

public class AgentActionDeath : AgentAction
{
    public Agent Attacker;
    public E_CriticalHitType  DecapType;
    public E_WeaponType FromWeapon;
    public E_DamageType DamageType;
    public Vector3 Impuls;

    public AgentActionDeath() : base(AgentActionFactory.E_Type.E_DEATH) { } 
}
