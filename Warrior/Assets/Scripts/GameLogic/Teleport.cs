using UnityEngine;
using System.Collections;


[RequireComponent(typeof(BoxCollider))]
public class Teleport : MonoBehaviour
{
	public Transform Destination;
	public float TeleportDelay = 0;
    public float FadeOUtTime = 1;
    public float FadeInTime = 1;
    public AudioClip Sound = null;


	// Use this for initialization
	void Start()
	{

	}

    // We'll draw a gizmo in the scene view, so it can be found....
    void OnDrawGizmos()
    {
        BoxCollider b = GetComponent("BoxCollider") as BoxCollider;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(b.transform.position + b.center, b.size);

        if (Destination != null)
        {
            Gizmos.DrawLine(b.transform.position, Destination.position);
            Gizmos.DrawWireCube(Destination.position, Vector3.one * 0.5f);
        }
    }

	void OnTriggerEnter(Collider other)
	{
        ComponentPlayer player = other.GetComponent<ComponentPlayer>();
        //if(player)
        //    player.Teleport(this);
	}
}
