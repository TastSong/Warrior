using UnityEngine;

public class AnimStateDeath : AnimState
{
    Vector3 StartPosition;
    Vector3 FinalPosition;
    Quaternion FinalRotation;
    Quaternion StartRotation;

    float RotationProgress;
    float MoveTime;
    float CurrentMoveTime;
    bool PositionOK = false;
    bool RotationOk = false;

    AgentActionDeath Action = null;


    public AnimStateDeath(Animation anims, Agent owner)
		: base(anims, owner)
	{
	}


	override public void OnActivate(AgentAction action)
	{
        base.OnActivate(action);

        Owner.BlackBoard.MotionType = E_MotionType.None;
        Owner.BlackBoard.MoveDir = Vector3.zero;
        Owner.BlackBoard.Speed = 0;

       //Time.timeScale = .1f;

    //   Debug.Log(this.ToString() + " Aactivate");
	}

    override public void Update()
    {
 //       //Debug.DrawLine(OwnerTransform.position + new Vector3(0, 1, 0), OwnerTransform.position + Action.Direction + new Vector3(0, 1, 0));
        if (RotationOk == false)
        {
            //Debug.Log("rotate");
            RotationProgress += Time.deltaTime * Owner.BlackBoard.RotationSmooth;

            if (RotationProgress >= 1)
            {
                RotationProgress = 1;
                RotationOk = true;
            }

            RotationProgress = Mathf.Min(RotationProgress, 1);
            Quaternion q = Quaternion.Lerp(StartRotation, FinalRotation, RotationProgress);
            Owner.Transform.rotation = q;
        }

        if (PositionOK == false)
        {
            CurrentMoveTime += Time.deltaTime;
            if (CurrentMoveTime >= MoveTime)
            {
                CurrentMoveTime = MoveTime;
                PositionOK = true;
            }

            float progress = Mathf.Min(1.0f, CurrentMoveTime / MoveTime);
            Vector3 finalPos = Mathfx.Sinerp(StartPosition, FinalPosition, progress);
            //MoveTo(finalPos);
            if (Move(finalPos - Transform.position) == false)
            {

                PositionOK = true;
            }
        }

    }

	override public void Release()
	{
	//	SetFinished(true);
	}

	override public bool HandleNewAction(AgentAction action)
	{
        if (action is AgentActionDeath)
		{
            action.SetFailed();
            return true;
		}
    	return false;
	}

    protected override void Initialize(AgentAction action)
    {
        base.Initialize(action);

        Action = action as AgentActionDeath;

        // play owner anims
        string animName = Owner.AnimSet.GetDeathAnim(Action.FromWeapon, Action.DamageType);

        CrossFade(animName, 0.1f);

       // Debug.Log(Action.AnimName + " " + EndOfStateTime );
        Owner.BlackBoard.MotionType = E_MotionType.None;

        StartPosition = Transform.position;

        if (Action.Attacker != null)
        {
            FinalPosition = StartPosition + Action.Attacker.Forward;

            StartRotation = Transform.rotation;
            FinalRotation.SetLookRotation(-Action.Attacker.Forward);

            PositionOK = false;
            RotationOk = false;

            RotationProgress = 0;
        }
        else
        {
            StartPosition = Transform.position;
            FinalPosition = StartPosition + Action.Impuls;

            PositionOK = false;
            RotationOk = true;
        }

        CurrentMoveTime = 0;
        MoveTime = AnimEngine[animName].length * 0.6f;

        Owner.Invoke("SpawnBlood", AnimEngine[animName].length);
        Owner.BlackBoard.MotionType = E_MotionType.Death;

        Owner.DisableCollisions();
    }   
}
