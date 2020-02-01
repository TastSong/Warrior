using UnityEngine;
using System.Collections;

public abstract class InteractionGameObject : InteractionBase
{
    [System.NonSerialized]
    public E_InteractionObjects InteractionType = E_InteractionObjects.None;

    public GameObject[] ObjectsToShow;
    public GameObject[] ObjectsToHide;

    public ParticleEmitter ActiveEffect;
    public AnimationClip CameraAnimation;


    public abstract Transform GetEntryTransform();

    public  Vector3 Position { get {return transform.position;}}

    public abstract float DoInteraction(E_InteractionType interaction);

    protected bool InteractionObjectUsable = true;
    protected bool Enabled = true;

    public bool IsActive { get { return InteractionObjectUsable; } }
    public bool IsEnabled { get { return Enabled; } }

    public void Enable(bool enable)
    {
        Enabled = enable;
        gameObject.SetActive(enable);

        if (ActiveEffect)
            ActiveEffect.emit = enable;

    //    Debug.Log(name + enable);
    }

    protected void OnInteractionStart()
    {
        if (ActiveEffect)
            ActiveEffect.emit = false;

        if (CameraAnimation)
            CameraBehaviour.Instance.PlayCameraAnim(CameraAnimation.name, true, true);

        for (int i = 0; Emitters != null && i < Emitters.Length; i++)
        {
            InteractionParticle ip = Emitters[i];
            MissionZone.Instance.StartCoroutine(PlayParticle(ip.Emitter, ip.Delay, ip.Life));


            if (ip.LinkOnRoot)
                ip.Emitter.transform.parent = GetEntryTransform();
        }

        for (int i = 0; Sounds != null && i < Sounds.Length; i++)
        {
            InteractionSound sound = Sounds[i];
            MissionZone.Instance.StartCoroutine(PlaySound(sound.Audio, sound.Delay, sound.Life));

            if (sound.Parent)
                sound.Audio.transform.parent = sound.Parent;
        }

        for (int i = 0; ObjectsToShow != null && i < ObjectsToShow.Length; i++)
            ObjectsToShow[i].SetActive(true);
    }

    protected void OnInteractionEnd()
    {
        for (int i = 0;ObjectsToHide != null && i < ObjectsToHide.Length; i++)
            ObjectsToHide[i].SetActive(false);

    }

    public virtual void Restart()
    {
         for (int i = 0; ObjectsToShow != null && i < ObjectsToShow.Length; i++)
            ObjectsToShow[i].SetActive(false);

        for (int i = 0;ObjectsToHide != null && i < ObjectsToHide.Length; i++)
            ObjectsToHide[i].SetActive(true);

        InteractionObjectUsable = true;
    }

}

