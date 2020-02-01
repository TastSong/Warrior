using System;

public class AgentActionPlayAnim : AgentAction
{
    public string AnimName;


    public AgentActionPlayAnim() : base(AgentActionFactory.E_Type.E_PLAY_ANIM) { }
}

