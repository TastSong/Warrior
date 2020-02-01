using UnityEngine;

public class AnimStatePlayAnim : AnimState
{

    AgentAction Action = null;

    float EndOfStateTime;
    public bool LookAtTarget;
    Quaternion FinalRotation;
    Quaternion StartRotation;
    float CurrentRotationTime;
    float RotationTime;
    bool RotationOk = false;


    public AnimStatePlayAnim(Animation anims, Agent owner)
		: base(anims, owner)
	{
	}


	override public void OnActivate(AgentAction action)
	{
        base.OnActivate(action);

       //Time.timeScale = .1f;

       //Debug.Log(this.ToString() + " Aactivate");
        Owner.BlackBoard.MotionType = E_MotionType.AnimationDrive;
        Owner.BlackBoard.MoveDir = Vector3.zero;
        Owner.BlackBoard.Speed = 0;
	}

	override public void OnDeactivate()
	{
      //  Time.timeScale = 1;
        LookAtTarget = false;
		Action.SetSuccess();
		Action = null;
        base.OnDeactivate();

        //Debug.Log(this.ToString() + " Deactivate");
	}

    override public void Update()
    {
 //       //Debug.DrawLine(OwnerTransform.position + new Vector3(0, 1, 0), OwnerTransform.position + Action.Direction + new Vector3(0, 1, 0));
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
        if (EndOfStateTime <= Time.timeSinceLevelLoad)
            Release();
    }

	override public bool HandleNewAction(AgentAction action)
	{
        if (action is AgentActionPlayAnim)
		{
            if (Action != null)
                action.SetFailed();
		}
    	return false;
	}

    protected override void Initialize(AgentAction action)
    {
        base.Initialize(action);

        Action = action;

        string animName = null;
        if (Action is AgentActionPlayAnim)
        {
            animName = (Action as AgentActionPlayAnim).AnimName;
            LookAtTarget = false;
        }
        else if (Action is AgentActionPlayIdleAnim)
        {
            animName = Owner.AnimSet.GetIdleActionAnim(Owner.BlackBoard.WeaponSelected, Owner.BlackBoard.WeaponState);
            LookAtTarget = true;
        }

        if (animName == null)
        {
            Action.SetFailed();
            Action = null;
            Release();
            return;
        }

        RotationOk = true;
                // play owner anims
        AnimEngine.Play(animName, AnimationPlayMode.Stop);

        //end of state
        EndOfStateTime = (AnimEngine[animName].length * 0.9f) + Time.timeSinceLevelLoad;

        // Debug.Log(Action.AnimName + " " + EndOfStateTime );
        
    }


    void UpdateFinalRotation()
    {
        if (Owner.BlackBoard.DesiredTarget == null)
            return;

        Vector3 dir =  Owner.BlackBoard.DesiredTarget.Position - Owner.Position;
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
