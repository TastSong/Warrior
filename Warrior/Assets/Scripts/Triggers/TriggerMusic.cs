using UnityEngine;
using System.Collections;


[RequireComponent(typeof(BoxCollider))]
public class TriggerMusic: MonoBehaviour 
{
    public AudioClip Music;

    public float FadeOutTime = 0.4f;
    public float FadeInTime = 0.4f;
    public float Volume = 1;


    // We'll draw a gizmo in the scene view, so it can be found....
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube((GetComponent<Collider>() as BoxCollider).center + transform.position, (GetComponent<Collider>() as BoxCollider).size);
    }

    public void Disable()
    {
		gameObject.SetActive(false);
    }


    void OnTriggerEnter(Collider other)
    {
//        if (other != Player.Instance.Agent.CharacterController)
//            return;
//
//        if (SoundDataManager.Instance.IsSwitchingMusic())
//            Mission.Instance.SetNewMusic(Music, Volume, FadeOutTime, FadeInTime);
//        
    }
}
