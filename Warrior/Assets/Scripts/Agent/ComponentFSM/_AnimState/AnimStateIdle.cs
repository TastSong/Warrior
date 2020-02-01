using UnityEngine;

public class AnimStateIdle : AnimState
{
    float TimeToFinishWeaponAction;
    AgentAction WeaponAction;

    public AnimStateIdle(Animation anims, Agent owner)
        : base(anims, owner)
    {

    }

    override public void Release()
    {
        SetFinished(true);
    }

    override public bool HandleNewAction(AgentAction action)
    {
        if (action is AgentActionWeaponShow)
        {
            if ((action as AgentActionWeaponShow).Show == true)
            {
                string s = Owner.AnimSet.GetShowWeaponAnim(Owner.BlackBoard.WeaponSelected);
                TimeToFinishWeaponAction = Time.timeSinceLevelLoad + AnimEngine[s].length * 0.8f;
                AnimEngine.CrossFade(s, 0.1f);
            }
            else
            {
                string s = Owner.AnimSet.GetHideWeaponAnim(Owner.BlackBoard.WeaponSelected);
                TimeToFinishWeaponAction = Time.timeSinceLevelLoad +( AnimEngine[s].length * 0.9f);
                AnimEngine.CrossFade(s, 0.1f);
            }
            WeaponAction = action;
            return true;
        }
        return false;
    }

    override public void Update()
    {
        if (WeaponAction != null && TimeToFinishWeaponAction < Time.timeSinceLevelLoad)
        {
            WeaponAction.SetSuccess();
            WeaponAction = null;
            CrossFade(Owner.AnimSet.GetIdleAnim(Owner.BlackBoard.WeaponSelected, Owner.BlackBoard.WeaponState), 0.2f);
        }
    }

    void PlayIdleAnim()
    {
        string s = Owner.AnimSet.GetIdleAnim(Owner.BlackBoard.WeaponSelected, Owner.BlackBoard.WeaponState);
        CrossFade(s, 0.2f);
    }

    protected override void Initialize(AgentAction action)
    {
        base.Initialize(action);

        Owner.BlackBoard.MotionType = E_MotionType.None;
        Owner.BlackBoard.MoveDir = Vector3.zero;
        Owner.BlackBoard.Speed = 0;

        if (WeaponAction == null)
            PlayIdleAnim();
    }
}