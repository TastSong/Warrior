using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class ArrowProjectileCache : Cache
{
	private List<ArrowProjectile> Cache = new List<ArrowProjectile>();

	public override void Init()
	{
		GameObject g;
		ArrowProjectile a;
		for (int i = 0; i < MaxCount; i++)
		{
			g = GameObject.Instantiate(Prefab) as GameObject;
			g.transform.parent = AutoElementManager.Instance.transform;
			g.name = g.name + i.ToString();
			g.SetActive(false);
			g.layer = cache_free;

			a = new ArrowProjectile(g);
			Cache.Add(a);
		}
	}

	public ArrowProjectile Get()
	{
		int len = Cache.Count;
		ArrowProjectile obj;
		for (int i = 0; i < len; i++)
		{
			obj = Cache[i];
			if (obj.GameObject.layer == cache_free)
			{
				obj.GameObject.SetActive(true);
				obj.Transform.position = new Vector3(0, 0, 10000);
				obj.GameObject.layer = cache_inuse;
				return obj;
			}
		}
		return null;
	}

	public override bool Return(GameObject arrow)
	{
		int len = Cache.Count;
		for (int i = 0; i < len; i++)
		{
			if (Cache[i].GameObject == arrow)
			{
				MissionZone.Instance.StartCoroutine(Hide(arrow));
				return true;
			}
		}
		return false;
	}
		
	IEnumerator Hide (GameObject gameObject)
	{
		return Hide(gameObject, 0.1f);
	}
}


public class ArrowProjectile
{
    public GameObject GameObject;
    public Transform Transform;
    private float Damage;
    private float Speed;
    private float Timer;
    private bool Hit;
    private Agent Owner;

    public ArrowProjectile(GameObject gameobject)
    {
        GameObject = gameobject;
        Transform = GameObject.transform;
    }

    public void Init(Agent owner, Vector3 pos, Vector3 dir, float speed, float damage)
    {
        Owner = owner;

        Transform.position = pos;
        Transform.forward = dir;

        Speed = speed;
        Damage = damage;
        Timer = 0;
        Hit = false;
    }

    public void Update()
    {
        if (IsFinished())
            return;

        if (Hit == false)
        {
            Transform.position += Transform.forward * Speed * Time.deltaTime;

            if (Player.Instance.Agent.IsInvulnerable == false && Player.Instance.Agent.BlackBoard.MotionType != E_MotionType.Roll)
            {
                Vector3 arrowPos = Transform.position;
                arrowPos.y = 0;
                Vector3 playerPos = Player.Instance.Agent.Transform.position;
                playerPos.y = 0;


                if ((arrowPos - playerPos).sqrMagnitude < 0.4 * 0.4f)
                {
                    Hit = true;
                    Player.Instance.Agent.ReceiveRangeDamage(Owner, Damage, Transform.forward);
                    GameObject.SetActive(false);
                }
            }
        }

        Timer += Time.deltaTime;
    }

    public bool IsFinished()
    {
        return Timer > 3 || Hit == true;
    }
}


