using UnityEngine;

public class AnimStateAttackRoll : AnimState
{
    enum E_State
    {
        Prepare,
        Roll,
        End,
    }

    Quaternion FinalRotation;
    Quaternion StartRotation;

    float CurrentRotationTime;
    float RotationTime;
    float EndOfStateTime;
    float NoHitTimer;

    
    CombatEffectsManager.CacheData Effect;

    AgentActionAttackRoll Action;

    bool RotationOk = false;
    E_State State;



	public AnimStateAttackRoll(Animation anims, Agent owner)
		: base(anims, owner)
	{
	}

    override public void OnActivate(AgentAction action)
    {
        base.OnActivate(action);

        Effect = null;
      //  Time.timeScale = 0.2f;
    }

    override public void OnDeactivate()
    {
    //    Time.timeScale = 1;

		 Action.SetSuccess();
         Action = null;

         if (Effect != null)
             CombatEffectsManager.Instance.ReturnRolllEffect(Effect);

         Effect = null;
         base.OnDeactivate();
    }


    override public bool HandleNewAction(AgentAction action)
    {
		 if (action as AgentActionAttackRoll != null)
		 {
             action.SetFailed();
			 return true;
		 }
		 return false;
    }

    override public void Update()
    {
        switch (State)
        {
            case E_State.Prepare:
                {
                    UpdateFinalRotation();

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

                    if (EndOfStateTime < Time.timeSinceLevelLoad)
                        InitializeRoll();


                }
                break;
            case E_State.Roll:
                {
//				MissionZone.Instance.CurrentGameZone.EnemiesRecvImpuls(Owner, 45, 2.5f, 2);
//                    if (Move(Transform.forward * 15 * Time.deltaTime, false) == false)
//                    {
//					MissionZone.Instance.CurrentGameZone.DoDamageToPlayer(Owner, Action.Data, 1.5f);
//                        InitializeStandUp();
//                    }
//                    else if (NoHitTimer < Time.timeSinceLevelLoad)
//                    {
//					MissionZone.Instance.CurrentGameZone.DoDamageToPlayer(Owner, Action.Data, 1.5f);
//                        NoHitTimer = Time.timeSinceLevelLoad + 0.2f;
//                    }

                }
                break;
            case E_State.End:
                {
                    if (EndOfStateTime < Time.timeSinceLevelLoad)
                        Release();
                }
                break;
        }
    }

    override protected void Initialize(AgentAction action)
    {
        Action = action as AgentActionAttackRoll;

        State = E_State.Prepare;

        CrossFade("attackRollStart", 0.4f);

        base.Initialize(action);

        Owner.BlackBoard.MotionType = E_MotionType.None;

        EndOfStateTime = AnimEngine["attackRollStart"].length * 0.95f + Time.timeSinceLevelLoad;
        NoHitTimer = 0;

        UpdateFinalRotation();

		Owner.Sound.PlayRoll();

    }

    void InitializeRoll()
    {
        /*LayerMask mask = 11;

        Owner.GameObject.layer = mask.value;*/

        State = E_State.Roll;

        CrossFade("attackRollLoop", 0.1f);

        Owner.BlackBoard.MotionType = E_MotionType.Roll;

        Effect = CombatEffectsManager.Instance.PlayRollEffect(Transform);
    }

    void InitializeStandUp()
    {
       /* LayerMask mask = 8;

        Owner.GameObject.layer = mask.value;*/

        State = E_State.End;

        CrossFade("attackRollEnd", 0.1f);

        Owner.BlackBoard.MotionType = E_MotionType.Roll;

        EndOfStateTime = AnimEngine["attackRollEnd"].length * 0.95f + Time.timeSinceLevelLoad;


        CombatEffectsManager.Instance.ReturnRolllEffect(Effect);
        Effect = null;

		Owner.Sound.PlayRoll();
    }

    void UpdateFinalRotation()
    {
        if (Action.Target == null)
            return;

        Vector3 dir = Action.Target.Position - Owner.Position;
        dir.Normalize();

        StartRotation = Owner.Transform.rotation;
        FinalRotation.SetLookRotation(dir);
        
        float angle = Vector3.Angle(Transform.forward, dir);

        if (angle > 0)
        {
            RotationTime = angle / 100.0f;
            RotationOk = false;
            CurrentRotationTime = 0;
        }
        else
        {
            RotationOk = true;
            RotationTime = 0;
        }
    }

}
