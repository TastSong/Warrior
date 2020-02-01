using UnityEngine;using System.Collections;


public class HitObject : BreakableObject{    public GameObject InteractionObject;

    private Agent Owner;	void Awake () 
    {
        base.Initialize();	}

    protected override void OnDone()
    {
        //do nothing
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}