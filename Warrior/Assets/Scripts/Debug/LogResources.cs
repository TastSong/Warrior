using UnityEngine;
using System.Collections;

public class LogResources : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		Invoke("Log", 1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	

	void Log()
	{	
	     Object[] resources = Resources.FindObjectsOfTypeAll(typeof(Texture));
	     
	     Debug.Log("number of textures " + resources.Length);
	     for (int i = 0; i < resources.Length; i++)
        {
            Debug.Log(resources[i].name + " " + (resources[i] as Texture).width + "x" + (resources[i] as Texture).height);
        }
        
         Object[] objects = Resources.FindObjectsOfTypeAll(typeof(Object));
	     
	     Debug.Log("number of objects " + resources.Length);
	     for (int i = 0; i < resources.Length; i++)
        {
            Debug.Log(objects[i].name);
        }
	}

}
