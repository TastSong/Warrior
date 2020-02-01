using UnityEngine;

public class AnimStateAttackWhirl : AnimState
{
	AgentActionAttackWhirl Action;
	float MaxSpeed;
    float TimeToEndState;
    bool MoveOk;

    CombatEffectsManager.CacheData Effect;
    float TimeToStartEffect;
    float TimeToEndEffect;
         
    

	Quaternion FinalRotation = new Quaternion();
	Quaternion StartRotation = new Quaternion();

    float NoHitTimer;
   // Vector3 FinalPosition;

	float RotationProgress;


    public AnimStateAttackWhirl(Animation anims, Agent owner)
		: base(anims, owner)
	{
	}


	override public void OnActivate(AgentAction action)
	{
        base.OnActivate(action);

        Effect = null;
	}

	override public void OnDeactivate()
	{
        if (Action != null)
        {
            Action.SetSuccess();
            Action = null;
        }

        Owner.BlackBoard.Speed = 0;
        if (Effect != null)
            CombatEffectsManager.Instance.ReturnWhirlEffect(Effect);

        Effect = null;

        base.OnDeactivate();

       // Time.timeScale = 1;
	}

	override public void Update()
	{
        UpdateFinalRotation();
//		Debug.DrawLine(OwnerTransform.position + new Vector3(0, 1, 0), FinalPosition + new Vector3(0, 1, 0));
        
		RotationProgress += Time.deltaTime * Owner.BlackBoard.RotationSmoothInMove;
		RotationProgress = Mathf.Min(RotationProgress, 1);
		Quaternion q = Quaternion.Slerp(StartRotation, FinalRotation, RotationProgress );
		Owner.Transform.rotation = q;
        
        if (MoveOk && AnimEngine[Action.Data.AnimName].time > AnimEngine[Action.Data.AnimName].length * 0.1f)
        {
            // Smooth the speed based on the current target direction
            float curSmooth = Owner.BlackBoard.SpeedSmooth * Time.deltaTime;

            Owner.BlackBoard.Speed = Mathfx.Hermite(Owner.BlackBoard.Speed, MaxSpeed, curSmooth);
            Owner.BlackBoard.MoveDir = Owner.Forward;

            float dist = Owner.BlackBoard.Speed * Time.deltaTime;
            MoveOk = Move(Owner.BlackBoard.MoveDir * dist);

            if (NoHitTimer < Time.timeSinceLevelLoad)
            {
				//MissionZone.Instance.CurrentGameZone.DoDamageToPlayer(Owner, Action.Data, Owner.BlackBoard.WeaponRange);
                NoHitTimer = Time.timeSinceLevelLoad + 0.75f;
            }
        }

        if (Effect == null && Time.timeSinceLevelLoad > TimeToStartEffect && Time.timeSinceLevelLoad < TimeToEndEffect)
        {
            Effect = CombatEffectsManager.Instance.PlayWhirlEffect(Transform);
        }
        else if (Effect != null && Time.timeSinceLevelLoad > TimeToEndEffect)
        {
            CombatEffectsManager.Instance.ReturnWhirlEffect(Effect);
            Effect = null;
        }


        if (TimeToEndState < Time.timeSinceLevelLoad)
            Release();
	}

	override public bool HandleNewAction(AgentAction action)
	{
        return false;
	}

	protected override void Initialize(AgentAction action)
	{
        base.Initialize(action);

        MoveOk = true;

        Action = action as AgentActionAttackWhirl;

        CrossFade(Action.Data.AnimName, 0.2f);

        UpdateFinalRotation();
       
        Owner.BlackBoard.MotionType = E_MotionType.Walk;

		RotationProgress = 0;

        TimeToEndState = AnimEngine[Action.Data.AnimName].length * 0.9f + Time.timeSinceLevelLoad;
        NoHitTimer = Time.timeSinceLevelLoad + 0.75f;

		Owner.Sound.PlayLoopSound(Owner.Sound.BerserkSound, 1, AnimEngine[Action.Data.AnimName].length - 1, 0.5f, 0.9f);

        TimeToStartEffect = Time.timeSinceLevelLoad + 1;
        TimeToEndEffect = Time.timeSinceLevelLoad + AnimEngine[Action.Data.AnimName].length - 1;

        MaxSpeed = 2;
	}


    void UpdateFinalRotation()
    {
        Vector3 dir = Player.Instance.Agent.Position - Owner.Position;
        dir.Normalize();

        FinalRotation.SetLookRotation(dir);
        StartRotation = Owner.Transform.rotation;

        if (StartRotation != FinalRotation)
           RotationProgress = 0;
    }
}
