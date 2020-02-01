using UnityEngine;
using System.Collections;

public class AutoElementManager : MonoBehaviour
{
	private static AutoElementManager _instance;

	public static AutoElementManager Instance {
		get {
			if (_instance == null) {
				_instance = (new GameObject ("AutoElementManager")).AddComponent<AutoElementManager> ();
			}
			return _instance;
		}
	}

}
