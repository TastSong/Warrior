using UnityEngine;

public class AnimStateRotate : AnimState
{
	AgentActionRotate Action;

    Quaternion FinalRotation;
    Quaternion StartRotation;
    float CurrentRotationTime;
    float RotationTime;
    float EndOfStateTime;
    string AnimName; 

	public AnimStateRotate(Animation anims, Agent owner)
		: base(anims, owner)
	{
	}


	override public void OnActivate(AgentAction action)
	{
        base.OnActivate(action);

        Owner.BlackBoard.MotionType = E_MotionType.None;
        Owner.BlackBoard.MoveDir = Vector3.zero;
        Owner.BlackBoard.Speed = 0;


      //  Time.timeScale = .1f;
	}

	override public void OnDeactivate()
	{
 //       Time.timeScale = 1;

        //AnimEngine.Stop(AnimName);
		Action.SetSuccess();
		Action = null;
        base.OnDeactivate();
	}

    Vector3 tmp;
    override public void Update()
    {
     //   if (Action.Target != null)
       //     UpdateFinalRotation();
        //Debug.DrawLine(OwnerTransform.position + new Vector3(0, 1, 0), OwnerTransform.position + new Vector3(0, 1, 0) + tmp * 5 );

        CurrentRotationTime += Time.deltaTime * Action.RotationModifier;
        
        if (CurrentRotationTime >= RotationTime)
        {
                CurrentRotationTime = RotationTime;
        }
        
        float progress = CurrentRotationTime / RotationTime;
        Quaternion q = Quaternion.Lerp(StartRotation, FinalRotation, Mathfx.Hermite(0,1,progress));

       // Debug.Log(q.ToString() + " " + StartRotation.ToString() + " " + FinalRotation.ToString() + " " + progress);

        Owner.Transform.rotation = q;

        if (EndOfStateTime <= Time.timeSinceLevelLoad)
            Release();
    }

	override public bool HandleNewAction(AgentAction action)
	{
        if (action is AgentActionRotate)
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

        Action = action as AgentActionRotate;

        CurrentRotationTime = 0;

		StartRotation = Transform.rotation;

        Vector3 finalDir;
        if (Action.Target != null)
        {
            finalDir = (Action.Target.Position + (Action.Target.BlackBoard.MoveDir * Action.Target.BlackBoard.Speed * 0.5f)) - Transform.position;
            finalDir.Normalize();
        }
        else if (Action.Direction != Vector3.zero)
        {
            finalDir = Action.Direction;
        }
        else
            finalDir = Transform.forward;

        

        if(Vector3.Dot(finalDir, Transform.right) > 0)
            AnimName = Owner.AnimSet.GetRotateAnim(Owner.BlackBoard.MotionType, E_RotationType.Right);
        else
            AnimName = Owner.AnimSet.GetRotateAnim(Owner.BlackBoard.MotionType, E_RotationType.Left);

        CrossFade(AnimName, 0.01f);
      
        FinalRotation.SetLookRotation(finalDir);

        RotationTime =  Vector3.Angle(Transform.forward, finalDir) / (360.0f * Owner.BlackBoard.RotationSmooth);

        if (RotationTime == 0)
            Release();

        float animLen = AnimEngine[AnimName].length;
        int steps = Mathf.CeilToInt(RotationTime / animLen);
        
        EndOfStateTime = AnimEngine[AnimName].length * steps + Time.timeSinceLevelLoad;

        //Debug.Log(steps + " " + RotationTime + " " + AnimEngine[AnimName].length);
	}


    void UpdateFinalRotation()
    {
        Vector3 dir = Action.Target.Position - Owner.Position;
        dir.Normalize();

        FinalRotation.SetLookRotation(dir);
        StartRotation = Owner.Transform.rotation;

        CurrentRotationTime = 0;
        RotationTime = Vector3.Angle(Transform.forward, dir) / 360.0f;
    }

}
