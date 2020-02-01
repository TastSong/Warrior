using UnityEngine;


public class AgentActionInjury : AgentAction
{
    public Agent Attacker;
    public E_WeaponType FromWeapon;
    public E_DamageType DamageType;
    public Vector3 Impuls;

    public AgentActionInjury() : base(AgentActionFactory.E_Type.E_INJURY) { }
}
