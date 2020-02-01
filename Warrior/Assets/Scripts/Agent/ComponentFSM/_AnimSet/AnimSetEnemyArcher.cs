using UnityEngine;

[System.Serializable]
public class AnimSetEnemyArcher : AnimSet
{
    protected AnimAttackData AnimAttacksBow;

	void Awake()
	{
        AnimAttacksBow = new AnimAttackData("bowFire", null, -1, 2.95f, 3.0f, 10, 20, 1, E_CriticalHitType.None, false);

		Animation anims = GetComponent<Animation>();

        anims["idle"].layer = 0;
        anims["idleBow"].layer = 0;
        anims["walk"].layer = 0;
        anims["combatMoveF"].layer = 0;
        anims["combatMoveB"].layer = 0;
        anims["combatMoveR"].layer = 0;
        anims["combatMoveL"].layer = 0;
        anims["rotateLeft"].layer = 0;
        anims["rotateRight"].layer = 0;
        
        anims["death01"].layer = 2;
        anims["death02"].layer = 2;
        anims["injuryFront"].layer = 1;
        anims["injuryBack"].layer = 1;

        anims["bowFire"].layer = 0;
 
        //anims["showBow"].layer = 0;
        //anims["hideSword"].layer = 1;

        anims["knockdown"].layer = 0;
        anims["knockdownLoop"].layer = 0;
        anims["knockdownUp"].layer = 0;
        anims["knockdownDeath"].layer = 0;
	}

	public override string GetIdleAnim(E_WeaponType weapon, E_WeaponState weaponState)
	{
        if (weaponState == E_WeaponState.NotInHands)
            return "idle";

        return "idleBow";
	}

    public override string GetIdleActionAnim(E_WeaponType weapon, E_WeaponState weaponState)
    {
        Debug.LogError("unssupported !!");

        if(weapon == E_WeaponType.Katana)
            return "idleTount";

        return "idle";
    }

    public override string GetMoveAnim(E_MotionType motion, E_MoveType move, E_WeaponType weapon, E_WeaponState weaponState)
    {
        if (weaponState == E_WeaponState.NotInHands)
        {
            return "walk";
        }
        else 
        {
            if (move == E_MoveType.Forward)
                return "combatMoveF";
            else if (move == E_MoveType.Backward)
                return "combatMoveB";
            else if (move == E_MoveType.StrafeRight)
                return "combatMoveR";
            else
                return "combatMoveL";
        }
    }

    public override string GetRotateAnim(E_MotionType motionType, E_RotationType rotationType)
    {
        if (rotationType == E_RotationType.Left)
            return "rotateLeft";

        return "rotateRight";
    }


    public override string GetRollAnim(E_WeaponType weapon, E_WeaponState weaponState){return null;}


    public override string GetBlockAnim(E_BlockState state, E_WeaponType weapon)
    {
        return "idle";
    }

    public override string GetShowWeaponAnim(E_WeaponType weapon)
    {
        return "idleBow";
    }

    public override string GetHideWeaponAnim(E_WeaponType weapon)
    {
        return "idle";
    }


    public override AnimAttackData GetFirstAttackAnim(E_WeaponType weapom, E_AttackType attackType)
    {
        return AnimAttacksBow;
    }

    public override AnimAttackData GetChainedAttackAnim(AnimAttackData parent, E_AttackType attackType)
    {
        return null;
    }


    public override string GetUseAnim(E_InteractionObjects objectType, E_InteractionType interaction){return null;}


    public override string GetInjuryAnim(E_WeaponType weapon, E_DamageType type)
    {

        if(type == E_DamageType.Back)
            return "injuryBack";

        string[] anims = { "injuryFront",};

        return anims[Random.Range(0, anims.Length)];
    }

    public override string GetDeathAnim(E_WeaponType weapon, E_DamageType type)
    {
        string[] anims = { "death01", "death02"};

        return anims[Random.Range(0, 100) % anims.Length];
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
            case E_KnockdownState.Fatality:
                return "knockdownDeath";
            default:
                return "";
        }
    }


}
