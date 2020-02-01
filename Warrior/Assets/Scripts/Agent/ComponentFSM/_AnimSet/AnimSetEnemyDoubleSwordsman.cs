using UnityEngine;

[System.Serializable]
public class AnimSetEnemyDoubleSwordsman : AnimSet
{
    protected AnimAttackData AnimAttacksSword1;
    protected AnimAttackData AnimAttacksSword2;
    protected AnimAttackData AnimAttackWhirl;

	void Awake()
	{
        AnimAttacksSword1 = new AnimAttackData("attackA", null, 1, 0.25f, 0.50f, 7, 20, 1, E_CriticalHitType.None, false);
        AnimAttacksSword2 = new AnimAttackData("attackAA", null, 1, 0.25f, 0.50f, 7, 20, 1, E_CriticalHitType.None, false);
        AnimAttackWhirl = new AnimAttackData("attackWhirl", null, 1, 0.25f, 0.40f, 7, 10, 1, E_CriticalHitType.None, false);
		Animation anims = GetComponent<Animation>();

        anims["idle"].layer = 0;
        anims["idleSword"].layer = 0;
        anims["idleTaunt"].layer = 1;

        anims["walk"].layer = 0;
        anims["combatMoveF"].layer = 0;
        anims["combatMoveB"].layer = 0;
        anims["combatMoveR"].layer = 0;
        anims["combatMoveL"].layer = 0;
        anims["combatRunF"].layer = 0;
        anims["combatRunB"].layer = 0;

        anims["rotationL"].layer = 0;
        anims["rotationR"].layer = 0;

        
        anims["death01"].layer = 2;
        anims["death02"].layer = 2;
        anims["injury01"].layer = 1;
        anims["injury02"].layer = 1;
        anims["injury03"].layer = 1;
        anims["injuryBack"].layer = 1;

        anims["attackA"].speed = 1.2f;
        anims["attackAA"].speed = 1.2f;

        anims["attackA"].layer = 0;
        anims["attackAA"].layer = 0;
        anims["attackWhirl"].layer = 0;

       // anims["attackA"].speed = 1.3f;
        //anims["attackAA"].speed = 1.3f;
        
            anims["knockdown"].layer = 0;
        anims["knockdownLoop"].layer = 0;
        anims["knockdownUp"].layer = 0;
        anims["knockdownDeath"].layer = 0;

        anims["showSword"].layer = 0;
        anims["hideSword"].layer = 1;

//        anims["spawn"].layer = 1;


	}

	public override string GetIdleAnim(E_WeaponType weapon, E_WeaponState weaponState)
	{
        if (weaponState == E_WeaponState.NotInHands)
            return "idle";

        return "idleSword";
	}

    public override string GetIdleActionAnim(E_WeaponType weapon, E_WeaponState weaponState)
    {
        if(weapon == E_WeaponType.Katana)
            return "idleTaunt";

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
            if (motion == E_MotionType.Walk)
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
            else
            {
                if (move == E_MoveType.Forward)
                    return "combatRunF";
                else if (move == E_MoveType.Backward)
                    return "combatRunB";
            }

        }

        return "idle";
    }

    public override string GetRotateAnim(E_MotionType motionType, E_RotationType rotationType)
    {
        if (motionType == E_MotionType.Block)
        {
            if (rotationType == E_RotationType.Left)
                return "blockStepL";

            return "blockStepR";
        }

        if (rotationType == E_RotationType.Left)
            return "rotationL";

        return "rotationR";
    }


    public override string GetRollAnim(E_WeaponType weapon, E_WeaponState weaponState){return null;}


    public override string GetBlockAnim(E_BlockState state, E_WeaponType weapon)
    {
        return null;
    }

    public override string GetShowWeaponAnim(E_WeaponType weapon)
    {
        return "showSword";
    }

    public override string GetHideWeaponAnim(E_WeaponType weapon)
    {
        return "hideSword";
    }


    public override AnimAttackData GetFirstAttackAnim(E_WeaponType weapom, E_AttackType attackType)
    {
        if (attackType == E_AttackType.X)
            return AnimAttacksSword1;


        return AnimAttacksSword2;
    }

    public override AnimAttackData GetChainedAttackAnim(AnimAttackData parent, E_AttackType attackType)
    {
        return null;
    }

    public override AnimAttackData GetWhirlAttackAnim()
    {
        return AnimAttackWhirl;
    }


    public override string GetUseAnim(E_InteractionObjects objectType, E_InteractionType interaction){return null;}


    public override string GetInjuryAnim(E_WeaponType weapon, E_DamageType type)
    {

        if(type == E_DamageType.Back)
            return "injuryBack";

        string[] anims = { "injury01", "injury02", "injury03" };

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
