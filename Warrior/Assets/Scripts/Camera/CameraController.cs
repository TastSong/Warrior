using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	
	public GameObject Target;

	private CameraOffset Offset;
	private Transform TargetTransform;

	// Use this for initialization
	void Start ()
	{
		Offset = Target.GetComponent<CameraOffset> ();
		TargetTransform = Offset.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void LateUpdate ()
	{
		Vector3 pos = Offset.CameraPosition;
		transform.position = Vector3.Lerp (transform.position, pos, Time.deltaTime * 4);

		Vector3 dir = transform.forward;
		dir.y = 0;
		dir.Normalize ();
		Vector3 t = TargetTransform.position;
		t += dir * 0.7f;

		Vector3 lookAt = t - transform.position;
		lookAt.Normalize ();

		transform.forward = Vector3.Lerp (transform.forward, lookAt, Time.deltaTime * 4);
	}
}
