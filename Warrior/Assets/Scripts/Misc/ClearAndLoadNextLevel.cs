using UnityEngine;
using System.Collections;

public class ClearAndLoadNextLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Debug.Log("start uclear and load");
       // Resources.UnloadUnusedAssets();

        Debug.Log("load next level");
 //       Game.Instance.LoadNextLevel(Game.Instance.NextLevelToLoad, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
    }
}
