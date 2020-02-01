using UnityEngine;

[System.Serializable]
public class AnimSetEnemyBossOrochi : AnimSet
{
    protected AnimAttackData AnimAttackX;
    protected AnimAttackData AnimAttackBerserk;
    protected AnimAttackData AnimAttackInjury;
	private bool orochiUnlock = false;

	void Awake()
	{
        AnimAttackX = new AnimAttackData("attackX", null, 0, 0.6f, 1.7f, 10, 60, 2, E_CriticalHitType.None, false);
        AnimAttackBerserk = new AnimAttackData("attackO", null, 0, 0.9f, 3.5f, 30, 30, 3.5f, E_CriticalHitType.None, false);
        AnimAttackInjury = new AnimAttackData("attackInjury", null, 0, 0.5f, 1.5f, 30, 180, 4, E_CriticalHitType.None, false);
	}

	public override string GetIdleAnim(E_WeaponType weapon, E_WeaponState weaponState)
	{
        return "idle";
	}

    public override string GetIdleActionAnim(E_WeaponType weapon, E_WeaponState weaponState)
    {
        return "tount";
    }

    public override string GetMoveAnim(E_MotionType motion, E_MoveType move, E_WeaponType weapon, E_WeaponState weaponState)
    {
        return "walk";
    }

    public override string GetRotateAnim(E_MotionType motionType, E_RotationType rotationType)
    {
        if (rotationType == E_RotationType.Left)
            return "rotateL";

        return "rotateR";
    }


    public override string GetRollAnim(E_WeaponType weapon, E_WeaponState weaponState){return null;}

    public override string GetBlockAnim(E_BlockState state, E_WeaponType weapon) { return ""; }

    public override string GetShowWeaponAnim(E_WeaponType weapon) {return ""; }

    public override string GetHideWeaponAnim(E_WeaponType weapon) {return ""; }


    public override AnimAttackData GetFirstAttackAnim(E_WeaponType weapon, E_AttackType attackType)
    {
        if (attackType == E_AttackType.X)
            return AnimAttackX;
        else if (attackType == E_AttackType.O)
            return AnimAttackInjury;
        else if (attackType == E_AttackType.Berserk)
            return AnimAttackBerserk;

        return null;
    }

    public override AnimAttackData GetChainedAttackAnim(AnimAttackData parent, E_AttackType attackType) {return null; }


    public override string GetUseAnim(E_InteractionObjects objectType, E_InteractionType interaction){return null;}

    public override string GetInjuryPhaseAnim(int phase) {
        string[] s = { "injury1", "injury2", "injury3", "injuryEnd" };

        return s[phase]; 
    }

    public override string GetInjuryAnim(E_WeaponType weapon, E_DamageType type) { return "null"; }

    public override string GetDeathAnim(E_WeaponType weapon, E_DamageType type)
    {
		if(orochiUnlock){
			//Achievements.UnlockAchievement(14);
		}
        return "death";
    }

    public override string GetKnockdowAnim(E_KnockdownState state, E_WeaponType weapon)  { return ""; }


}
