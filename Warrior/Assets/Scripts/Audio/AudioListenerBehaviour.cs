using UnityEngine;
using System.Collections;

public class AudioListenerBehaviour : MonoBehaviour
{
    Transform CameraTransform;
    Transform Transform;

	// Use this for initialization
	void Start () {
        CameraTransform = Camera.main.transform;
        Transform = transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Transform.rotation =  CameraTransform.rotation;
        Transform.rotation.SetLookRotation(CameraTransform.forward);
	}
}
