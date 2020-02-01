using UnityEngine;

public class AnimStateMove : AnimState
{
	AgentActionMove Action;
	float MaxSpeed;

	Quaternion FinalRotation = new Quaternion ();
	Quaternion StartRotation = new Quaternion ();
	float RotationProgress;


	public AnimStateMove (Animation anims, Agent owner)
		: base (anims, owner)
	{
	}


	override public void OnActivate (AgentAction action)
	{
		// Time.timeScale = 0.1f;
		base.OnActivate (action);
		PlayAnim (GetMotionType ());
	}

	override public void OnDeactivate ()
	{
		if (Action != null) {
			Action.SetSuccess ();
			Action = null;
		}

		Owner.BlackBoard.Speed = 0;

		base.OnDeactivate ();

		// Time.timeScale = 1;
	}

	override public void Update ()
	{
		//Debug.DrawLine(OwnerTransform.position + new Vector3(0, 1, 0), Action.FinalPosition + new Vector3(0, 1, 0));
        
		//if (Owner.debugAnims) Debug.Log(Time.timeSinceLevelLoad + " " + "Speed " + Owner.BlackBoard.Speed + " Max Speed " + Owner.BlackBoard.MaxWalkSpeed);
		if (Action.IsActive () == false) {
			Release ();
			return;
		}

		RotationProgress += Time.deltaTime * Owner.BlackBoard.RotationSmooth;
		RotationProgress = Mathf.Min (RotationProgress, 1);
		Quaternion q = Quaternion.Slerp (StartRotation, FinalRotation, RotationProgress);
		Owner.Transform.rotation = q;

		if (Quaternion.Angle (q, FinalRotation) > 40.0f)
			return;

		MaxSpeed = Mathf.Max (Owner.BlackBoard.MaxWalkSpeed, Owner.BlackBoard.MaxRunSpeed * Owner.BlackBoard.MoveSpeedModifier);

		// Smooth the speed based on the current target direction
		float curSmooth = Owner.BlackBoard.SpeedSmooth * Time.deltaTime;

		Owner.BlackBoard.Speed = Mathf.Lerp (Owner.BlackBoard.Speed, MaxSpeed, curSmooth);
		Owner.BlackBoard.MoveDir = Owner.BlackBoard.DesiredDirection;

		// MOVE
		if (Move (Owner.BlackBoard.MoveDir * Owner.BlackBoard.Speed * Time.deltaTime) == false)
			Release ();

		E_MotionType motion = GetMotionType ();
		if (motion != Owner.BlackBoard.MotionType)
			PlayAnim (motion);
		
	}

	override public bool HandleNewAction (AgentAction action)
	{
		if (action is AgentActionMove) {
			if (Action != null)
				Action.SetSuccess ();

			SetFinished (false); // just for sure, if we already finish in same tick

			Initialize (action);

			return true;
		}

		if (action is AgentActionIdle) {
			action.SetSuccess ();

			SetFinished (true); 
		}

		if (action is AgentActionWeaponShow) {
			action.SetSuccess ();

//            Owner.ShowWeapon((action as AgentActionWeaponShow).Show, 0);
  
			PlayAnim (GetMotionType ());
			return true;
		}
		return false;
	}

	private void PlayAnim (E_MotionType motion)
	{
		Owner.BlackBoard.MotionType = motion;

		CrossFade (Owner.AnimSet.GetMoveAnim (Owner.BlackBoard.MotionType, E_MoveType.Forward, Owner.BlackBoard.WeaponSelected, Owner.BlackBoard.WeaponState), 0.2f);
	}

	private E_MotionType GetMotionType ()
	{
		if (Owner.BlackBoard.Speed > Owner.BlackBoard.MaxRunSpeed * 1.5f)
			return E_MotionType.Sprint;
		else if (Owner.BlackBoard.Speed > Owner.BlackBoard.MaxWalkSpeed * 1.5f)
			return E_MotionType.Run;
        
		return E_MotionType.Walk;
	}

	protected override void Initialize (AgentAction action)
	{
		base.Initialize (action);

		Action = action as AgentActionMove;

		FinalRotation.SetLookRotation (Owner.BlackBoard.DesiredDirection);

		StartRotation = Owner.Transform.rotation;

		Owner.BlackBoard.MotionType = GetMotionType ();

		RotationProgress = 0;
	}
}
