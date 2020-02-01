using UnityEngine;[System.Serializable]
public class AnimSetEnemyBoss01 : AnimSet{
    protected AnimAttackData AnimAttacksSwordL = new AnimAttackData("attackA", null, -1, 0.5f, 0.9f, 10, 20, 1, E_CriticalHitType.None, false);
    protected AnimAttackData AnimAttacksSwordS = new AnimAttackData("attackAA", null, -1, 0.5f, 1.5f, 30, 20, 1, E_CriticalHitType.None, false);

    protected AnimAttackData AnimAttacksBash = new AnimAttackData("attackBash", null, -1, 0.5f, 1.5f, 30, 20, 5, E_CriticalHitType.None, false);
    protected AnimAttackData AnimAttacksAfterknockdown = new AnimAttackData("attackAfterKnockdown", null, -1, 0.5f, 1.5f, 30, -1, 5, E_CriticalHitType.None, false);	void Awake()	{	//	Animation anims = animation;	}	public override string GetIdleAnim(E_WeaponType weapon, E_WeaponState weaponState)	{        return "idle";	}

    public override string GetIdleActionAnim(E_WeaponType weapon, E_WeaponState weaponState)
    {
        return "";
    }

    public override string GetMoveAnim(E_MotionType motion, E_MoveType move, E_WeaponType weapon, E_WeaponState weaponState)    {
        return "walk";    }

    public override string GetRotateAnim(E_MotionType motionType, E_RotationType rotationType)
    {
        if (rotationType == E_RotationType.Left)
            return "rotateLeft";

        return "rotateRight";
    }
    public override string GetRollAnim(E_WeaponType weapon, E_WeaponState weaponState){return null;}    public override string GetBlockAnim(E_BlockState state, E_WeaponType weapon){return null;}

    public override string GetShowWeaponAnim(E_WeaponType weapon) { return null; }

    public override string GetHideWeaponAnim(E_WeaponType weapon) { return null; }


    public override AnimAttackData GetFirstAttackAnim(E_WeaponType weapom, E_AttackType attackType)
    {
        if (attackType == E_AttackType.X)
            return AnimAttacksSwordL;
        else if (attackType == E_AttackType.Fatality)
            return AnimAttacksAfterknockdown;
        else if (attackType == E_AttackType.BossBash)
            return AnimAttacksBash;
        
        return AnimAttacksSwordS;
    }

    public override AnimAttackData GetChainedAttackAnim(AnimAttackData parent, E_AttackType attackType)
    {
        return null;
    }
    public override string GetUseAnim(E_InteractionObjects objectType, E_InteractionType interaction){return null;}


    public override string GetInjuryAnim(E_WeaponType weapon, E_DamageType type)
    {
        return "injury01";
    }

    public override string GetDeathAnim(E_WeaponType weapon, E_DamageType type)
    {
        return "knockdownDeath";
    }

    public override string GetKnockdowAnim(E_KnockdownState state, E_WeaponType weapon)
    {
        switch (state)
        {
            case E_KnockdownState.Down:
                return "knockdown";
            case E_KnockdownState.Loop:
                return "knockdownLoop";
            case E_KnockdownState.Up:
                return "knockdownUp";
            default:
                return null;
        }
    }}