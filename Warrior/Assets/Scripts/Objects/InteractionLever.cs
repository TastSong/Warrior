using UnityEngine;
using System.Collections;

/// <summary>
/// ½»»¥¿ØÖÆ¸Ë
/// </summary>
public class InteractionLever : InteractionGameObject
{
    public enum E_State
    {
        E_OFF,
        E_GOING_ON,
        E_ON,
        E_GOING_OFF,
        E_DISABLED,

    }

    Animation Animation;
    Agent Owner;

    public GameObject InteractionObject;
    public Transform RootTransform;


    public AnimationClip AnimON;
    public AnimationClip AnimOFF;

    public E_State State = E_State.E_OFF;

    public bool DisableAfterUse = true;

	// Use this for initialization
    void Awake()
    {
        InteractionType = E_InteractionObjects.UseLever;
        gameObject.layer = 9;
    }


	void Start () {

        Animation = InteractionObject.GetComponent<Animation>();
        Animation.wrapMode =  WrapMode.Once;
	}

    public override Transform GetEntryTransform()
    {
        return RootTransform;
    }

    public override float DoInteraction(E_InteractionType interaction)
    {
        OnInteractionStart();

        if (DisableAfterUse)
            InteractionObjectUsable = false;

        string s;
        if (interaction == E_InteractionType.On)
        {
            s = AnimON.name;
            State = E_State.E_GOING_ON;
        }
        else
        {
            s = AnimOFF.name;
            State = E_State.E_GOING_OFF;
        }

        Animation.Play(s);        

        Invoke("AnimationDone", Animation[s].length);

        return Animation[s].length;
    }

    void AnimationDone()
    {
        OnInteractionEnd();

        if (State == E_State.E_GOING_OFF)
            State = E_State.E_GOING_OFF;
        else
            State = E_State.E_GOING_ON;

        if (DisableAfterUse)
            gameObject.SetActive(false);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

    public override void Restart()
    {
        State = E_State.E_OFF;
        gameObject.SetActive(true);
        Animation.Stop();
        AnimON.SampleAnimation(InteractionObject, 0);

        base.Restart();
    }

}
