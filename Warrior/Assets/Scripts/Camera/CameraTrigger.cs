using UnityEngine;
using System.Collections;

public class CameraTrigger : MonoBehaviour 
{
		public Transform CameraOffset;
		public float Times = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

		void OnDrawGizmos()
		{
				if (null == CameraOffset)
						return;

				Gizmos.DrawSphere (transform.position, 0.4f);
				Gizmos.DrawSphere (CameraOffset.position, 0.3f);
				Gizmos.DrawLine (transform.position, CameraOffset.position);
				BoxCollider collider = GetComponent<BoxCollider> ();
				Gizmos.DrawWireCube (collider.center + transform.position, collider.size);
		}
}
