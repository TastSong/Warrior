using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent (typeof(BoxCollider))]
public class SpawnZone : MonoBehaviour, IEDable
{

	/// <summary>
	/// 范围出怪配置Round info.
	/// </summary>
	[System.Serializable]
	public class RoundInfo
	{
		[System.Serializable]
		public class SpawnInfo
		{
			//怪物类型
			public E_EnemyType EnemyType;
			//可能出现的点
			public SpwanPointEnemy[] SpwanPoint;
			//出生延时
			public float SpwanDelay = 0f;
			//出生是否转向玩家
			public bool RotateToPlayer = true;
			//主要Boss用，死亡后就不在刷新怪
			public bool WhenKilledStopSpawn = false;
		}
		//一个波次出怪配置
		public SpawnInfo[] Spawns;
		//每个波次出怪延时
		public float SpawnDelay = 0f;
		//下一波次出现的最小怪物数量
		public int MinEnemiesFomLastRound = 0;
	}

	//出怪状态
	public enum E_State
	{
		WaitStart,
		SpawnEnemys,
		WaitAllDead,
		Finshed,
	}

	//出怪配置信息
	public RoundInfo[] SpawnRounds;

	//所有的出怪点
	private SpwanPointEnemy[] SpwanPoints;

	public E_State State = E_State.WaitStart;

	public PadLock LockIn = null;
	public PadLock LockOut = null;


	private GameObject GameObject;
	private List<Agent> EnemiesAlive = new List<Agent>();

	private GameZone MyGameZone;
	public bool IsActive() { return EnemiesAlive.Count > 0; }
	public Agent GetEnemy(int index) { return EnemiesAlive[index]; }
	public int GetEnemyCount() { return EnemiesAlive.Count; }

	Agent ImportantAgent = null;

	void Awake ()
	{
		GameObject = gameObject;
		MyGameZone = GameObject.transform.parent.GetComponent<GameZone>();
		SpwanPoints = GetComponentsInChildren<SpwanPointEnemy> ();
	}

	public void Enable ()
	{
		//进入退出的锁
		if (LockIn != null)
			LockIn.Unlock();
		if (LockOut != null)
			LockOut.Unlock();
	}

	public void Disable ()
	{

	}

	void FixedUpdate()
	{

		for (int i = EnemiesAlive.Count - 1; i >= 0; i--)
		{
			if (EnemiesAlive[i].IsAlive == true)
				continue;

			EnemiesAlive.RemoveAt(i);
		}

		if (State != E_State.WaitAllDead)
			return;

		if (EnemiesAlive.Count == 0)
		{
			State = E_State.Finshed;

			if (LockOut != null)
				LockOut.Unlock();

//			if (SoundDataManager.Instance.IsSwitchingMusic())
//				MissionZone.Instance.SetNewMusic(SoundDataManager.Instance.CalmMusic, SoundDataManager.Instance.CalmMusicVolume, SoundDataManager.Instance.CalmMusicFadeOutTime, SoundDataManager.Instance.CalmMusicFadeInTime);

			MyGameZone.EnableAllActiveInteraction(true);
		}
	}
		
	public void Restart()
	{
		StopAllCoroutines();

		State = E_State.WaitStart;

		if (LockIn != null)
			LockIn.Reset();
		if (LockOut != null)
			LockOut.Reset();
	}

	void OnTriggerEnter (Collider other)
	{
		if (State != E_State.WaitStart || other.CompareTag ("Player") == false)
			return;

		MyGameZone.SetInProgress();

		MyGameZone.EnableAllActiveInteraction(false);

		if (SpawnRounds != null && SpawnRounds.Length > 0) {
			//开始范围出怪
			StartCoroutine (SpawnEnemysInRounds ());
		} else {
			//没有出怪配置，直接在出怪点进行刷怪
			StartCoroutine (SpawnEnemys ());
		}

		if (LockIn != null)
			LockIn.Lock();
		if (LockOut != null)
			LockOut.Lock();

//		BoxCollider box = GetComponent<BoxCollider> ();
//		box.enabled = false;
	}

	IEnumerator SpawnEnemysInRounds ()
	{
		State = E_State.SpawnEnemys;

		ImportantAgent = null;

		for (int i = 0; i < SpawnRounds.Length; i++) {
			RoundInfo round = SpawnRounds [i];
			float delay = round.SpawnDelay;
			//波次延时
			while (delay > 0) {
				if (ImportantAgent != null && ImportantAgent.IsAlive == false)
				{
					State = E_State.WaitAllDead;
					yield break;
				}

				if (EnemiesAlive.Count == 0 || EnemiesAlive.Count <= round.MinEnemiesFomLastRound)
					break;
				
				yield return new WaitForSeconds (0.5f);
				delay -= 0.5f;
			}
			//开始该波次出怪
			for (int ii = 0; ii < round.Spawns.Length; ii++) {
				RoundInfo.SpawnInfo spawnInfo = round.Spawns [ii];
				//出怪延时
				yield return new WaitForSeconds (spawnInfo.SpwanDelay);

				SpwanPointEnemy[] temp;
				if (spawnInfo.SpwanPoint != null && spawnInfo.SpwanPoint.Length > 0) {
					temp = spawnInfo.SpwanPoint;
				} else {
					temp = SpwanPoints;
				}

				//选择出怪最佳位置
				SpwanPointEnemy point = GetAvailableSpawnPoint(temp);
				//SpwanPointEnemy point = temp [0];

				//改变出生点的方向即改变怪物的朝向
				if (spawnInfo.RotateToPlayer == true) {
					Vector3 playerPos = Player.Instance.Agent.Position;
					Vector3 dir = playerPos - point.transform.position;
					dir.Normalize ();
					point.transform.forward = dir;
				}

				//开场动画
				//yield return new WaitForSeconds (0.5f);

				yield return CreateEnemy (point, spawnInfo.EnemyType);

				yield return new WaitForSeconds (1f);
			}
		}

		State = E_State.WaitAllDead;
	}


	IEnumerator SpawnEnemys ()
	{
		State = E_State.SpawnEnemys;

		yield return new WaitForEndOfFrame ();
		for (int i = 0; i < SpwanPoints.Length; i++) {
			yield return new WaitForSeconds (1f);
			//产生怪
			StartCoroutine (SpawnEnemy (SpwanPoints [i]));
								
			yield return new WaitForSeconds (Random.Range (0.5f, 1.5f));
		}
		yield return new WaitForSeconds (5f);
		State = E_State.WaitAllDead;
	}

	IEnumerator SpawnEnemy (SpwanPointEnemy point)
	{
		//todo: 开场动画什么的

		yield return new WaitForSeconds (0.5f);

		//Create Enemy
		yield return CreateEnemy (point);

		yield return new WaitForSeconds (0.1f);
	}

	IEnumerator CreateEnemy (SpwanPointEnemy point, E_EnemyType enemyType = E_EnemyType.None, RoundInfo.SpawnInfo spawnInfo = null)
	{
				
		if (enemyType == E_EnemyType.None) {
			enemyType = point.EnemyType;
		}
		//从缓存获取怪物
		//GameObject prefab = Resources.Load<GameObject> ("Prefabs/Monster");
		//Agent enemy = GameObject.Instantiate (prefab).GetComponent<Agent> ();

		Agent enemy = MissionZone.Instance.GetHuman(enemyType,point.transform);

		while(enemy == null)
		{
			yield return new WaitForSeconds(0.5f);
			enemy = MissionZone.Instance.GetHuman(enemyType, point.transform);
		}

		enemy.PrepareForStart();

		CombatEffectsManager.Instance.PlaySpawnEffect(point.Transform.position, point.Transform.forward);

		MyGameZone.AddEnemy(enemy);
		EnemiesAlive.Add(enemy);

		if (spawnInfo != null && spawnInfo.WhenKilledStopSpawn)
			ImportantAgent = enemy;
		
		yield return new WaitForSeconds(0.1f);
	}

	SpwanPointEnemy GetAvailableSpawnPoint(SpwanPointEnemy[] spawnPoints)
	{
		Vector3 pos = Player.Instance.Agent.Position;

		float bestValue = 0;
		int bestSpawn = -1; 

		for(int i = 0; i < spawnPoints.Length;i++)
		{
			if (MyGameZone.IsEnemyInRange(spawnPoints[i].transform.position, 2))
			{
				continue;
			}
			float value = 0;
			float dist = Mathf.Min(14, (spawnPoints[i].Transform.position - pos).magnitude);
			value = Mathfx.Hermite(0, 7, dist/7);

			if (value <= bestValue)
				continue;

			bestValue = value;
			bestSpawn = i;
		}

		if( bestSpawn == -1)
			return spawnPoints[Random.Range(0, spawnPoints.Length)];
		return spawnPoints[bestSpawn];
	}
				
	void OnDrawGizmos ()
	{
		BoxCollider collider = GetComponent<BoxCollider> ();
		if (collider != null) {
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube (collider.center + collider.transform.position, collider.size);
		}

		if (SpwanPoints == null)
			return;

		foreach (SpwanPointEnemy point in SpwanPoints) {
			if (collider != null)
				Gizmos.DrawLine (collider.transform.position + collider.center, point.transform.position);
			else
				Gizmos.DrawLine (transform.position, point.transform.position);
		}
	}
}
