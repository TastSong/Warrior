using UnityEngine;

public class AnimStateBlocking : AnimState
{
    enum E_State
    {
        E_START,
        E_BLOCK,
        E_BLOCK_HIT,
        E_END,
    }

    AgentActionBlock ActionBlock;
    AgentActionDamageBlocked ActionDamageBlocked;
    AnimAttackData AnimAttackData;

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



    E_State State;

    public AnimStateBlocking(Animation anims, Agent owner)
		: base(anims, owner)
	{

	}

    override public void OnActivate(AgentAction action)
    {
      //  Time.timeScale = 0.1f;
        base.OnActivate(action);

        Owner.BlackBoard.MotionType = E_MotionType.Block;
        Owner.BlackBoard.MoveDir = Vector3.zero;
        Owner.BlackBoard.Speed = 0;
    }

    override public void OnDeactivate()
    {
      //  Time.timeScale = 1.0f;
        if (ActionDamageBlocked != null)
            ActionDamageBlocked.SetSuccess();

        ActionDamageBlocked = null;
        
		 ActionBlock.SetSuccess();
         ActionBlock = null;

         Owner.BlackBoard.MotionType = E_MotionType.None;

         base.OnDeactivate();
    }


    override public bool HandleNewAction(AgentAction action)
    {
		 if (action as AgentActionBlock != null)
		 {
             Debug.LogError("obsolete AgentActionBlock arrived");
             action.SetFailed();
			 return true;
		 }
         if (action as AgentActionDamageBlocked != null)
         {
             ActionDamageBlocked = action as AgentActionDamageBlocked;
             //if (Owner.debugAnims == true) Debug.Log(Time.timeSinceLevelLoad + " Handle new action " + action.ToString());

             if (ActionDamageBlocked.BreakBlock == true)
                 InitializeBlockFailed();
             else
                 InitializeBlockSuccess();

             return true;
         }

		 return false;
    }

    override public void Update()
    {
		 //if (m_Human.PlayerProperty != null)
			
        //Debug.Log(Time.timeSinceLevelLoad + " " + this.ToString() + " - update " + State.ToString() + " " + EndOfStateTime);
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
            case E_State.E_START:
                if (EndOfStateTime <= Time.timeSinceLevelLoad)
                    InitializeBlockLoop();
                break;
            case E_State.E_BLOCK:
                if (EndOfStateTime <= Time.timeSinceLevelLoad)
                    InitializeBlockEnd();
                break;
            case E_State.E_BLOCK_HIT:
                if (EndOfStateTime <= Time.timeSinceLevelLoad)
                {
                    if(Time.timeSinceLevelLoad < BlockEndTime)
                        InitializeBlockLoop();
                    else
                        InitializeBlockEnd();

                    if (ActionDamageBlocked != null)
                    {
                        ActionDamageBlocked.SetSuccess();
                        ActionDamageBlocked = null;
                    }
                }
                break;
            case E_State.E_END:
                if (EndOfStateTime <= Time.timeSinceLevelLoad)
                    Release();
                break;
        }
    }

    protected override void Initialize(AgentAction action)
    {
        base.Initialize(action);


        //Debug.Log(Time.timeSinceLevelLoad + Owner.name + "Block init");
        ActionBlock = action as AgentActionBlock;

        string animName = Owner.AnimSet.GetBlockAnim(E_BlockState.Start, Owner.BlackBoard.WeaponSelected);

        StartRotation = Transform.rotation;
        StartPosition = Transform.position;

        Vector3 dir = ActionBlock.Attacker.Position - Transform.position;
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
        MoveTime = 0;

        RotationOk = RotationTime == 0;
        PositionOK = MoveTime == 0;

        CurrentRotationTime = 0;
        CurrentMoveTime = 0;

        CrossFade(animName, 0.05f);

        EndOfStateTime = Time.timeSinceLevelLoad + AnimEngine[animName].length * 0.9f;
        BlockEndTime = Time.timeSinceLevelLoad + ActionBlock.Time;

        State = E_State.E_START;
    }

    private void InitializeBlockLoop()
    {
        string animName = Owner.AnimSet.GetBlockAnim(E_BlockState.Loop, Owner.BlackBoard.WeaponSelected);
        CrossFade(animName, 0.05f); ;

        EndOfStateTime = BlockEndTime;

        State = E_State.E_BLOCK;
        Owner.BlackBoard.MotionType = E_MotionType.Block;
    }

    private void InitializeBlockSuccess()
    {
        //Debug.Log(Time.timeSinceLevelLoad + Owner.name + "Block success");

        string animName = Owner.AnimSet.GetBlockAnim(E_BlockState.HitBlocked, Owner.BlackBoard.WeaponSelected);
        CrossFade(animName, 0.05f); ;

        StartRotation = Transform.rotation;
        StartPosition = Transform.position;

        Vector3 dir = ActionBlock.Attacker.Position - Transform.position;
        float angle = 0;

        if (dir.sqrMagnitude > 0.1f * 0.1f)
        {
            dir.Normalize();
            angle = Vector3.Angle(Transform.forward, dir);
        }
        else
            dir = Transform.forward;

        FinalRotation.SetLookRotation(dir);
        FinalPosition = StartPosition - dir * 0.75f;

        RotationTime = angle / 500.0f;
        MoveTime = 0.1f;

        RotationOk = RotationTime == 0;
        PositionOK = MoveTime == 0;

        CurrentRotationTime = 0;
        CurrentMoveTime = 0;

        CrossFade(animName, 0.05f);

        EndOfStateTime = Time.timeSinceLevelLoad + AnimEngine[animName].length * 0.9f;

        //FIX_ME
        //kazdy zasach prodlouzit block od 2 sekundu.  lepsi by bylo kdyby to delal agent nebo GOAP ale neni cas..   
       // BlockEndTime += 2;

        State = E_State.E_BLOCK_HIT;
        Owner.BlackBoard.MotionType = E_MotionType.BlockingAttack;
    }


    private void InitializeBlockFailed()
    {
      //  Debug.Log(Time.timeSinceLevelLoad + Owner.name + " Block failed");

        string animName = Owner.AnimSet.GetBlockAnim(E_BlockState.Failed, Owner.BlackBoard.WeaponSelected);
        CrossFade(animName, 0.05f); ;

        StartRotation = Transform.rotation;
        StartPosition = Transform.position;

        Vector3 dir = ActionBlock.Attacker.Position - Transform.position;
        float angle = 0;

        if (dir.sqrMagnitude > 0.1f * 0.1f)
        {
            dir.Normalize();
            angle = Vector3.Angle(Transform.forward, dir);
        }
        else
            dir = Transform.forward;

        FinalRotation.SetLookRotation(dir);
        FinalPosition = StartPosition - dir * 2;

        RotationTime = angle / 500.0f;
        MoveTime = AnimEngine[animName].length * 0.8f;

        RotationOk = RotationTime == 0;
        PositionOK = MoveTime == 0;

        CurrentRotationTime = 0;
        CurrentMoveTime = 0;

        CrossFade(animName, 0.05f);

        EndOfStateTime = Time.timeSinceLevelLoad + AnimEngine[animName].length * 0.9f;

        State = E_State.E_END;
        Owner.BlackBoard.MotionType = E_MotionType.Injury;

    }
    private void InitializeBlockEnd()
    {
        //Debug.Log(Time.timeSinceLevelLoad + Owner.name + "Block end");

        string animName = Owner.AnimSet.GetBlockAnim(E_BlockState.End, Owner.BlackBoard.WeaponSelected);
        CrossFade(animName, 0.05f); ;

        EndOfStateTime = Time.timeSinceLevelLoad + AnimEngine[animName].length * 0.9f;
        Owner.BlackBoard.MotionType = E_MotionType.None;

        State = E_State.E_END;

    }
            
    void UpdateFinalRotation()
    {
        if (ActionBlock.Attacker == null)
            return;

        Vector3 dir = ActionBlock.Attacker.Position - Owner.Position;
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
