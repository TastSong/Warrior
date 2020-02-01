using UnityEngine;
using System.Collections;
using System;

public class Test : MonoBehaviour 
{
		public Transform A, B, C;
		Quaternion q1 = Quaternion.identity;
		Vector3 dir = Vector3.zero;
	// Use this for initialization
	void Start () {
//				float a = 600.0f;
//				float b = 1.0f - 80.0f / 100.0f;
//				float c = a * b;
//				int d = (int)c;
//				c = 120.0f;
//				//d = (int)c;
//				d = Convert.ToInt32(d);
//				Debug.Log (a);        
//				Debug.Log (b);        
//				Debug.Log (c);        
//				Debug.Log (d); 
				dir = A.position - B.position;
	}
	

	// Update is called once per frame
	void Update () {
				//不可直接使用C.rotation.SetLookRotation(A.position,B.position);
				q1.SetLookRotation(A.position, B.position);
				//q1.SetLookRotation(dir);
				C.rotation = q1;
				//分别绘制A、B和C.right的朝向线
				//请在Scene视图中查看
				Debug.DrawLine(A.position, B.position, Color.black);
				Debug.DrawLine(Vector3.zero, A.position, Color.red);
				Debug.DrawLine(Vector3.zero, B.position, Color.green);
				Debug.DrawLine(C.position, C.TransformPoint(Vector3.right * 2.5f), Color.yellow);
				Debug.DrawLine(C.position, C.TransformPoint(Vector3.forward * 2.5f), Color.gray);
				//分别打印C.right与A、B的夹角
				Debug.Log("C.right与A的夹角:" + Vector3.Angle(C.right, A.position));
				Debug.Log("C.right与B的夹角:" + Vector3.Angle(C.right, B.position));
				//C.up与B的夹角
				Debug.Log("C.up与B的夹角:" + Vector3.Angle(C.up, B.position));
	}
}
