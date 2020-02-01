using UnityEngine;

public class AnimStateCombatMove : AnimState
{
	AgentActioCombatMove Action;
	float MaxSpeed;
    float MovedDistance;

	Quaternion FinalRotation = new Quaternion();
	Quaternion StartRotation = new Quaternion();

   // Vector3 FinalPosition;

	float RotationProgress;


    public AnimStateCombatMove(Animation anims, Agent owner)
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
        if (Action != null)
        {
            Action.SetSuccess();
            Action = null;
        }

        Owner.BlackBoard.Speed = 0;

        base.OnDeactivate();

       // Time.timeScale = 1;
	}

	override public void Update()
	{
        UpdateFinalRotation();
        //UpdateFinalPosition();
        //UpdateMoveType();

//		Debug.DrawLine(OwnerTransform.position + new Vector3(0, 1, 0), FinalPosition + new Vector3(0, 1, 0));
        
		RotationProgress += Time.deltaTime * Owner.BlackBoard.RotationSmoothInMove;
		RotationProgress = Mathf.Min(RotationProgress, 1);
		Quaternion q = Quaternion.Slerp(StartRotation, FinalRotation, RotationProgress );
		Owner.Transform.rotation = q;

		if (Quaternion.Angle(q, FinalRotation) > 40.0f)
			return;

    	// Smooth the speed based on the current target direction
		float curSmooth = Owner.BlackBoard.SpeedSmooth * Time.deltaTime;

        Owner.BlackBoard.Speed = Mathf.Lerp(Owner.BlackBoard.Speed, MaxSpeed, curSmooth);

//            Owner.BlackBoard.MoveDir = FinalPosition - Owner.Position;
//            Owner.BlackBoard.MoveDir.Normalize();

        if (Action.MoveType == E_MoveType.Forward)
            Owner.BlackBoard.MoveDir = Owner.Forward;
        else if (Action.MoveType == E_MoveType.Backward)
            Owner.BlackBoard.MoveDir = -Owner.Forward;
        else if (Action.MoveType == E_MoveType.StrafeRight)
            Owner.BlackBoard.MoveDir = Owner.Right;
        else if (Action.MoveType == E_MoveType.StrafeLeft)
            Owner.BlackBoard.MoveDir = -Owner.Right;

        float dist = Owner.BlackBoard.Speed * Time.deltaTime;

        if (Move(Owner.BlackBoard.MoveDir * dist) == false)
            Release();
      
        MovedDistance +=  dist;
        if (MovedDistance > Action.DistanceToMove)
            Release();

        if (Action.MinDistanceToTarget > Owner.BlackBoard.DistanceToTarget)
            Release();
	}

	override public bool HandleNewAction(AgentAction action)
	{
		if (action is AgentActioCombatMove)
		{
			if (Action != null)
				Action.SetSuccess();

            SetFinished(false); // just for sure, if we already finish in same tick

			Initialize(action);

			return true;
		}

        if (action is AgentActionIdle)
        {
            action.SetSuccess();
            SetFinished(true); 
        }

        if (action is AgentActionWeaponShow)
        {
            action.SetSuccess();

            //Owner.ShowWeapon((action as AgentActionWeaponShow).Show, 0);

            UpdateMoveType();
            return true;
        }
        return false;
	}



	protected override void Initialize(AgentAction action)
	{
        base.Initialize(action);

        Action = action as AgentActioCombatMove;

       // Debug.Log(Action.MoveType + " Distance to move " + Action.DistanceToMove);

        UpdateFinalRotation();
        //UpdateFinalPosition();
        
        Owner.BlackBoard.MotionType = E_MotionType.Walk;

		RotationProgress = 0;
        MovedDistance = 0;

        Owner.BlackBoard.MoveType = E_MoveType.None;

        UpdateMoveType();
	}

    void UpdateFinalRotation()
    {
        if (Action.Target == null)
            return;

        Vector3 dir = Action.Target.Position - Owner.Position;
        dir.Normalize();

        FinalRotation.SetLookRotation(dir);
        StartRotation = Owner.Transform.rotation;

        if (StartRotation != FinalRotation)
           RotationProgress = 0;
    }
    /*
    void UpdateFinalPosition()
    {
        if (Action.MoveType == E_MoveType.E_STRAFE_RIGHT)
        {
            FinalPosition = Action.Target.Position - Action.Target.Right * Action.DistanceToTarget;
        }
        else if (Action.MoveType == E_MoveType.E_STRAFE_LEFT)
        {
            FinalPosition = Action.Target.Position + Action.Target.Right * Action.DistanceToTarget;
        }
        else
        {
            Vector3 dir = Owner.Position - Action.Target.Position;
            dir.Normalize();
            FinalPosition = Action.Target.Position + dir * Action.DistanceToTarget;
        }
    }*/

    void UpdateMoveType()
    {
        Owner.BlackBoard.MoveType = Action.MoveType;

/*        Vector3 dirToPos = FinalPosition - OwnerTransform.position;
        dirToPos.Normalize();

        float forwardAngleToPos = Vector3.Angle(OwnerTransform.forward,dirToPos); 
        float rightAngleToPos = Vector3.Angle(OwnerTransform.right,dirToPos); 

        E_MoveType oldMoveType = Owner.BlackBoard.MoveType;
        if (forwardAngleToPos < 30)
            Owner.BlackBoard.MoveType = E_MoveType.E_FORWARD;
        else if (forwardAngleToPos > 150)
            Owner.BlackBoard.MoveType = E_MoveType.E_BACKWARD;
        else if (rightAngleToPos < 30)
            Owner.BlackBoard.MoveType = E_MoveType.E_STRAFE_RIGHT;
        else if (rightAngleToPos > 150)
            Owner.BlackBoard.MoveType = E_MoveType.E_STRAFE_LEFT;
        else
            Owner.BlackBoard.MoveType = Action.MoveType;
        */
        
        //Debug.Log(Owner.BlackBoard.MoveType + " forwardAngle " + forwardAngleToPos + " right angle :" + rightAngleToPos);

       // if(oldMoveType != Owner.BlackBoard.MoveType)
        CrossFade(Owner.AnimSet.GetMoveAnim(Action.MotionType, Owner.BlackBoard.MoveType, Owner.BlackBoard.WeaponSelected, Owner.BlackBoard.WeaponState), 0.2f);

        MaxSpeed = Action.MotionType == E_MotionType.Run ? Owner.BlackBoard.MaxRunSpeed : Owner.BlackBoard.MaxCombatMoveSpeed;
    }
}
