using UnityEngine;
using System.Collections;

/// <summary>
/// Breakable object.
/// 可以破坏的物品，加钱加经验加物品
/// </summary>
public abstract class BreakableObject : InteractionBase
{
		//破碎物体
    public GameObject BreakableGameObject;
		//破碎Animation
    public AnimationClip AnimBreak;

    //是否是可以攻击的
    protected bool Active = true;
    protected Transform Root;
    private Animation Animation;
    private GameObject GameObject;


    public bool IsActive { get { return Active; } }
    public Vector3 Position { get { return Root.position; } }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {
        GameObject = gameObject;
        Root = transform;
        Animation = BreakableGameObject.GetComponent<Animation>();
        Animation.wrapMode = WrapMode.Once;
    }


    /// <summary>
    /// 破碎时调用
    /// </summary>
    public virtual void Break()
    {
        Active = false;
        Animation.Play(AnimBreak.name);
        //动画时间长度
        float end = Animation[AnimBreak.name].length;
        
        for (int i = 0; Emitters != null && i < Emitters.Length; i++)
        {
            InteractionParticle ip = Emitters[i];
            MissionZone.Instance.StartCoroutine(PlayParticle(ip.Emitter, ip.Delay, ip.Life));

            if (end < ip.Delay + ip.Life)
                end = ip.Delay + ip.Life;

            if (ip.LinkOnRoot)
                ip.Emitter.transform.parent = Root;
        }

        for (int i = 0; Sounds != null && i < Sounds.Length; i++)
        {
            InteractionSound sound = Sounds[i];
            MissionZone.Instance.StartCoroutine(PlaySound(sound.Audio, sound.Delay, sound.Life));

            if (end < sound.Delay + sound.Life)
                end = sound.Delay + sound.Life;

            if (sound.Parent)
                sound.Audio.transform.parent = sound.Parent;
        }

        Invoke("OnDone", end +0.1f);
        OnStart();
    }

    /// <summary>
    /// 完成破碎后做后续逻辑
    /// </summary>
    protected virtual void OnStart()
    {

    }

    protected virtual void OnDone()
    {
        GameObject.SetActive(false);
        BreakableGameObject.SetActive(false);
    }

    public virtual void Restart()
    {
        Animation.Stop();
        AnimBreak.SampleAnimation(BreakableGameObject, 0);

        Active = true;
    }

    public void Enable()
    {
        GameObject.SetActive(true);
        BreakableGameObject.SetActive(true);
    }

    public void Disable()
    {
        GameObject.SetActive(false);
        BreakableGameObject.SetActive(false) ;
    }


}

