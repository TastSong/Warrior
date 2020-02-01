using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CombatEffectsManager : MonoBehaviour
{
    public class CacheData
    {
        public GameObject GameObject;
        public ParticleEmitter[] Emitters;
        public Transform Transform;
    }

    [System.Serializable]
    public class CombatEffect
    {
        private Queue<CacheData> Cache = new Queue<CacheData>();
        private List<CacheData> InUse = new List<CacheData>();
        public GameObject Prefab;


        public void Init(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CacheData c = new CacheData();
                c.GameObject = GameObject.Instantiate(Prefab) as GameObject;

		c.GameObject.transform.parent = AutoElementManager.Instance.transform;

                c.Emitters = c.GameObject.GetComponentsInChildren<ParticleEmitter>();
                c.Transform = c.GameObject.transform;
                Cache.Enqueue(c);
                c.GameObject.SetActive(false);
            }
        }
        
        public void Update()
        {
            for (int i = InUse.Count - 1; i >= 0 ; i--)
            {
                CacheData c = InUse[i];
                bool emitting = false;
                for (int ii = 0; ii < c.Emitters.Length; ii++)
                {
                    if (c.Emitters[ii].emit == true || c.Emitters[ii].particleCount != 0)
                        emitting = true;

                    if(c.Emitters[ii].particleCount > 0)
                        c.Emitters[ii].emit = false;// turn off it 
                }
            
                if (emitting == false)
                {
                    //Debug.Log(Time.timeSinceLevelLoad + " remove " + c.GameObject.name);

					c.Transform.parent = AutoElementManager.Instance.transform;

                    //c.Transform.parent = null;
                    Cache.Enqueue(InUse[i]);
                    InUse.RemoveAt(i);
                    c.GameObject.SetActive(false);
                }
            }
        }

        public CacheData Get()
        {
            if (Cache.Count == 0)
                Init(2);

            return Cache.Dequeue();
        }

        public void Return (CacheData c)
        {
            InUse.Add(c);
        }

        public void Play(Vector3 pos, Vector3 dir)
        {
            if (Cache.Count == 0)
                Init(2);
            
            CacheData c = Cache.Dequeue();
            InUse.Add(c);

            c.GameObject.SetActive(true);

            c.Transform.position = pos;
            c.Transform.rotation.SetLookRotation(dir);
            
            for (int i = 0; i < c.Emitters.Length; i++)
                c.Emitters[i].emit = true;
        }
    }

    [SerializeField]
    CombatEffect Blood;
    [SerializeField]
    CombatEffect BloodBig;
    [SerializeField]
    CombatEffect BlockHit;
    [SerializeField]
    CombatEffect BlockBreak;
    [SerializeField]
    CombatEffect Critical;
    [SerializeField]
    CombatEffect Knockdown;

    [SerializeField]
    CombatEffect Spawn;
    [SerializeField]
    CombatEffect Disappear;
    [SerializeField]
    CombatEffect Whirl;
    [SerializeField]
    CombatEffect Roll;

    static public CombatEffectsManager Instance = null;

    void Awake()
    {
        if(Instance != null)
            Debug.LogError(this.name + " is singleton, somebody is creating another instance !!");

        Instance = this;

        Blood.Init(10);
        BloodBig.Init(10);
        BlockHit.Init(5);
        BlockBreak.Init(3);
        Critical.Init(4);
        Knockdown.Init(4);
        
        Whirl.Init(3);
        Roll.Init(3);

        Spawn.Init(5);
        Disappear.Init(5);
    }

    void LateUpdate()
    {

        Blood.Update();
        BloodBig.Update();
        BlockHit.Update();
        BlockBreak.Update();
        Critical.Update();
        Knockdown.Update();

        Whirl.Update();
        Roll.Update();

        Spawn.Update();
        Disappear.Update();
    }
  
    public void PlayBloodEffect(Vector3 pos, Vector3 dir)
    {
        Blood.Play(pos, dir);
    }

    public void PlayBloodBigEffect(Vector3 pos, Vector3 dir)
    {
        BloodBig.Play(pos, dir);
    }
    
    public void PlayBlockHitEffect(Vector3 pos, Vector3 dir)
    {
        BlockHit.Play(pos, dir);
    }

    public void PlayBlockBreakEffect(Vector3 pos, Vector3 dir)
    {
        BlockBreak.Play(pos, dir);
    }

    public void PlayCriticalEffect(Vector3 pos, Vector3 dir)
    {
        Critical.Play(pos, dir);
    }

    public void PlayKnockdownEffect(Vector3 pos, Vector3 dir)
    {
        Knockdown.Play(pos, dir);
    }

    public void PlaySpawnEffect(Vector3 pos, Vector3 dir)
    {
        Spawn.Play(pos, dir);
    }

    public void PlayDisappearEffect(Vector3 pos, Vector3 dir)
    {
        Disappear.Play(pos, dir);
    }

    public CacheData PlayWhirlEffect(Transform parent)
    {
        //Debug.Log(Time.timeSinceLevelLoad + " play " + c.GameObject.name);

        CacheData c = Whirl.Get();

        c.Transform.parent = parent;
        c.Transform.position = parent.position + new Vector3(0, 1.6f, 0);
        c.Transform.forward = parent.forward;
        
        for (int i = 0; i < c.Emitters.Length; i++)
            c.Emitters[i].emit = true;

        return c;
    }

    public void ReturnWhirlEffect(CacheData c)
    {
        for (int i = 0; i < c.Emitters.Length; i++)
            c.Emitters[i].emit = false;

        c.Transform.parent = null;
        Whirl.Return(c);
    }

    public CacheData PlayRollEffect(Transform parent)
    {
        CacheData c = Roll.Get();

        c.Transform.parent = parent;
        c.Transform.localPosition = new Vector3(0, 0, 0);
        c.Transform.forward = -parent.forward;

        for (int i = 0; i < c.Emitters.Length; i++)
            c.Emitters[i].emit = true;

        return c;
    }

    public void ReturnRolllEffect(CacheData c)
    {
        for (int i = 0; i < c.Emitters.Length; i++)
            c.Emitters[i].emit = false;

        c.Transform.parent = null;
        Roll.Return(c);
    }

    
    public IEnumerator PlayAndStop(ParticleEmitter emitter, float delay)
    {
        if (emitter == null)
            yield break;

        yield return new WaitForSeconds(delay);
        emitter.emit = true;

        yield return new WaitForEndOfFrame();
        emitter.emit = false;
    }
}