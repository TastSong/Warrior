using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraOffset : MonoBehaviour
{
	public Vector3 DefaultOffset = new Vector3 (0, 10, -5);
	GameObject Offset;
	Transform OffsetTransform;

	public Vector3 CameraPosition {
		get {
			return OffsetTransform.position * 0.9f + transform.position;
		}
	}

	void Awake ()
	{
		Offset = new GameObject ("CameraOffset");
		OffsetTransform = Offset.transform;
		OffsetTransform.position = OffsetTransform.TransformPoint (DefaultOffset);
	}


	void OnTriggerEnter (Collider other)
	{
		CameraTrigger cameraTrigger = other.GetComponent<CameraTrigger> ();

		if (null == cameraTrigger)
			return;
				
		if (cameraTrigger.Times == 0) {
			OffsetTransform.position = cameraTrigger.CameraOffset.localPosition;
			return;
		}

		List<Vector3> pos = new List<Vector3> ();
		if (null == cameraTrigger.CameraOffset)
			pos.Add (DefaultOffset);
		else
			pos.Add (cameraTrigger.CameraOffset.localPosition);
				
		iTween.moveToBezier (Offset, cameraTrigger.Times, 0, pos);

		/*
				if (null != cameraTrigger) {
						
						if (null == cameraTrigger.CameraOffset) {
								pos.Add (DefaultOffset);
								iTween.moveToBezier (Offset, cameraTrigger.Times, 0, pos);
						} else {
								
								if (cameraTrigger.Times == 0) {
										OffsetTransform.position = cameraTrigger.CameraOffset.localPosition;
										//Offset.transform = cameraTrigger.CameraOffset.localPosition;
								} else {
										pos.Add (cameraTrigger.CameraOffset.localPosition);
										iTween.moveToBezier (Offset, cameraTrigger.Times, 0, pos);
								}
						}
				}
				*/
	}
}
