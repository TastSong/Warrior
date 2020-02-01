using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class InteractionTriggerAnim : InteractionGameObject
{
    Animation Animation;
    Agent Owner;

    public GameObject InteractionObject;

    public AnimationClip Anim;
    
    public bool DisableAfterUse = true;

    bool Useable;

         
	// Use this for initialization
    void Awake()
    {
        InteractionType = E_InteractionObjects.TriggerAnim;
    }

	void Start () 
    {
        Animation = InteractionObject.GetComponent<Animation>();
        Animation.wrapMode =  WrapMode.Once;
        Useable = true;

	}

    public override Transform GetEntryTransform()
    {
        Debug.LogError(gameObject.name + " " + name + " error in call GetEntryTransform");
        return transform;
    }

    
    public override float DoInteraction(E_InteractionType type)
    {
        if (DisableAfterUse)
            Useable = false;

        OnInteractionStart();

        Animation.Play(Anim.name);        

        Invoke("AnimationDone", Animation[Anim.name].length);

        return Animation[Anim.name].length;
    }

    void AnimationDone()
    {
        OnInteractionEnd();

        if (DisableAfterUse)
            gameObject.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (GetComponent<Collider>() is SphereCollider)
            Gizmos.DrawWireSphere(transform.position, (GetComponent<Collider>() as SphereCollider).radius);
        else if (GetComponent<Collider>() is BoxCollider)
            Gizmos.DrawWireCube(transform.position + (GetComponent<Collider>() as BoxCollider).center, (GetComponent<Collider>() as BoxCollider).size);
    }

    void OnTriggerEnter(Collider other)
    {
        //if (Useable == false || other != Player.Instance.Agent.CharacterController)
        //    return;

        DoInteraction(E_InteractionType.On);
    }

    public override void Restart()
    {
        Useable = true;
        gameObject.SetActive(true);

        Animation.Stop();
        Anim.SampleAnimation(InteractionObject, 0);
        //Animation[Anim.name].time = 0; 
        //Animation.Rewind();
        
        base.Restart();
    }


}
