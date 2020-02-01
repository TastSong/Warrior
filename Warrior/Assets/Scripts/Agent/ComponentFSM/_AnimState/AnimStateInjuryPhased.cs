using UnityEngine;

public class AnimStateInjuryPhased : AnimState
{
    AgentActionInjury Action = null;
    int CurrentPhase;
    const int MaxPhases = 3;

    float EndOfStateTime;


    public AnimStateInjuryPhased(Animation anims, Agent owner)
		: base(anims, owner)
	{
	}


	override public void OnActivate(AgentAction action)
	{
        base.OnActivate(action);

        Owner.BlackBoard.MotionType = E_MotionType.Injury;
        Owner.BlackBoard.MoveDir = Vector3.zero;
        Owner.BlackBoard.Speed = 0;
	}

	override public void OnDeactivate()
	{
      //  Time.timeScale = 1;

		Action.SetSuccess();
		Action = null;

        Owner.BlackBoard.MotionType = E_MotionType.None;

        base.OnDeactivate();
	}

    override public void Update()
    {
        if (EndOfStateTime <= Time.timeSinceLevelLoad)
            Release();
    }

	override public bool HandleNewAction(AgentAction action)
	{
        if (action is AgentActionInjury)
		{
            InitializeNext(action);

			return true;
		}
    	return false;
	}

    protected override void Initialize(AgentAction action)
    {
        base.Initialize(action);

        CurrentPhase = 0;

        Action = action as AgentActionInjury;

        // play owner anims
        string animName = Owner.AnimSet.GetInjuryPhaseAnim(CurrentPhase);

        CrossFade(animName, 0.1f);
        //end of state
        EndOfStateTime = AnimEngine[animName].length + Time.timeSinceLevelLoad;

        Owner.BlackBoard.MotionType = E_MotionType.None;
    }

    void InitializeNext(AgentAction action)
    {
        if(CurrentPhase + 1 == MaxPhases)
        {/// no more phases
            action.SetFailed();
            return;
        }

        if (Action != null)
            Action.SetSuccess();
        
        SetFinished(false); // just for sure

        CurrentPhase++;

        Action = action as AgentActionInjury;

        // play owner anims
        string animName = Owner.AnimSet.GetInjuryPhaseAnim(CurrentPhase);

        CrossFade(animName, 0.1f);
        //end of state
        EndOfStateTime = AnimEngine[animName].length + Time.timeSinceLevelLoad;

        Owner.BlackBoard.MotionType = E_MotionType.None;
    }
}
