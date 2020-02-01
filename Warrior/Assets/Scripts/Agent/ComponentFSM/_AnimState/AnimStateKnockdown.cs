using UnityEngine;

//击倒状态
public class AnimStateKnockdown : AnimState
{
    enum E_State
    {
        Start,
        Loop,
        Fatality,
        Death,
        End,
    }

    AgentActionKnockdown Action;
    AgentActionDeath ActionDeath;

    Quaternion FinalRotation;
    Quaternion StartRotation;
    Vector3 StartPosition;
    Vector3 FinalPosition;
    float CurrentRotationTime;
    float RotationTime;
    float CurrentMoveTime;
    float MoveTime;
    float EndOfStateTime;
    float KnockdownEndTime;

    bool RotationOk = false;
    bool PositionOK = false;



    E_State State;

    public AnimStateKnockdown(Animation anims, Agent owner)
		: base(anims, owner)
	{

	}

    override public void OnActivate(AgentAction action)
    {
      //  Time.timeScale = 0.1f;
        base.OnActivate(action);

        Owner.BlackBoard.MotionType = E_MotionType.Knockdown;
        Owner.BlackBoard.MoveDir = Vector3.zero;
        Owner.BlackBoard.Speed = 0;
    }

    override public void OnDeactivate()
    {
        //  Time.timeScale = 1.0f;
        if (ActionDeath != null)
            ActionDeath.SetSuccess();

        ActionDeath = null;

        Action.SetSuccess();
        Action = null;

        Owner.BlackBoard.MotionType = E_MotionType.None;

        base.OnDeactivate();
    }


    override public bool HandleNewAction(AgentAction action)
    {
		 if (action as AgentActionKnockdown != null)
		 {
             Debug.LogError("obsolete AgentActionBlock arrived");
             action.SetFailed();
			 return true;
		 }
         if (action as AgentActionDeath != null)
         {
             ActionDeath = action as AgentActionDeath;
             //if (Owner.debugAnims == true) Debug.Log(Time.timeSinceLevelLoad + " " + this.ToString() + " Handle new action " + action.ToString());


             InitializeDeath();

             return true;
         }

		 return false;
    }

    override public void Release()
    {
        SetFinished(true);
    }

    override public void Update()
    {
		 //if (m_Human.PlayerProperty != null)
			
        //Debug.Log(Time.timeSinceLevelLoad + " " + this.ToString() + " - update " + State.ToString() + " " + EndOfStateTime);

        if (State == E_State.Death)
            return;

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

        if (PositionOK == false)
        {
            CurrentMoveTime += Time.deltaTime;
            if (CurrentMoveTime >= MoveTime)
            {
                CurrentMoveTime = MoveTime;
                PositionOK = true;
            }

            float progress = CurrentMoveTime / MoveTime;
            Vector3 finalPos = Mathfx.Sinerp(StartPosition, FinalPosition, progress);
            //MoveTo(finalPos);
            if (Move(finalPos - Transform.position) == false)
                PositionOK = true;
        }

        switch (State)
        {
            case E_State.Start:
                if (EndOfStateTime <= Time.timeSinceLevelLoad)
                    InitializeKnockdownLoop();
                break;
            case E_State.Loop:
                if (EndOfStateTime <= Time.timeSinceLevelLoad)
                    InitializeKnockdownUp();
                break;
            case E_State.Fatality:
                if (EndOfStateTime <= Time.timeSinceLevelLoad)
                {
                    if (ActionDeath != null)
                    {
                        ActionDeath.SetSuccess();
                        ActionDeath = null;
                    }
                    InitializeDeath();
                }
                break;
            case E_State.End:
                if (EndOfStateTime <= Time.timeSinceLevelLoad)
                    Release();
                break;
            case E_State.Death:
                break;
        }
    }

    protected override void Initialize(AgentAction action)
    {
        base.Initialize(action);

        Action = action as AgentActionKnockdown;

        string animName = Owner.AnimSet.GetKnockdowAnim(E_KnockdownState.Down, Owner.BlackBoard.WeaponSelected);

        StartRotation = Transform.rotation;
        StartPosition = Transform.position;

        Vector3 dir = Action.Attacker.Position - Transform.position;
        float angle = 0;

         if (dir.sqrMagnitude > 0.1f * 0.1f)
         {
               dir.Normalize();
                angle = Vector3.Angle(Transform.forward, dir);
         }
         else
             dir = Transform.forward;
        
        
        FinalRotation.SetLookRotation(dir);
        RotationTime = angle / 500.0f;
        FinalPosition = StartPosition + Action.Impuls;
        MoveTime = AnimEngine[animName].length * 0.4f;

        RotationOk = RotationTime == 0;
        PositionOK = MoveTime == 0;

        CurrentRotationTime = 0;
        CurrentMoveTime = 0;

        CrossFade(animName, 0.05f);

        EndOfStateTime = Time.timeSinceLevelLoad + AnimEngine[animName].length * 0.9f;
        KnockdownEndTime = EndOfStateTime + Action.Time;

        State = E_State.Start;
    }

    private void InitializeKnockdownLoop()
    {
        string animName = Owner.AnimSet.GetKnockdowAnim(E_KnockdownState.Loop, Owner.BlackBoard.WeaponSelected);
        CrossFade(animName, 0.05f); ;

        EndOfStateTime = KnockdownEndTime;
        State = E_State.Loop;

        Owner.DisableCollisions(); // mozna to bude blbnout jak se pak zapnou kolize zpet.. uvidime
    }

   
    private void InitializeDeath()
    {
        string animName = Owner.AnimSet.GetKnockdowAnim(E_KnockdownState.Fatality, Owner.BlackBoard.WeaponSelected);
        CrossFade(animName, 0.1f);

        EndOfStateTime = Time.timeSinceLevelLoad + AnimEngine[animName].length * 0.9f;
        ActionDeath.SetSuccess();
        State = E_State.Death;
        Owner.BlackBoard.MotionType = E_MotionType.Death;
    }

    private void InitializeKnockdownUp()
    {
        string animName = Owner.AnimSet.GetKnockdowAnim( E_KnockdownState.Up, Owner.BlackBoard.WeaponSelected);
        CrossFade(animName, 0.05f);

        EndOfStateTime = Time.timeSinceLevelLoad + AnimEngine[animName].length * 0.9f;

        State = E_State.End;
        Owner.EnableCollisions();
    }
            
    void UpdateFinalRotation()
    {
        Vector3 dir = Action.Attacker.Position - Owner.Position;
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
