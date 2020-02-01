using UnityEngine;
using System.Collections;

public class InteractionBase : MonoBehaviour {

    /// <summary>
    /// Interaction particle.
    /// 破坏粒子特效
    /// </summary>
    [System.Serializable]
    public class InteractionParticle
    {
        //特效对象
        public ParticleEmitter Emitter;
        //延时
        public float Delay;
        //播放时间
        public float Life;
        //跟随GameObject
        public bool LinkOnRoot;

    }

    /// <summary>
    /// Interaction sound.
    /// 破碎声音
    /// </summary>
    [System.Serializable]
    public class InteractionSound
    {
        public AudioSource Audio;
        //延时
        public float Delay;
        //播放时间
        public float Life;
        public Transform Parent;
    }

    public InteractionParticle[] Emitters;
    public InteractionSound[] Sounds;

    /// <summary>
    /// 特效播放
    /// </summary>
    /// <param name="emitter">特效对象</param>
    /// <param name="start">开始时间</param>
    /// <param name="life">播放时长</param>
    /// <returns></returns>
    protected IEnumerator PlayParticle(ParticleEmitter emitter, float start, float life)
    {
        yield return new WaitForSeconds(start);
        emitter.emit = true;
        yield return new WaitForSeconds(life);
        emitter.emit = false;
    }

    /// <summary>
    /// 播放声音
    /// </summary>
    /// <param name="audio">声音片段</param>
    /// <param name="start">开始时间</param>
    /// <param name="life">播放时长</param>
    /// <returns></returns>
    protected IEnumerator PlaySound(AudioSource audio, float start, float life)
    {
        yield return new WaitForSeconds(start);
        audio.Play();
        yield return new WaitForSeconds(life);
        audio.Stop();
    }
}
