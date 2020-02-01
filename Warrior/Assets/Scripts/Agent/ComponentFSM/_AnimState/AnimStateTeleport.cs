using UnityEngine;
using System.Collections;

public class AnimStateTeleport : AnimState
{
    AgentActionTeleport Action;

    public AnimStateTeleport(Animation anims, Agent owner)
        : base(anims, owner)
    {

    }

    override public void Release()
    {
        //if (m_Human.PlayerProperty != null)
        //Debug.Log(Time.timeSinceLevelLoad + " " + this.ToString() + " - release");

        SetFinished(true);
    }

    override public void OnActivate(AgentAction action)
    {
        base.OnActivate(action);

    }

    override public void OnDeactivate()
    {
        if (Action != null)
            Action.SetSuccess();

        Action = null;

        base.OnDeactivate();
    }


    override public bool HandleNewAction(AgentAction action)
    {
        return false;
    }

    override public void Update()   
    {
    }

    protected override void Initialize(AgentAction action)
    {
        base.Initialize(action);

        Action = action as AgentActionTeleport;

        Owner.BlackBoard.MotionType = E_MotionType.None;
        Owner.BlackBoard.MoveDir = Vector3.zero;
        Owner.BlackBoard.Speed = 0;

        string s = Owner.AnimSet.GetIdleAnim(Owner.BlackBoard.WeaponSelected, Owner.BlackBoard.WeaponState);
        CrossFade(s, 0.2f);

        Owner.StartCoroutine(Teleport());
    }

    IEnumerator Teleport()
    {
        yield return new WaitForSeconds(Action.Teleport.TeleportDelay);

        Vector3 offset = Vector3.zero;
        if (Owner.IsPlayer)
             offset = Owner.Transform.InverseTransformPoint(Camera.main.transform.position); 

        if (Action.Teleport.Sound)
			Owner.Sound.PlaySound(Action.Teleport.Sound);

//        if(Action.FadeGui)
//            GuiManager.Instance.FadeOut(Action.Teleport.FadeOUtTime);
        yield return new WaitForSeconds(Action.Teleport.FadeOUtTime + 0.1f);

        //Owner.Teleport(Action.Teleport.Destination);

        if (Owner.IsPlayer)
        {
            CameraBehaviour.Instance.Activate(Owner.Transform.position + offset, Owner.Transform.position);
        }

//        if (Action.FadeGui)
//            GuiManager.Instance.FadeIn(Action.Teleport.FadeInTime);
//
        Release();
    }
}