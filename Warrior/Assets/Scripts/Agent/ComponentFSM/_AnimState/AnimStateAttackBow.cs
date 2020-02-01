using UnityEngine;

public class AnimStateAttackBow : AnimState
{
    AgentActionAttack Action;
    AnimAttackData AnimAttackData;

    Quaternion FinalRotation;
    Quaternion StartRotation;
    float CurrentRotationTime;
    float RotationTime;
    float EndOfStateTime;
    float FireTime;
    bool RotationOk = false;


    public AnimStateAttackBow(Animation anims, Agent owner)
		: base(anims, owner)
	{

	}

    override public void OnActivate(AgentAction action)
    {
       // Time.timeScale = 0.1f;
        base.OnActivate(action);

    }

    override public void OnDeactivate()
    {
        Time.timeScale = 1.0f;

		 Action.SetSuccess();
         Action = null;

         base.OnDeactivate();
    }


    override public bool HandleNewAction(AgentAction action)
    {
		 if (action as AgentActionAttack != null)
		 {
             if (Action != null)
                 Action.SetSuccess();

             Initialize(action);
			 return true;
		 }
		 return false;
    }

    override public void Update()
    {
         UpdateFinalRotation();

         //Debug.DrawLine(Transform.position + Vector3.up * 1.5f, Transform.position + Vector3.up * 1.5f + Transform.forward * 5);
        
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

        if (Action.Hit == false && FireTime <= Time.timeSinceLevelLoad)
        {
            Action.Hit = true;
            ProjectileManager.Instance.SpawnArrow(Owner, Transform.position + Vector3.up * 1.5f, Transform.forward, 15, AnimAttackData.HitDamage);
			Owner.Sound.PlayAttackHit();
        }

        if (EndOfStateTime <= Time.timeSinceLevelLoad)
            Release();


    }

    private void PlayAnim()
    {
        CrossFade(AnimAttackData.AnimName, 0.1f);
   //     AnimEngine[AnimAttackData.AnimName].speed = 0.9f;

        //end of state
        if (AnimEngine[AnimAttackData.AnimName].length > AnimAttackData.AttackEndTime  )
            EndOfStateTime = Time.timeSinceLevelLoad + AnimEngine[AnimAttackData.AnimName].length;
        else
            EndOfStateTime = Time.timeSinceLevelLoad + AnimAttackData.AttackEndTime;

        // when to do hit !!!
        FireTime = Time.timeSinceLevelLoad + AnimAttackData.HitTime;

        Owner.BlackBoard.MotionType = E_MotionType.Attack;
    }

    override protected void Initialize(AgentAction action)
    {
        base.Initialize(action);

        Action = action as AgentActionAttack;

        SetFinished(false);

        if (Action.Data == null)
            Action.Data = Owner.AnimSet.GetFirstAttackAnim(Owner.BlackBoard.WeaponSelected, Action.AttackType);
        
        AnimAttackData = Action.Data;

        StartRotation = Transform.rotation;

        Action.AttackPhaseDone = false;
        Action.Hit = false;

        float angle = 0;

        if (Action.Target != null)
        {
            Vector3 dir = Action.Target.Position - Transform.position;
            //float distance = dir.magnitude;

            if (dir.sqrMagnitude > 0.1f * 0.1f)
            {
                dir.Normalize();
                angle = Vector3.Angle(Transform.forward, dir);
            }
            else
            {
                dir = Transform.forward;
            }

            FinalRotation.SetLookRotation(dir);
            RotationTime = angle / 180.0f;
        }
        else
        {
            FinalRotation.SetLookRotation(Action.AttackDir);
            RotationTime = Vector3.Angle(Transform.forward, Action.AttackDir) / 1040.0f;
        }
        RotationOk = RotationTime == 0;

        CurrentRotationTime = 0;
        PlayAnim();
    }

    void UpdateFinalRotation()
    {
        if (Action.Target == null)
            return;

        Vector3 dir = Action.Target.Position - Owner.Position;
        dir.Normalize();

        FinalRotation.SetLookRotation(dir);
        StartRotation = Owner.Transform.rotation;
        float angle = Vector3.Angle(Transform.forward, dir);

        if (angle > 0)
        {
            RotationTime = angle / 100.0f;
            RotationOk = false;
            CurrentRotationTime = 0;
        }
    }
}
