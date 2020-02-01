using System;

public class AgentActionUseLever : AgentAction
{
    public InteractionGameObject InterObj;
    public E_InteractionType Interaction;

    public AgentActionUseLever() : base(AgentActionFactory.E_Type.E_USE_LEVER) { }
}

