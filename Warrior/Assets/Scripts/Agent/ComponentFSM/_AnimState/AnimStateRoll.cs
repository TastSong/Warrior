using UnityEngine;

public class AnimStateRoll : AnimState
{
	AgentActionRoll Action;

    Quaternion FinalRotation;
    Quaternion StartRotation;
    Vector3 StartPosition;
    Vector3 FinalPosition;
    float CurrentRotationTime;
    float RotationTime;
    float CurrentMoveTime;
    float MoveTime;
    float EndOfStateTime;
    float BlockEndTime;

    bool RotationOk = false;
    bool PositionOK = false;

    ParticleEmitter Effect;


	public AnimStateRoll(Animation anims, Agent owner)
		: base(anims, owner)
	{
        Effect = owner.Transform.Find("sust").GetComponent<ParticleEmitter>();
	}


	override public void OnActivate(AgentAction action)
	{
        base.OnActivate(action);



  //     Time.timeScale = .7f;
	}

	override public void OnDeactivate()
	{
  //      Time.timeScale = 1;

		Action.SetSuccess();
		Action = null;
        base.OnDeactivate();
	}

    override public void Update()
    {
        if (RotationOk == false)
        {
            CurrentRotationTime += Time.deltaTime;

            if (CurrentRotationTime >= RotationTime)
            {
                CurrentRotationTime = RotationTime;
                RotationOk = true;
            }

            float progress = CurrentRotationTime / RotationTime;
            Quaternion q = Quaternion.Lerp(StartRotation, FinalRotation, progress);
            Owner.Transform.rotation = q;
        }

        if (PositionOK == false)// && (RotationOk || (Quaternion.Angle(Owner.Transform.rotation, FinalRotation) > 40.0f))
        {
            CurrentMoveTime += Time.deltaTime;
            if (CurrentMoveTime >= MoveTime)
            {
                CurrentMoveTime = MoveTime;
                PositionOK = true;
            }

            float progress = CurrentMoveTime / MoveTime;
            Vector3 finalPos = Mathfx.Hermite(StartPosition, FinalPosition, progress);
            //MoveTo(finalPos);
            if (Move(finalPos - Transform.position) == false)
                PositionOK = true;
        }

        if (EndOfStateTime <= Time.timeSinceLevelLoad)
            Release();
    }

	override public bool HandleNewAction(AgentAction action)
	{
        if (action is AgentActionRoll)
		{
			if (Action != null)
				Action.SetSuccess();


            Initialize(action);

			return true;
		}
		return false;
	}


	protected override void Initialize(AgentAction action)
	{
        base.Initialize(action);

        Action = action as AgentActionRoll;

        CurrentMoveTime = 0;
        CurrentRotationTime = 0;

		StartRotation = Transform.rotation;
        StartPosition = Transform.position;

        Vector3 finalDir; 
        if (Action.ToTarget != null)
        {
            finalDir = Action.ToTarget.Position - Transform.position;
            finalDir.Normalize();

            FinalPosition = Action.ToTarget.Position - finalDir * Owner.BlackBoard.WeaponRange;
        }
        else
        {
            finalDir = Action.Direction;
            FinalPosition = StartPosition + Action.Direction * Owner.BlackBoard.RollDistance;
        }

        string AnimName = Owner.AnimSet.GetRollAnim(Owner.BlackBoard.WeaponSelected, Owner.BlackBoard.WeaponState);
        CrossFade(AnimName, 0.1f);
      

        FinalRotation.SetLookRotation(finalDir);

        RotationTime = Vector3.Angle(Transform.forward, finalDir) / 1000.0f;
        MoveTime = AnimEngine[AnimName].length * 0.85f;
        EndOfStateTime = AnimEngine[AnimName].length * 0.9f + Time.timeSinceLevelLoad;

        RotationOk = RotationTime == 0;
        PositionOK = false;

        Owner.BlackBoard.MotionType = E_MotionType.Roll;

        if (Effect)
            CombatEffectsManager.Instance.StartCoroutine(CombatEffectsManager.Instance.PlayAndStop(Effect, 0.1f));
	}
}
