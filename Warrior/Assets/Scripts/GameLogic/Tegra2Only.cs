using UnityEngine;
using System.Collections;

public class Tegra2Only : MonoBehaviour {

    public GameObject Message;

	// Use this for initialization
	void Start () 
    {
        if (SystemInfo.graphicsDeviceVendor.ToLower().IndexOf("nvidia") >= 0)
        {
            Application.LoadLevel("MainMenu");
            return;
        }
        Message.SetActive(true);
        Invoke("Quit", 2);
	}
	
	// Update is called once per frame
	void Quit () 
    {
         Application.Quit();
	}
}
