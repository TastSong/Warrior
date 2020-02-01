using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {

	public float RotationSpeedX;
	public float RotationSpeedY;
	public float RotationSpeedZ;

	private Transform MyTransform;
	private Renderer Mesh;

	void Awake()
	{
		MyTransform = transform;
		Mesh = GetComponent<Renderer>();

	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Mesh != null && Mesh.isVisible == false)
			return;
		MyTransform.Rotate(RotationSpeedX * Time.deltaTime, RotationSpeedY * Time.deltaTime, RotationSpeedZ * Time.deltaTime);
	}
}
