using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PadLock : MonoBehaviour
{
    public enum E_State
    {
        E_OFF,
        E_ON,
    }

    public GameObject Collision;

    Animation Animation;
    GameObject GameObject;
    AudioSource AudioSource;

    public AnimationClip AnimON;
    public AnimationClip AnimOFF;
    public AnimationClip AnimLoop;

    public AudioClip SoundOn;
    public AudioClip SoundOff;

    public E_State State = E_State.E_OFF;

    public bool HideWhenUnlock = true;

	// Use this for initialization
    void Awake()
    {
        Animation = GetComponent<Animation>();
        GameObject = gameObject;
        AudioSource = GetComponent<AudioSource>();
        
        Reset();
    }

    void OnEnable()
    {
   //     Debug.Log(GameObject.name + " enable " + State);

        if (State == E_State.E_OFF)
        {
            if (HideWhenUnlock)
                GameObject.SetActive(false);

            if (Collision != null)
                Collision.SetActive(false);
        }
    }


    public void Lock()
    {
        if (State == E_State.E_ON)
            return;

        State = E_State.E_ON;

        GameObject.SetActive(true);

        if (Collision != null)
            Collision.SetActive(true);

        if (AnimON != null)
            Animation.Play(AnimON.name);

        if (AnimLoop != null)
            Animation.PlayQueued(AnimLoop.name);

        if (SoundOn != null)
            AudioSource.PlayOneShot(SoundOn);

        

    //    Debug.Log(GameObject.name + " Lock " + State);
    }

    public void Unlock()
    {
        if (State == E_State.E_OFF)
            return;
        
        State = E_State.E_OFF;

      //  Debug.Log(GameObject.name + " UnLock " + State);

        StartCoroutine(Hide());
    }

    public void Reset()
    {
        State = E_State.E_OFF;

       // Debug.Log(GameObject.name + " reset " + State);
        Animation.Stop();

        if (HideWhenUnlock)
            GameObject.SetActive(false);

        if (Collision != null)
            Collision.SetActive(false);
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(0.3f);

        Animation.Stop();

        if(AnimOFF != null)
            Animation.Play(AnimOFF.name);

        float wait = 0;
        if (SoundOff != null)
        {
            AudioSource.PlayOneShot(SoundOff);
            wait = SoundOff.length;
        }

        if (AnimOFF != null)
        {
            if (wait < AnimOFF.length)
                wait = AnimOFF.length;
            else
                Invoke("CollisionOff", AnimOFF.length);
        }

        yield return new WaitForSeconds(wait);

        if (HideWhenUnlock)
            GameObject.SetActive(false);

        CollisionOff();
    }

    void CollisionOff()
    {
        if (Collision != null)
            Collision.SetActive(false);
    }

}
