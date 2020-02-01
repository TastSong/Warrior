using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Human cache.
/// </summary>
public class HumanCache : Cache
{
	//怪物类型
	public E_EnemyType EnemyType = E_EnemyType.None;
}

/// <summary>
/// 怪物模型资源
/// </summary>
[System.Serializable]
public class LiveHumanCache : HumanCache
{
	private List<GameObject> Cache = new List<GameObject> ();

	public override void Init ()
	{
		switch (EnemyType) {
		case E_EnemyType.SwordsMan:
			Prefab = Resources.Load<GameObject> ("Enemies/EnemySwordsMan");
			break;
		case E_EnemyType.Peasant:
			Prefab = Resources.Load<GameObject> ("Enemies/EnemyPeasant");
			break;
		case E_EnemyType.TwoSwordsMan:
			Prefab = Resources.Load<GameObject> ("Enemies/EnemyDoubleSwordsman");
			break;
		case E_EnemyType.Bowman:
			Prefab = Resources.Load<GameObject> ("Enemies/EnemyBowman");
			break;
		case E_EnemyType.PeasantLow:
			Prefab = Resources.Load<GameObject> ("Enemies/EnemyPeasantEasy");
			break;
		case E_EnemyType.MiniBoss01:
			Prefab = Resources.Load<GameObject> ("Enemies/EnemyMiniBoss");
			break;
		case E_EnemyType.SwordsManLow:
			Prefab = Resources.Load<GameObject> ("Enemies/EnemySwordsManEasy");
			break;
		case E_EnemyType.BossOrochi:
			Prefab = Resources.Load<GameObject> ("Enemies/EnemyBossOrochi");
			break;
		default:
			Debug.LogError ("HumanCache::Init -  Unknow enemy type");
			break;

		}

		for (int i = 0; i < MaxCount; i++) {
			GameObject go;
			go = GameObject.Instantiate<GameObject> (Prefab);
			//指定怪物类型
			go.GetComponent<Agent>().EnemyType = EnemyType;
			go.name = go.name + Cache.Count.ToString ();
			go.transform.parent = AutoElementManager.Instance.transform;
			go.SetActive (false);
			go.layer = cache_free;
			Cache.Add (go);
		}
	}

	public GameObject Get ()
	{
		int len = Cache.Count;
		GameObject go;
		for (int i = 0; i < len; i++) {
			go = Cache [i];
			if (go.layer == cache_free) {
				go.SetActive (true);
				go.transform.position = new Vector3 (0, 0, 10000);
				go.layer = cache_inuse;
				return go;
			}
		}
		return null;
	}

	public override bool Return (GameObject enemy)
	{
		int len = Cache.Count;
		GameObject go;
		for (int i = 0; i < len; i++) {
			go = Cache [i];
			if (go == enemy) {
				MissionZone.Instance.StartCoroutine (Hide (enemy));
				return true;
			}
		}
		return false;
	}
}


/// <summary>
/// 怪物死亡模型资源
/// </summary>
[System.Serializable]
public class DeadHumanCache : HumanCache
{

	private List<GameObject>[] Cache = new List<GameObject>[(int)E_DeadBodyType.Max];

	public override void Init ()
	{
		//Legs = 0, Beheaded, HalfBody, SliceFrontBack, SliceLeftRight
		//SwordsMan=0, Peasant = 1, TwoSwordsMan = 2, Bowman = 3, PeasantLow = 4, MiniBoss01=5, SwordsManLow = 6
		//7种怪有死亡模型，每个怪有5种死亡模型
		string[][] resources = {
			new string[] {"Enemies/DeadSwordsmanHLegs",
				"Enemies/DeadSwordsmanHHead",
				"Enemies/DeadSwordsmanHBody",
				"Enemies/DeadSwordsmanVFront",
				"Enemies/DeadSwordsmanVSide"
			}, //E_EnemyType.SwordsMan
			new string[] { "Enemies/DeadPeasantHLegs",
				"Enemies/DeadPeasantHHead", 
				"Enemies/DeadPeasantHBody", 
				"Enemies/DeadPeasantVFront", 
				"Enemies/DeadPeasantVSide"
			}, //E_EnemyType.Peasant
			new string[] { "Enemies/DeadTwoSwordsmanHLegs", 
				"Enemies/DeadTwoSwordsmanHHead", 
				"Enemies/DeadTwoSwordsmanHBody", 
				"Enemies/DeadTwoSwordsmanVFront", 
				"Enemies/DeadTwoSwordsmanVSide"
			}, //E_EnemyType.TwoSwordsMan
			new string[] { "Enemies/DeadBowmanH", 
				"Enemies/DeadBowmanH", 
				"Enemies/DeadBowmanH", 
				"Enemies/DeadBowmanV", 
				"Enemies/DeadBowmanV"
			}, //E_EnemyType.Bowman
			new string[] { "Enemies/DeadPeasantLowHLegs", 
				"Enemies/DeadPeasantLowHHead", 
				"Enemies/DeadPeasantLowHBody", 
				"Enemies/DeadPeasantLowVFront", 
				"Enemies/DeadPeasantLowVSide"
			}, //E_EnemyType.PeasantLow
			new string[] { }, // MiniBoss01
			new string[] { "Enemies/DeadSwordsmanLowHLegs", 
				"Enemies/DeadSwordsmanLowHHead", 
				"Enemies/DeadSwordsmanLowHBody", 
				"Enemies/DeadSwordsmanLowVFront", 
				"Enemies/DeadSwordsmanLowVSide"
			}, //E_EnemyType.SwordsManLow
		};
		GameObject go;
		for (int i = 0; i < (int)E_DeadBodyType.Max; i++) {
			Cache [i] = new List<GameObject> ();
			//非上述7种怪跳过
			Prefab = Resources.Load<GameObject> (resources [(int)EnemyType] [i]);
			for (int ii = 0; ii < MaxCount; ii++) {
				go = GameObject.Instantiate<GameObject> (Prefab);
				go.name = go.name + ii.ToString ();
				go.transform.parent = AutoElementManager.Instance.transform;
				go.SetActive (false);
				go.layer = cache_free;
				Cache [i].Add (go);
			}
		}
	}

	public GameObject Get (E_DeadBodyType type)
	{
		List<GameObject> cache = Cache [(int)type];
		GameObject obj;
		for (int i = 0; i < cache.Count; i++) {
			obj = cache [i];
			if (obj.layer == cache_free) {
				obj.SetActive (true);
				obj.transform.position = new Vector3 (0, 0, 10000);
				obj.layer = cache_inuse;
				return obj;
			}
		}
		return null;
	}

	public override bool Return (GameObject enemy)
	{
		for (int i = 0; i < Cache.Length; i++) {
			for (int ii = 0; ii < Cache [i].Count; ii++) {
				if (Cache [i] [ii] == enemy) {
					//隐藏
					MissionZone.Instance.StartCoroutine (Hide (enemy));
					return true;
				}
			}
		}
		return false;
	}

}
