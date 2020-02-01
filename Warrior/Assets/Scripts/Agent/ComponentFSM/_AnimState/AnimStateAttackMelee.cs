using UnityEngine;

public class AnimStateAttackMelee : AnimState
{
	enum E_State
	{
		E_PREPARING,
		E_ATTACKING,
		E_FINISHED,
	}


	AgentActionAttack Action;
	AnimAttackData AnimAttackData;

	Quaternion FinalRotation;
	Quaternion StartRotation;
	Vector3 StartPosition;
	Vector3 FinalPosition;
	float CurrentRotationTime;
	float RotationTime;
	float MoveTime;
	float CurrentMoveTime;
	float EndOfStateTime;
	float HitTime;
	float AttackPhaseTime;

	bool RotationOk = false;
	bool PositionOK = false;
	// bool MovingToAttackPos;

	// 危险的
	bool Critical = false;
	// 击倒的
	bool Knockdown = false;

	E_State State;

	public AnimStateAttackMelee (Animation anims, Agent owner)
		: base (anims, owner)
	{

	}

	override public void OnActivate (AgentAction action)
	{
		/*if(Owner.IsPlayer == false)
            Time.timeScale = 0.2f;*/

		base.OnActivate (action);

	}

	override public void OnDeactivate ()
	{
		Action.SetSuccess ();
		Action = null;
        
		base.OnDeactivate ();
	}


	override public bool HandleNewAction (AgentAction action)
	{
		if (action as AgentActionAttack != null) {
			if (Action != null) {
				Action.AttackPhaseDone = true;
				Action.SetSuccess ();
			}

			Initialize (action);
			return true;
		}
		return false;
	}

	override public void Update ()
	{
		if (State == E_State.E_PREPARING) {
			bool dontMove = false;
			if (RotationOk == false) {
				//Debug.Log("rotate");
				CurrentRotationTime += Time.deltaTime;

				if (CurrentRotationTime >= RotationTime) {
					CurrentRotationTime = RotationTime;
					RotationOk = true;
				}

				float progress = CurrentRotationTime / RotationTime;
				Quaternion q = Quaternion.Lerp (StartRotation, FinalRotation, progress);
				Owner.Transform.rotation = q;

				if (Quaternion.Angle (q, FinalRotation) > 20.0f)
					dontMove = true;
			}

			if (dontMove == false && PositionOK == false) {
				CurrentMoveTime += Time.deltaTime;
				if (CurrentMoveTime >= MoveTime) {
					CurrentMoveTime = MoveTime;
					PositionOK = true;
				}

				if (CurrentMoveTime > 0) {
					float progress = CurrentMoveTime / MoveTime;
					Vector3 finalPos = Mathfx.Hermite (StartPosition, FinalPosition, progress);
					//if (MoveToCollideWithEnemy(finalPos, Transform.forward) == false)
					if (Move (finalPos - Transform.position) == false) {
						PositionOK = true;
					}
				}
			}

			if (RotationOk && PositionOK) {
				State = E_State.E_ATTACKING;
				PlayAnim ();
			}
		} else if (State == E_State.E_ATTACKING) {
			CurrentMoveTime += Time.deltaTime;

			if (AttackPhaseTime < Time.timeSinceLevelLoad) {
				Action.AttackPhaseDone = true;
				State = E_State.E_FINISHED;
			}

			if (CurrentMoveTime >= MoveTime)
				CurrentMoveTime = MoveTime;

			if (CurrentMoveTime > 0 && CurrentMoveTime <= MoveTime) {
				float progress = Mathf.Min (1.0f, CurrentMoveTime / MoveTime);
				Vector3 finalPos = Mathfx.Hermite (StartPosition, FinalPosition, progress);
				//if (MoveToCollideWithEnemy(finalPos, Transform.forward) == false)
				if (Move (finalPos - Transform.position, false) == false) {
					CurrentMoveTime = MoveTime;
				}

			}

			if (Action.Hit == false && HitTime <= Time.timeSinceLevelLoad) {
				Action.Hit = true;

				if (Owner.IsPlayer && AnimAttackData.FullCombo)
//                GuiManager.Instance.ShowComboMessage(AnimAttackData.ComboIndex);

		MissionZone.Instance.CurrentGameZone.BreakBreakableObjects (Owner);
                
				if (Action.AttackType == E_AttackType.Fatality)
					MissionZone.Instance.CurrentGameZone.DoDamageFatality (Owner, Action.Target, Owner.BlackBoard.WeaponSelected, AnimAttackData);
				else
					MissionZone.Instance.CurrentGameZone.DoMeleeDamage (Owner, Action.Target, Owner.BlackBoard.WeaponSelected, AnimAttackData, Critical, Knockdown);

				if (AnimAttackData.LastAttackInCombo || AnimAttackData.ComboStep == 3)
					CameraBehaviour.Instance.ComboShake (AnimAttackData.ComboStep - 3);

				if (AnimAttackData.LastAttackInCombo)
					Owner.StartCoroutine (ShowTrail (AnimAttackData, 1, 0.3f, Critical, MoveTime - Time.timeSinceLevelLoad));
				else
					Owner.StartCoroutine (ShowTrail (AnimAttackData, 2, 0.1f, Critical, MoveTime - Time.timeSinceLevelLoad));

			}
		} else if (State == E_State.E_FINISHED && EndOfStateTime <= Time.timeSinceLevelLoad) {
			Action.AttackPhaseDone = true;
			Release ();
		}
	}

	private void PlayAnim ()
	{
		CrossFade (AnimAttackData.AnimName, 0.2f);

		// when to do hit !!!
		HitTime = Time.timeSinceLevelLoad + AnimAttackData.HitTime;

		StartPosition = Transform.position;
		FinalPosition = StartPosition + Transform.forward * AnimAttackData.MoveDistance;
		MoveTime = AnimAttackData.AttackMoveEndTime - AnimAttackData.AttackMoveStartTime;

		EndOfStateTime = Time.timeSinceLevelLoad + AnimEngine [AnimAttackData.AnimName].length * 0.9f;

		if (AnimAttackData.LastAttackInCombo)
			AttackPhaseTime = Time.timeSinceLevelLoad + AnimEngine [AnimAttackData.AnimName].length * 0.9f;
		else
			AttackPhaseTime = Time.timeSinceLevelLoad + AnimAttackData.AttackEndTime;

		CurrentMoveTime = -AnimAttackData.AttackMoveStartTime; // move a little bit later

		if (Action.Target && Action.Target.IsAlive) {
			if (Critical) {
				CameraBehaviour.Instance.InterpolateTimeScale (0.25f, 0.5f);
				CameraBehaviour.Instance.InterpolateFov (25, 0.5f);
				CameraBehaviour.Instance.Invoke ("InterpolateScaleFovBack", 0.7f);
			} else if (Action.AttackType == E_AttackType.Fatality) {
				CameraBehaviour.Instance.InterpolateTimeScale (0.25f, 0.7f);
				CameraBehaviour.Instance.InterpolateFov (25, 0.65f);
				CameraBehaviour.Instance.Invoke ("InterpolateScaleFovBack", 0.8f);
			}
		}
	}

	override protected void Initialize (AgentAction action)
	{
		base.Initialize (action);
		SetFinished (false);

		State = E_State.E_PREPARING;
		Owner.BlackBoard.MotionType = E_MotionType.Attack;

		Action = action as AgentActionAttack;
		Action.AttackPhaseDone = false;
		Action.Hit = false;

		if (Action.Data == null)
			Action.Data = Owner.AnimSet.GetFirstAttackAnim (Owner.BlackBoard.WeaponSelected, Action.AttackType);

		AnimAttackData = Action.Data;

		if (AnimAttackData == null)
			Debug.LogError ("AnimAttackData == null");

		StartRotation = Transform.rotation;
		StartPosition = Transform.position;

		float angle = 0;

		// 背刺
		bool backstab = false;

		float distance = 0;
		if (Action.Target != null) {
			Vector3 dir = Action.Target.Position - Transform.position;
			distance = dir.magnitude;

			if (distance > 0.1f) {
				dir.Normalize ();
				angle = Vector3.Angle (Transform.forward, dir);

				//attacker uhel k cili je mensi nez 40 and uhel enemace a utocnika je mensi nez 80 stupnu
				if (angle < 40 && Vector3.Angle (Owner.Forward, Action.Target.Forward) < 80)
					backstab = true;
			} else {
				dir = Transform.forward;
			}


			FinalRotation.SetLookRotation (dir);

			if (distance < Owner.BlackBoard.WeaponRange)
				FinalPosition = StartPosition;
			else
				FinalPosition = Action.Target.Transform.position - dir * Owner.BlackBoard.WeaponRange;
   
			MoveTime = (FinalPosition - StartPosition).magnitude / 20.0f;
			RotationTime = angle / 720.0f;
		} else {
			FinalRotation.SetLookRotation (Action.AttackDir);

			RotationTime = Vector3.Angle (Transform.forward, Action.AttackDir) / 720.0f;
			MoveTime = 0;
		}

		RotationOk = RotationTime == 0;
		PositionOK = MoveTime == 0;

		CurrentRotationTime = 0;
		CurrentMoveTime = 0;

		if (Owner.IsPlayer && AnimAttackData.HitCriticalType != E_CriticalHitType.None && Action.Target && Action.Target.BlackBoard.CriticalAllowed && Action.Target.IsBlocking == false && Action.Target.IsInvulnerable == false && Action.Target.IsKnockedDown == false) {
			if (backstab)
				Critical = true;
			else {
				Critical = Random.Range (0, 100) < Owner.GetCriticalChance () * AnimAttackData.CriticalModificator * Action.Target.BlackBoard.CriticalHitModifier;
			}
		} else
			Critical = false;

		Knockdown = AnimAttackData.HitAreaKnockdown && Random.Range (0, 100) < 60 * Owner.GetCriticalChance ();
	}
}
