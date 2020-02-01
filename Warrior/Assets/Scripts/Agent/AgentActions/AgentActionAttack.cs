using UnityEngine;


public class AgentActionAttack : AgentAction
{
    //init values
	public Agent Target;
    public AnimAttackData Data;
    public E_AttackType AttackType;
    public Vector3 AttackDir;



	public bool Hit;
    public bool AttackPhaseDone;

    public AgentActionAttack() : base(AgentActionFactory.E_Type.E_ATTACK) { }

    public override void Reset()
    {
        base.Reset();
        Target = null;
        Hit = false;
        AttackPhaseDone = false;
        Data = null;
        AttackType = E_AttackType.None;
    }
    public override string ToString() { return "HumanActionAttack " + (Target != null ? Target.name : "no target") + " " + AttackType.ToString() + " " + Status.ToString(); }

}
