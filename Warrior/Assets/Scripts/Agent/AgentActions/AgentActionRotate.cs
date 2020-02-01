using System;
using UnityEngine;

public class AgentActionRotate : AgentAction
{
    public Vector3 Direction = Vector3.zero;
    public Agent Target = null;
    public float RotationModifier = 1;

    public AgentActionRotate() : base(AgentActionFactory.E_Type.Rotate) { }

    public override void Reset() { Target = null; Direction = Vector3.zero; RotationModifier = 1; }
}

