using UnityEngine;
using System.Collections;


public class SpawnPoint : MonoBehaviour
{

	[System.NonSerialized]
	public Transform Transform;
	public AnimationClip SpawnAnimation;

	void Awake ()
	{
		Transform = transform;
	}

	// Use this for initialization
	void Start ()
	{
		enabled = false;
	}
}
