using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ObjectDamage : MonoBehaviour {

    public float Damage = 10;
    public float NextDamageDelay = 1;
    public float Impuls;


    public Vector3 Size;

    private Bounds Bounds = new Bounds();

    public AudioClip[] SoundHit = null;

    public float SoundRange = 2;
	private Transform Transform;
    private AudioSource Audio;

    private float TestTime = 0;

	void Awake()
	{
		Transform = transform;
        Audio = GetComponent<AudioSource>();
        Bounds.size = Size;
	}

    void Start()
    {
        Bounds.size = Size;
    }

	// Update is called once per frame
	void Update () 
    {
        //Vector3 playerPos = Player.Instance.Agent.Position;
        
     //   float distance = (playerPos - Transform.position).sqrMagnitude;
        
       // Debug.Log(playerPos.ToString() + " " + myPos + " " + Transform.position + " " +  distance);
        if (TestTime < Time.timeSinceLevelLoad)
        {
            //if (PointInsideObject(playerPos))
            {
                //Debug.Log("play hit ");
                TestTime = Time.timeSinceLevelLoad + NextDamageDelay;

                if (SoundHit != null && SoundHit.Length > 0)
                    Audio.PlayOneShot(SoundHit[Random.Range(0, SoundHit.Length)]);
                //Player.Instance.Agent.ReceiveEnviromentDamage(Damage, Player.Instance.Agent.Forward * -2);
            }
        }
	}

    // We'll draw a gizmo in the scene view, so it can be found....
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Bounds.size = Size;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Bounds.size);
    //    Gizmos.matrix = Matrix4x4.identity;

    }

    public bool PointInsideObject(Vector3 point)
    {
        // Transform the point to the box space 
        Vector3 localPoint =  Transform.InverseTransformPoint(point);

        return Bounds.Contains(localPoint);
    } 


}
