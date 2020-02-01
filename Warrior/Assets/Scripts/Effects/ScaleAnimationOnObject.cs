using UnityEngine;
using System.Collections;

public class ScaleAnimationOnObject : MonoBehaviour {

    public enum Scaletype
    {
        Coserp,
        Sinerp,
    }

    public Vector3 ScalePeek = new Vector3(2, 2, 2);
    public Scaletype Type = Scaletype.Sinerp;
    public float Speed = 1;

	private Transform MyTransform;
	private Renderer Mesh;
    private Vector3 Center;
    private float ScaleTime;

	void Awake()
	{
		MyTransform = transform;
        Center = MyTransform.localScale;

	}
	void Start () 
    {
        Mesh = GetComponent<Renderer>();
        ScaleTime = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (Mesh != null && Mesh.isVisible == false)
			return;

        ScaleTime += Speed * Time.deltaTime;


        switch (Type)
        {
            case Scaletype.Coserp:

                MyTransform.localScale = Center + ScalePeek * Mathf.Cos(ScaleTime * Mathf.PI );
                break;
            case Scaletype.Sinerp:
                MyTransform.localScale = Center + ScalePeek * Mathf.Sin(ScaleTime * Mathf.PI);
                break;
        }
	}
}
