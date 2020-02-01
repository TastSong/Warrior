using UnityEngine;

[System.Serializable]
public class AnimSetEnemyBoss03 : AnimSet
{
    protected AnimAttackData AnimAttacksSwordL = new AnimAttackData("attackA", null, -1, 0.5f, 0.9f, 10, 20, 1, E_CriticalHitType.None, false);
    protected AnimAttackData AnimAttacksSwordS = new AnimAttackData("attackB", null, -1, 0.5f, 1.5f, 30, 20, 1, E_CriticalHitType.None, false);

	void Awake()
	{
		Animation anims = GetComponent<Animation>();

        anims["idleSword"].layer = 0;
        anims["walkSword"].layer = 0;
        
        /*anims["death01"].layer = 2;
        anims["death02"].layer = 2;
        anims["injuryFront01"].layer = 1;
        anims["injuryFront02"].layer = 1;
        anims["injuryFront03"].layer = 1;
        anims["injuryFront04"].layer = 1;
        anims["injuryBack"].layer = 1;*/

        anims["attackA"].layer = 0;
        anims["attackB"].layer = 0;

        anims["blockStart"].layer = 0;
        anims["blockLoop"].layer = 0;
        anims["blockEnd"].layer = 0;
        anims["blockFailed"].layer = 0;
        anims["blockHit"].layer = 0;
      //  anims["blockStepLeft"].layer = 0;
//        anims["blockStepRight"].layer = 0;
	}

	public override string GetIdleAnim(E_WeaponType weapon, E_WeaponState weaponState)
	{
        return "idleSword";
	}

    public override string GetIdleActionAnim(E_WeaponType weapon, E_WeaponState weaponState)
    {
        return "idleTount";
    }

    public override string GetMoveAnim(E_MotionType motion, E_MoveType move, E_WeaponType weapon, E_WeaponState weaponState)
    {
        return "walkSword";
    }

    public override string GetRotateAnim(E_MotionType motionType, E_RotationType rotationType)
    {
        if (motionType == E_MotionType.Block)
        {
            if (rotationType == E_RotationType.Left)
                return "rotateBlockLeft";

            return "rotateBlockRight";
        }

        if (rotationType == E_RotationType.Left)
            return "rotateLeft";

        return "rotateRight";
    }


    public override string GetRollAnim(E_WeaponType weapon, E_WeaponState weaponState){return null;}


    public override string GetBlockAnim(E_BlockState state, E_WeaponType weapon)
    {
        if (state == E_BlockState.Start)
            return "blockStart";
        else if (state == E_BlockState.Loop)
            return "blockLoop";
        else if (state == E_BlockState.Failed)
            return "blockFailed";
        else if (state == E_BlockState.HitBlocked)
            return "blockHit";
        else
            return "blockEnd";
    }

    public override string GetShowWeaponAnim(E_WeaponType weapon)
    {
        return "";
    }

    public override string GetHideWeaponAnim(E_WeaponType weapon)
    {
        return "";
    }


    public override AnimAttackData GetFirstAttackAnim(E_WeaponType weapom, E_AttackType attackType)
    {
        if (attackType == E_AttackType.X)
            return AnimAttacksSwordL;


        return AnimAttacksSwordS;
    }

    public override AnimAttackData GetChainedAttackAnim(AnimAttackData parent, E_AttackType attackType)
    {
        return null;
    }


    public override string GetUseAnim(E_InteractionObjects objectType, E_InteractionType interaction){return null;}


    public override string GetInjuryAnim(E_WeaponType weapon, E_DamageType type)
    {
        return "injury";

/*        if(type == E_DamageType.Back)
            return "injuryBack";

        string[] anims = { "injuryFront01", "injuryFront02", "injuryFront03", "injuryFront04" };

        return anims[Random.Range(0, anims.Length)];*/
    }

    public override string GetDeathAnim(E_WeaponType weapon, E_DamageType type)
    {
       // string[] anims = { "death01", "death02"};

        return "death";

      //  return anims[Random.Range(0, 100) % anims.Length];
    }

    public override string GetKnockdowAnim(E_KnockdownState state, E_WeaponType weapon)
    {
        return "";
    }


}
