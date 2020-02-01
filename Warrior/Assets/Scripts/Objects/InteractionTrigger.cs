using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class InteractionTrigger : InteractionGameObject
{
    Animation Animation;
    Agent Owner;

    public GameObject InteractionObject;
    public Transform RootTransform;


    public AnimationClip Anim;
    
    public bool DisableAfterUse = true;

         
	// Use this for initialization
    void Awake()
    {
        InteractionType = E_InteractionObjects.Trigger;
    }

	void Start () 
    {
        Animation = InteractionObject.GetComponent<Animation>();
        Animation.wrapMode =  WrapMode.Once;
	}

    public override Transform GetEntryTransform()
    {
        return RootTransform;
    }

    public override float DoInteraction(E_InteractionType type)
    {
      //  Debug.Log(gameObject.name + " DoInteraction");

        OnInteractionStart();

        Animation.Play(Anim.name);        

        Invoke("AnimationDone", Animation[Anim.name].length);

        return Animation[Anim.name].length;
    }

    void AnimationDone()
    {
        //Debug.Log(gameObject.name + " AnimationDone");

        OnInteractionEnd();

        if (DisableAfterUse)
            gameObject.SetActive(false);
    }


    public override void Restart()
    {
       // Debug.Log(gameObject.name + " restart");
        gameObject.SetActive(true);
        Animation.Stop();
        Anim.SampleAnimation(InteractionObject, 0);

        base.Restart();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, (GetComponent<Collider>() as SphereCollider).radius);
    }
}
