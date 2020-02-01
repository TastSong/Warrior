using UnityEngine;


public class AgentActionDamageBlocked : AgentAction
{
    public Agent Attacker;
    public E_WeaponType AttackerWeapon;
    public bool BreakBlock;

    public AgentActionDamageBlocked() : base(AgentActionFactory.E_Type.E_DAMAGE_BLOCKED) { }

    public override void Reset() {
        BreakBlock = false;
    }
}
